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
    public void Awake()
    {
        LoadComplexUserPrefs();
    }

    public void OnApplicationQuit()
    {
        SaveComplexUserPrefs();
    }

    /// <summary>
    /// Wrapper-Class for serialization of general software-state (CUP) in JSON.
    /// </summary>
    /// <author> Jakob Kern</author>    
    [System.Serializable]
    public class ComplexUserPrefsPersistentObject
    {
        //Central Lighting values
        public Color lightColor;
        public float lightIntensity;

        //Files and Whiteboards existent in the scene
        public FilePersistentObject[] files;
        public string[] whiteboards;


        /// <summary>
        ///    Called by SaveComplexUserPrefs() to easily encapsulate necessary game state data.
        /// </summary>
        /// <author> Jakob Kern </author>
        public ComplexUserPrefsPersistentObject()
        {
            LightController lights = GameObject.Find("Lights").GetComponent<LightController>();
            lightColor = lights.color;
            lightIntensity = lights.brightness;

            //Add Tags to acutal Files
            int f = 0;
            File[] filesToEncode = Resources.FindObjectsOfTypeAll<File>();
            files = new FilePersistentObject[filesToEncode.Length];

            foreach (File file in filesToEncode)
            {
                files[f] = new FilePersistentObject(file);
                f++;
            }

            GameObject markerObject = GameObject.Find("Pen_Interactable");
            WhiteboardMarker marker = markerObject.GetComponent<WhiteboardMarker>();
            whiteboards = marker.paths.ToArray();
        }
    }

    /// <summary>
    /// Wrapper-Class for serialization of fileCubes in JSON. Part of CUP.
    /// </summary>
    /// <author> Jakob Kern </author> 
    [System.Serializable]
    public class FilePersistentObject
    {
        string name;
        Color color;
        Vector3 position;

        /// <summary>
        /// Called by CUP Constructor to get a serializable fileCube representation.
        /// </summary>
        /// <author> Autoren </author>
        /// <param name="file"> The File to be serialized. </param>
        public FilePersistentObject(File file)
        {
            name = file.name;
            color = file.GetComponent<ColorChanger>().objectColor; // Access ColorChanger Script of file
            position = file.gameObject.transform.position;
        }
    }

    /// <summary>
    /// Wrapper-Class for serialization of single lights in JSON. Part of (CUP), not used yet.
    /// </summary>
    /// <author> Jakob Kern</author> 
    [System.Serializable]
    public class LightPersistentObject
    {
        uint modelID; //Modelidentifier
        Color color;
        float intesity; //float?
        Vector3 positon;
    }

    /// <summary>
    /// Wrapper-Class for serialization of a free draw scene element in JSON.
    /// </summary>
    /// <author> Jakob Kern</author> 
    public class FreeDrawPersistentObject
    {
        uint id;
        Color color;
        Vector3[] vectors;

        /// <summary>
        /// Called by SaveFreeDraw(VRDrawingManager manager) to extract a managers current LineRenderer.
        /// Persitent-relevant data contains an identifier, the color and the vector positions of a free draw.
        /// </summary>
        /// <author> Jakob Kern </author>
        /// <param name="manager"> Contains the LineRender whose information is to be persisted. </param>
        public FreeDrawPersistentObject(VRDrawingManager manager)
        {
            id = manager.id;
            color = manager.tipMaterial.color;

            vectors = new Vector3[manager._currentDrawing.positionCount];              //still private in CORE Version of VRDrawingManager, change to param maybe?
            manager._currentDrawing.GetPositions(vectors);
        }
    }

    /// <summary>
    ///  Wrapper-Class for serialization of a single mindmap node in JSON. Used to serialize a mindmap as a list of nodes.
    /// </summary>
    /// <author> Jakob Kern</author> 
    [System.Serializable]
    public class NodePersistentObject
    {
        public uint id { get; }
        public uint parentId { get; }

        public string text { get; }
        public Color userColor { get; }
        public Vector3 position { get; }
        public Vector3 size { get; }

        public uint[] childrenIds { get; }
        public uint[] destinationIds { get; }

        /// <summary>
        /// Called by SaveMindmap(Mindmap mindmap) to easily encapsulate a mindmaps nodes.
        /// A mindmap is primarily described by its name (which is its path) and a list of nodes.
        /// </summary>
        /// <author> Jakob Kern </author>
        /// <param name="node"> The node to be wrapped. </param>
        public NodePersistentObject(Node node)
        {
            //Set simple parameters
            id = node.id;
            parentId = node.parent.id;
            text = node.text;
            position = node.transform.position;
            size = node.transform.localScale;

            //Encode userColor
            userColor = node.GetComponent<ColorChanger>().objectColor;

            //Encode childrenIds
            childrenIds = new uint[node.children.ToArray().Length];
            destinationIds = new uint[node.destinations.ToArray().Length];
            int i = 0;
            foreach (Node child in node.children)
            {
                childrenIds[i] = child.id;
                i++;
            }

            i = 0;
            foreach (Node destination in node.destinations)
            {
                childrenIds[i] = destination.id;
                i++;
            }
        }
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
        string json = JsonUtility.ToJson(new ComplexUserPrefsPersistentObject());

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
            ComplexUserPrefsPersistentObject cup = JsonUtility.FromJson<ComplexUserPrefsPersistentObject>(json);

            //Lichteinstellungen setzen
            LightController lights = FindObjectOfType<LightController>();
            lights.color = cup.lightColor;
            light.brightness = cup.lightIntensity;

            //File-Objekte erstellen
            foreach (FilePersistentObject persistentFile in cup.files)
            {
                
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
    /// Save the current FreeDraw by use of VRDrawingManager.
    /// </summary>
    /// <author> Jakob Kern </author>
    /// <param name="drawing"> VRDrawingManager containing the LineRenderer </param>
    public void SaveFreeDraw(VRDrawingManager drawing)
    {
        FreeDrawPersistentObject freeDraw = new FreeDrawPersistentObject(drawing);

        string json = JsonUtility.ToJson(freeDraw);

        string fullPath = Path.Combine(Application.dataPath, "Pesistent Data");
        fullPath = Path.Combine(fullPath, "freeDraw");
        fullPath = Path.Combine(fullPath, drawing.id.ToString());

        System.IO.File.WriteAllText(fullPath, json);
    }

    /// <summary>
    /// Load a FreeDraw by its id.
    /// </summary>
    /// <author> Jakob Kern </author>
    /// <param name="drawing"> VRDrawingManager containing the LineRenderer </param>
    /// <returns> An object containing all the (now deserialized) data of the freeDraw with id=drawingID </returns>
    public FreeDrawPersistentObject LoadFreeDraw(uint drawingID)
    {
        string fullPath = Path.Combine(Application.dataPath, "Pesistent Data");
        fullPath = Path.Combine(fullPath, "freeDraw");
        fullPath = Path.Combine(fullPath, drawingID.ToString());

        if (System.IO.File.Exists(fullPath))
        {
            string json = System.IO.File.ReadAllText(fullPath);

            FreeDrawPersistentObject positions = JsonUtility.FromJson<FreeDrawPersistentObject>(json);
            return positions;
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
    /// <param name="mindmap"> The Mindmap object to be wrapped and saved. </param>
    public static void SaveMindmap(Mindmap mindmap)
    {
        Node[] nodes = mindmap.nodes.ToArray();
        NodePersistentObject[] persistenceMapped = new NodePersistentObject[nodes.Length];

        uint i = 0;
        foreach (Node node in nodes)
        {
            persistenceMapped[i] = new NodePersistentObject(node);
            i++;
        }

        String json = JsonUtility.ToJson(persistenceMapped);

        string fullPath = Path.Combine(Application.dataPath, "Persistent Data");
        fullPath = Path.Combine(fullPath, "mindmaps");
        fullPath = Path.Combine(fullPath, mindmap.name);
        System.IO.File.WriteAllText(fullPath, json);
    }


    /// <summary>
    /// Load a given Mindmap. This function is to be called on GUI- or socket-interaction of the fileCube correlating with the mindmap.
    /// </summary>
    /// <param name="mindmap"> A Mindmap Object, which is allowed to be empty except for its name (correlating to path). The Function fills this Mindmap object, according to saved data. </param>
    /// <returns> A list of all the nodes that have been succesfully deserialized. Not needed for implementation, but helpful for debugging.</returns>
    public static List<Node> LoadMindmap(Mindmap mindmap)
    {   
        //construct path
        string fullPath = Path.Combine(Application.dataPath, "Persistent Data");
        fullPath = Path.Combine(fullPath, "mindmaps");
        fullPath = Path.Combine(fullPath, mindmap.name);

        //check if file available
        if (System.IO.File.Exists(fullPath))
        {
            //read file contents
            string json = System.IO.File.ReadAllText(fullPath);

            //deserialize
            NodePersistentObject[] persistenceMapped = JsonUtility.FromJson<NodePersistentObject[]>(json);
            Node[] nodes = new Node[persistenceMapped.Length];

            //create Nodes from persisted data
            uint i = 0;
            foreach (NodePersistentObject persistedNode in persistenceMapped)
            {
                Node node = new Node(persistedNode.id, persistedNode.text, persistedNode.userColor, persistedNode.position, persistedNode.size, mindmap);
                nodes[i] = node;
                i++;
            }

            //Convert id references to gameobject references (cannot be done in loop before because Nodes are persisted unsorted and therefore a parent or child might not yet be instantiated)
            foreach (NodePersistentObject persistedNode in persistenceMapped)
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
                    GameObject connection = Instantiate(mindmap.connectionPrefab, node.transform.position, Quaternion.identity);
                    connection.GetComponent<Connection>().SetFromTo(node, destination);
                    node.destinations.Add(destination);               //complete data model by reference to connection destinations of a node
                    connection.transform.parent = mindmap.transform;
                }

            }

            return nodes.ToList();
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
    /// <returns> Texture2D that shows the loaded whiteboard. Caller can ignore this, as it is the same contained in the given Whiteboard, though helpful for debugging.</returns>
    public void LoadWhiteboard(Whiteboard whiteboard) //maybe change to returning fresh Whiteboard
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
     * Helper to QuickSort Elements of type NodePersistentObject by id.
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


    /*
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveComplexUserPrefs();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadComplexUserPrefs();
        }
    }
}*/
    }