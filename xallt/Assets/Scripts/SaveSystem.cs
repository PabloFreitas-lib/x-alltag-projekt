using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using static SaveSystem;
using UnityEngine.EventSystems;


/* TODO:
 *  
 *  ComplexPlayerPrefs:
 *      -everything
 *  Mindmap:
 *      - move nodes into mindmap container, so it can be disabled in unity hirachy
 *      - add remaining Attributes to Save/Load Cycle
 *          - gameObject related (Mesh, Scale, Material, Position, Components)
 * 
 */
[System.Diagnostics.DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class SaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class ComplexUserPrefsPersistentObject
    {
        public Vector3 playerPosition;

        public LightPersistentObject[] lights;
        public FilePersistentObject[] files;

        public ComplexUserPrefsPersistentObject()
        {
            playerPosition = GameObject.Find("XR Origin").transform.position; //overwriten by HMD Position Tracking

            //Add Tags to acutal Files
            int f = 0;
            File[] filesToEncode = Resources.FindObjectsOfTypeAll<File>();
            files = new FilePersistentObject[filesToEncode.Length];
            foreach (File file in filesToEncode) 
            {
                files[f] = new FilePersistentObject(file);
                f++;
            }

            /* Lookup actual implementation of Lights and and Tags
            static List<Light> lightsToEncode = GameObject.FindGameObjectsWithTag("Light");
            foreach (GameObject light in GameObject.FindGameObjectsWithTag("Light"))
            {
                lights.Add(new FilePersistentObject(light));
            }
            */

        }
    }

    [System.Serializable]
    public class FilePersistentObject
    {
        string name;
        Color color;
        Vector3 position;

        public FilePersistentObject(File file) 
        {
            name = file.name;
            color = file.userColor;
            position = file.gameObject.transform.position;
        }
    }

    [System.Serializable]
    public class LightPersistentObject
    {
            uint modelID; //Modelidentifier
            Color color;
            Vector3 positon; 
    }



    /*
    * Objects of this class have the purpose of collecting serializable data of a mindmap to be saved from or respectively be loaded to.
    */
    [System.Serializable]
    public class NodePersistentObject
    {
        public uint id { get; }
        public uint parentId { get; }

        public string text { get; }
        public Color userColor { get; }

        public uint[] childrenIds { get; }

        public NodePersistentObject(Node node)
        {
            //Set simple parameters
            id = node.id;
            parentId = node.parent.id;
            text = node.text;

            //Encode userColor
            userColor = node.userColor;

            //Encode childrenIds
            childrenIds = new uint[node.children.ToArray().Length];
            int i = 0;
            foreach (Node child in node.children)
            {
                childrenIds[i] = child.id;
                i++;
            }
        }

    }


    /*
    * Used to save general GameState.
    * (Player position, how many and which files cubes are there?, user(?))
    */
    public void saveComplexUserPrefs()
    {
        string fullPath = Path.Combine(Application.dataPath, "cup");
        string json = JsonUtility.ToJson(new ComplexUserPrefsPersistentObject()); 

        System.IO.File.WriteAllText(fullPath, json);


        Debug.Log("User prefs saved" + json);
    }

    /*
     * Used to load general GameState.
     * (Player position, how many and which files cubes are there?, user(?))
     */
    public void loadComplexUserPrefs()
    {
        string fullPath = Path.Combine(Application.dataPath, "cup");

        if (System.IO.File.Exists(fullPath))
        {
            string json = System.IO.File.ReadAllText(fullPath);
            ComplexUserPrefsPersistentObject cup = JsonUtility.FromJson<ComplexUserPrefsPersistentObject>(json);

            GameObject.Find("XR Origin").transform.position = cup.playerPosition;

            //File Objekte erstellen
            //Lichtobjekte erstellen


            Debug.Log("User prefs loaded" + cup.playerPosition.ToString());
        }
        else
        {
            Debug.Log("User Preferences not found.");
        }
    }

    /*
     * Called to save Mindmap when clicking button or on socket interaction.
     */
    public static void saveMindmap(Mindmap mindmap)
    {
        Node[] nodes = Resources.FindObjectsOfTypeAll<Node>();
        NodePersistentObject[] persistenceMapped = new NodePersistentObject[nodes.Length];

        uint i = 0;
        foreach (Node node in nodes)
        {
            persistenceMapped[i] = new NodePersistentObject(node);
            i++;
        }

        String json = JsonUtility.ToJson(persistenceMapped);

        string fullPath = Path.Combine(Application.dataPath, "mindmaps");
        fullPath = Path.Combine(fullPath, mindmap.name);
        System.IO.File.WriteAllText(fullPath, json);
    }


    /*
     * Called to load Mindmap when clicking button or on socket interaction.
     */
    public static List<Node> loadMindmap(Mindmap mindmap)
    {
        string fullPath = Path.Combine(Application.dataPath, "mindmaps");
        fullPath = Path.Combine(fullPath, mindmap.name);

        string json = System.IO.File.ReadAllText(fullPath);
        NodePersistentObject[] persistenceMapped = JsonUtility.FromJson<NodePersistentObject[]>(json);
        Node[] nodes = new Node[persistenceMapped.Length];

        uint i = 0;
        foreach (NodePersistentObject persistedNode in persistenceMapped)
        {
            Node node = new Node(persistedNode.id, persistedNode.text, persistedNode.userColor);
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
        }

        return nodes.ToList();
    }
    
    /*
     * Called to save all Whiteboards when clicking L.
     */
    public static void saveWhiteboard(Whiteboard whiteboard)
    {
        byte[] whiteBoardtexture = whiteboard.texture.EncodeToPNG();


        string fullPath = Path.Combine(Application.dataPath, "whiteboards");
            fullPath = Path.Combine(fullPath, whiteboard.id);
            fullPath = Path.Combine(fullPath, ".png");

            System.IO.File.WriteAllBytes(fullPath, whiteBoardtexture);
            Debug.Log("Whiteboard saved to: " + fullPath);
     }

    /*
     * Called to load Whiteboard when clicking button or on whiteboard interaction..
     * Which argument use to load just ID 
     */
    public static void loadWhitebaord(Whiteboard whiteboard)
    {

        Texture2D texture;
        byte[] fileData;
        string fullPath = Path.Combine(Application.dataPath, "whiteboards");
        fullPath = Path.Combine(fullPath, whiteboard.id);
        fullPath = Path.Combine(fullPath, ".png");

        if (System.IO.File.Exists(fullPath))
        {
            fileData = System.IO.File.ReadAllBytes(fullPath);
            texture = new Texture2D((int)whiteboard.textureSize.x, (int)whiteboard.textureSize.y); 
            texture.LoadImage(fileData);
            whiteboard.texture = texture; 
            Debug.Log("Whiteboard loaded");
        }
        else
        {
            Debug.Log("There is no save to this whiteboard");
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            saveComplexUserPrefs();
        }else if (Input.GetKeyDown(KeyCode.L)) 
        {
            loadComplexUserPrefs();
        }
    }  
}
