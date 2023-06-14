using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using static SaveSystem;


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
public class SaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class ComplexPlayerPrefsPersistentObject
    {
        public ComplexPlayerPrefsPersistentObject()
        {

        }
    }

    /*
    * Objects of this class have the purpose of collecting serializable data of a mindmap to be saved from or respectively be loaded to.
    */
    [System.Serializable]
    public class NodePersistentObject
    {
        public uint id {  get;  }
        public uint parentId {  get;  }
        public DateTime creationDate;

        public string text { get; }
        public float[] userColor { get; }

        public uint[] childrenIds {  get; }

        public NodePersistentObject(Node node)
        {
            //Set simple parameters
            id = node.id;
            parentId = node.parent.id;
            creationDate = node.creationDate;
            text = node.text;

            //Encode userColor
            userColor[0] = node.userColor.r;
            userColor[1] = node.userColor.g;
            userColor[2] = node.userColor.b;
            userColor[3] = node.userColor.a;

            //Encode childrenIds
            childrenIds = new uint[node.children.ToArray().Length];
            int i=0;
            foreach (Node child in node.children)
            {
                childrenIds[i] = child.id;
                i++;
            }
        }

    }


    /*
    * Objects of this class have the purpose of collecting serializable data of a whiteboard to be saved from or respectively be loaded to.
    */
    [System.Serializable]
    public class WhiteBoardPersistentObject
    {
        //this must be a texture2D now the question ist which information of texture 2D are important in unity it says 16MB per Texture which ist quit a lot 
        public Texture2D texture;
        public Vector2 textureSize; 
        public string id { get; }

        public WhiteBoardPersistentObject(Whiteboard whiteboard)
        {
            texture = whiteboard.texture;
            textureSize = whiteboard.textureSize;
            id = whiteboard.id;
        }
    }

    /*
    * Used to save general GameState.
    * (Player position, how many and which files cubes are there?, user(?))
    */
    public void saveComplexUserPrefs()
    {

    }

    /*
     * Used to load general GameState.
     * (Player position, how many and which files cubes are there?, user(?))
     */
    public void loadComplexUserPrefs()
    {

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
    public static void loadMindmap(Mindmap mindmap)
    {
        string fullPath = Path.Combine(Application.dataPath, "mindmaps");
        fullPath = Path.Combine(fullPath, mindmap.name);

        string json = System.IO.File.ReadAllText(fullPath);
        NodePersistentObject[] persistenceMapped = JsonUtility.FromJson<NodePersistentObject[]>(json);
        Node[] nodes = new Node[persistenceMapped.Length];

        uint i = 0;
        foreach (NodePersistentObject persistedNode in persistenceMapped)
        {
            Node node = new Node(persistedNode.id, persistedNode.text, persistedNode.creationDate, persistedNode.userColor);
            nodes[i] = node;
            i++;
        }

        //Convert id references to gameobject references (cannot be done in loop before because Nodes are persisted unsorted and therefore a parent or child might not yet be instantiated)
        foreach (NodePersistentObject persistedNode in persistenceMapped)
        {
            Node node = BinarySearch(nodes, persistedNode.id);
            node.parent = BinarySearch(nodes, persistedNode.parentId);
            foreach (uint childId in persistedNode.childrenIds) {
                node.children.Add(BinarySearch(nodes, childId));
            }
        }

    }

    /*
     * Called to save all Whiteboards when clicking L.
     */
    public static void saveWhiteboard()
    {
        
        //_whiteboard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
                  

        // Find all whiteboard objects and store im them in an array called perstistenceMapped[]
        //not sure if this as any advantage at alll because i want to store it seperatly anyways but maybe later to store all whiteboards in an json file
        Whiteboard[] whiteboards = Resources.FindObjectsOfTypeAll<Whiteboard>();
        WhiteBoardPersistentObject[] persistenceMapped = new WhiteBoardPersistentObject[whiteboards.Length];
        Debug.Log(whiteboards.Length);
        uint i = 0;
        foreach (Whiteboard whiteboard in whiteboards)
        {
            persistenceMapped[i] = new WhiteBoardPersistentObject(whiteboard);
            i++;
        }


       //encodes multiple whiteboards to different pngs
        int x = 0;
        foreach (WhiteBoardPersistentObject whiteboardPersistent in persistenceMapped)
        {
            byte[] whiteBoardtexture = persistenceMapped[i].texture.EncodeToPNG();


            string fullPath = Path.Combine(Application.dataPath, "whiteboards");
            fullPath = Path.Combine(fullPath, persistenceMapped[i].id);

            System.IO.File.WriteAllBytes(fullPath, whiteBoardtexture);
            Debug.Log("Whiteboard saved to: " + fullPath);
            x++;
        }
        
    }

    /*
     * Called to load Whiteboard when clicking button or on whiteboard interaction..
     */
    public static void loadWhitebaord()
    {
       // Whiteboard.texture.Apply();
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

    /*
     * Save with L key (for test purposes)
     */
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //saveMindmap();
            saveWhiteboard();
            Debug.Log("Speichern...");
        }
        
    }
}
