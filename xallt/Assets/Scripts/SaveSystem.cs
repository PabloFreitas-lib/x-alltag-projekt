using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem
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
        public uint id {  get;  private set; }
        public uint parentId {  get;  private set; }

        public uint[] childrenIds {  get; private set; }

        public NodePersistentObject(Node node)
        {

        }

    }

    /*
    * Objects of this class have the purpose of collecting serializable data of a whiteboard to be saved from or respectively be loaded to.
    */
    [System.Serializable]
    public class WhiteboardPersistentObject
    {
        public WhiteboardPersistentObject()
        {

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

        //Actual saving via OS Interface
    }


    /*
     * Called to load Mindmap when clicking button or on socket interaction.
     */
    public static void loadMindmap()
    {
        //Actual loading via OS Interface
        string json = "";
        NodePersistentObject[] persistenceMapped = JsonUtility.FromJson<NodePersistentObject[]>(json);
        Node[] nodes = new Node[persistenceMapped.Length];

        uint i = 0;
        foreach (NodePersistentObject persistedNode in persistenceMapped)
        {
            nodes[i] = new Node(); //calls constructor in node class
            i++;
        }

        //Convert id references to gameobject references (cannot be done in loop before because Nodes are persisted unsorted and therefore a parent or child might not yet be instantiated)
        foreach (NodePersistentObject persistedNode in persistenceMapped)
        {
            Node node = BinarySearch(nodes, persistedNode.id);
            node.parent = (Node)BinarySearch(nodes, persistedNode.parentId);
            foreach (uint childId in persistedNode.childrenIds) {
                node.children.Add((Node)BinarySearch(nodes, childId));
            }
        }

    }

    /*
     * Called to save Whiteboard when clicking button or on whiteboard interaction.
     */
    public static void saveWhiteboard()
    {

    }

    /*
     * Called to load Whiteboard when clicking button or on whiteboard interaction..
     */
    public static void loadWhitebaord()
    {

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
}
