using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///    Class implementing functions for persisting general software-state (CUP), mindmaps, whiteboards or freeDraw.
///    JSON is used for serialization.
/// </summary>
/// <author> Noah Horn, Jakob Kern, Jie Su </author>
[System.Diagnostics.DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class SaveSystem : MonoBehaviour
{
    

    /// <summary>
    /// Wrapper-Class for serialization of single lights in JSON. Part of (CUP), not used yet.
    /// </summary>
    /// <author> Jakob Kern</author> 
    [System.Serializable]
    public class LightWrapper
    {
        uint modelID; //Modelidentifier
        Color color;
        float intesity; //float?
        Vector3 positon;
    }

    // SaveSystems reference to the whiteboard, node and connection prefabs
    [SerializeField]
    public GameObject nodePrefab;
    [SerializeField]
    public GameObject filePrefab;
    [SerializeField]
    public Connection connectionPrefab;
    [SerializeField]
    public Whiteboard whiteboardPrefab;

    //Reder-Bake-Pipeline
    [SerializeField]
    private MeshBuffer meshBuffer;

    //Runtime Data Management
    List<FreeDrawWrapper> allFreeDrawData = new List<FreeDrawWrapper>();


    /// <summary>
    /// Called on launch.
    /// </summary>
    /// <author>Jakob Kern</author>
    public void Awake()
    {
        LoadComplexUserPrefs();
    }

    public void addFreeDraw(FreeDrawWrapper freeDraw)
    {
        allFreeDrawData.Add(freeDraw);
    }

    /// <summary>
    /// Called on quit.
    /// </summary>
    /// <author>Jakob Kern</author>
    public void OnApplicationQuit()
    {
        SaveComplexUserPrefs();
    }

    /// <summary>
    /// Save the general software state (CUP).
    /// Should be called on start of the software.
    /// </summary>
    /// <author> Jakob Kern, Jie Su </author>
    public void SaveComplexUserPrefs()
    {
        string fullPath = Path.Combine(Application.dataPath, "Persistent Data");
        fullPath = Path.Combine(fullPath, "cup");
        string json = JsonUtility.ToJson(new ComplexUserPrefsWrapper());
        SaveFreeDraw();

        System.IO.File.WriteAllText(fullPath, json);
    }

    /// <summary>
    /// Load the general software state (CUP).
    /// Should be called at least when the software is closed.
    /// </summary>
    /// <author> Jakob Kern, Jie Su </author>
    public void LoadComplexUserPrefs()
    {
        string fullPath = Path.Combine(Application.dataPath, "Persistent Data");
        fullPath = Path.Combine(fullPath, "cup");

        if (System.IO.File.Exists(fullPath))
        {
            string json = System.IO.File.ReadAllText(fullPath);
            ComplexUserPrefsWrapper cup = JsonUtility.FromJson<ComplexUserPrefsWrapper>(json);

            //Lichteinstellungen setzen
            LightController lights = FindObjectOfType<LightController>();
            lights.color = cup.lightColor;
            lights.brightness = cup.lightIntensity;

            //FreeDraws laden
            LoadFreeDraw();

            //File-Objekte erstellen
            foreach (FileWrapper persistentFile in cup.files)
            {
                GameObject go = Instantiate(filePrefab, persistentFile.position, Quaternion.identity);
                go.GetComponent<File>().name = persistentFile.name;
                go.GetComponent<ColorChanger>().objectColor = persistentFile.color;
            }

            //Liste der verfügbaren Whiteboards erstellen
        }
        else
        {
            Debug.Log("User Preferences not found.");
            throw new Exception("User Preferences not found");
        }
    }

    /// <summary>
    /// Save the all Free Draws held by the VRDrawingManager.
    /// </summary>
    /// <author> Jakob Kern </author>
    /// <param name="drawing"> VRDrawingManager containing the LineRenderer </param>
    public void SaveFreeDraw()
    {
        String json = JsonUtility.ToJson(allFreeDrawData.ToArray());

        string fullPath = Path.Combine(Application.dataPath, "Pesistent Data");
        fullPath = Path.Combine(fullPath, "freeDraw");

        System.IO.File.WriteAllText(fullPath, json);
    }

    /// <summary>
    /// Load all Free Draws.
    /// </summary>
    /// <author> Jakob Kern </author>
    /// <param name="drawing"> VRDrawingManager containing the LineRenderer </param>
    public void LoadFreeDraw()
    {
        string fullPath = Path.Combine(Application.dataPath, "Pesistent Data");
        fullPath = Path.Combine(fullPath, "freeDraw");

        if (System.IO.File.Exists(fullPath))
        {
            string json = System.IO.File.ReadAllText(fullPath);

            allFreeDrawData =  JsonUtility.FromJson<FreeDrawWrapper[]>(json).ToList<FreeDrawWrapper>();
            LineRenderer renderer = gameObject.AddComponent<LineRenderer>();
            
            foreach(FreeDrawWrapper wrapper in allFreeDrawData)
            {
                renderer.SetPositions(wrapper.vectors);
                renderer.startColor = renderer.endColor = wrapper.color;
                Mesh mesh = new Mesh();
                renderer.BakeMesh(mesh, true);
                meshBuffer.addBakedDrawing(mesh, renderer.material);
            }

            return;
        }
        else
        {
            Debug.Log("No such free draw file.");
            throw new Exception("No such free draw file.");
        }
    }

    /// <summary>
    /// Save a given Mindmap. This function is to be called on GUI- or socket-interaction of the fileCube correlating with the mindmap.
    /// </summary>
    /// <author> Jakob Kern </author>
    /// <param name="socket"> The Socket containing the mindmap object to be wrapped and saved. </param>
    public void SaveMindmap(FileSocket socket)
    {
        Node[] nodes = socket.map.nodes.ToArray();
        NodeWrapper[] persistenceMapped = new NodeWrapper[nodes.Length];

        uint i = 0;
        foreach (Node node in nodes)
        {
            persistenceMapped[i] = new NodeWrapper(node);
            i++;
        }

        String json = JsonUtility.ToJson(persistenceMapped);

        string fullPath = Path.Combine(Application.dataPath, "Persistent Data");
        fullPath = Path.Combine(fullPath, "mindmaps");
        fullPath = Path.Combine(fullPath, socket.map.name);
        System.IO.File.WriteAllText(fullPath, json);
    }


    /// <summary>
    /// Load a given Mindmap. This function is to be called on GUI- or socket-interaction of the fileCube correlating with the mindmap.
    /// </summary>
    /// <param name="mindmap"> A Mindmap Object, which is allowed to be empty except for its name (correlating to path). The Function fills this Mindmap object, according to saved data. </param>
    public void LoadMindmap(FileSocket socket)
    {   
        //construct path
        string fullPath = Path.Combine(Application.dataPath, "Persistent Data");
        fullPath = Path.Combine(fullPath, "mindmaps");
        fullPath = Path.Combine(fullPath, socket.map.name);

        //check if file available
        if (System.IO.File.Exists(fullPath))
        {
            //read file contents
            string json = System.IO.File.ReadAllText(fullPath);

            //deserialize
            NodeWrapper[] persistenceMapped = JsonUtility.FromJson<NodeWrapper[]>(json);
            Node[] nodes = new Node[persistenceMapped.Length];

            //create Nodes from persisted data
            uint i = 0;
            foreach (NodeWrapper persistedNode in persistenceMapped)
            {
                GameObject go = Instantiate(nodePrefab, persistedNode.position, Quaternion.identity);
                Node node = go.GetComponent<Node>();
                node.id = persistedNode.id;
                node.label.text.text = persistedNode.text;
                go.GetComponent<ColorChanger>().objectColor = persistedNode.userColor;
                node.mindmap = socket.map;
                go.transform.parent = socket.map.transform;
                go.transform.position = persistedNode.position;
                go.transform.localScale = persistedNode.size;
                nodes[i] = node;
                i++;
            }

            //Convert id references to gameobject references (cannot be done in loop before because Nodes are persisted unsorted and therefore a parent or child might not yet be instantiated)
            foreach (NodeWrapper persistedNode in persistenceMapped)
            {
                Node node = BinarySearch(nodes, persistedNode.id);
                node.parent = BinarySearch(nodes, persistedNode.parentId);
                foreach (uint childId in persistedNode.childrenIds)
                {
                    node.children.Add(BinarySearch(nodes, childId));
                }

                // Add connections
                foreach (uint destinationId in persistedNode.destinationIds)
                {
                    Node destination = BinarySearch(nodes, destinationId);
                    GameObject connection = Instantiate(socket.map.connectionPrefab, node.transform.position, Quaternion.identity);
                    connection.GetComponent<Connection>().SetFromTo(node, destination);
                    node.destinations.Add(destination);               //complete data model by reference to connection destinations of a node
                    connection.transform.parent = socket.map.transform;
                }

            }

            return;
        }
        else
        {
            Debug.Log("No such mindmap file.");
            throw new Exception("No such mindmap file.");
        }
    }


    /// <summary>
    /// Save a given Whiteboard. This function is to be called on GUI- or socket-interaction.
    /// </summary>
    /// <author>Noah Horn</author>
    /// <param name="whiteboard"> A whiteboard that is to be persisted </param>
    public void SaveWhiteboard(Whiteboard whiteboard)
    {
        byte[] whiteBoardtexture = whiteboard.drawingTexture.EncodeToPNG();

        string fullPath = Path.Combine(Application.dataPath, "Persistent Data");
        fullPath = Path.Combine(fullPath, "whiteboards");
        fullPath = Path.Combine(fullPath, whiteboard.id);
        fullPath = Path.Combine(fullPath, ".png");

        System.IO.File.WriteAllBytes(fullPath, whiteBoardtexture);
        Debug.Log("Whiteboard saved to: " + fullPath);
    }

    /// <summary>
    /// Save a given Whiteboard. This function is to be called on GUI- or socket-interaction.
    /// </summary>
    /// <author>Noah Horn</author>
    /// <param name="whiteboard"> A Whiteboard object, which is allowed to be empty except for its id (correlating to path). Will be filled with stored information.</param>
    public void LoadWhiteboard(Whiteboard whiteboard) 
    {

        Texture2D texture;
        byte[] fileData;
        string fullPath = Path.Combine(Application.dataPath, "Persistent Data");
        fullPath = Path.Combine(fullPath, "whiteboards");
        fullPath = Path.Combine(fullPath, whiteboard.id);
        fullPath = Path.Combine(fullPath, ".png");

        if (System.IO.File.Exists(fullPath))
        {
            fileData = System.IO.File.ReadAllBytes(fullPath);
            texture = new Texture2D((int)whiteboard.textureSize.x, (int)whiteboard.textureSize.y);
            texture.LoadImage(fileData);
            whiteboard.drawingTexture = texture;
            Debug.Log("Whiteboard loaded");
        }
        else
        {
            Debug.Log("There is no save to this whiteboard");
            // "Nicht jeder Pfad hatte eine R�ckgabe" Fehler - Dmitry
        }
    }


    /*
     * Helper for finding loaded information for specific node quickly by id. Works for all arrays of objects with ids.
     */
    public static Node BinarySearch(Node[] nodes, uint targetId)
    {
        int low = 0;
        int high = nodes.Length - 1;

        QuickSort(nodes, low, high);

        while (low <= high)
        {
            int mid = low + (high - low) / 2;
            if (nodes[mid].id == targetId)
            {
                return nodes[mid];
            }
            else if (nodes[mid].id > targetId)
            {
                high = mid - 1;
            }
            else
            {
                low = mid + 1;
            }
        }

        return null;  // TargetId not found
    }

    /**
     * Helper to QuickSort Elements of type NodeWrapper by id.
     */
    public static void QuickSort(Node[] nodes, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(nodes, low, high);
            QuickSort(nodes, low, pivotIndex - 1);
            QuickSort(nodes, pivotIndex + 1, high);
        }
    }

    /*
     * Helper for QuickSort by node.id
     */
    public static int Partition(Node[] nodes, int low, int high)
    {
        int pivot = (int)nodes[high].id;
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (nodes[j].id < pivot)
            {
                i++;
                Swap(nodes, i, j);
            }
        }

        Swap(nodes, i + 1, high);
        return i + 1;
    }

    /*
     * Helper for Partition
     */
    public static void Swap(Node[] nodes, int i, int j)
    {
        Node temp = nodes[i];
        nodes[i] = nodes[j];
        nodes[j] = temp;
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }


    
   
}
    