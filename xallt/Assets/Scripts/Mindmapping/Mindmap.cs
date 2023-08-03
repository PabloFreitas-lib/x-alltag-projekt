using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// includes functions for the spawn position, creating, selecting and deleting nodes and the changing the mode
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>


public class Mindmap : MonoBehaviour
{
    // special nodes (root, selected, prevSelected (previously selected))
    public Node root;
    public Node selected;
    public Node prevSelected;

    // prefabs for node and connection
    [Header("Prefabs")]
    public GameObject NodePrefab;
    public GameObject connectionPrefab;

    // spawn position
    [HideInInspector]public Transform Spawnposition;
    
    // list of nodes
    public List<Node> nodes;

    // modes (default, connect, edit, delete)
    public Mode mode;
    public enum Mode
    {
        defaultMode = 0,
        ConnectMode = 1,
        EditMode = 2,
        DeleteMode = 3
    }

    /// <summary>
    /// deletes nodes (if a node is selected in delete mode and the node isn't the root) 
    /// (also removes parent-children relation in structure)
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void DeleteNode()
    {
        if(selected != null && mode == Mode.DeleteMode)
        {
            if (!selected.isRoot)
            {
                if(selected.parent != null)
                {
                    selected.parent.children.Remove(selected);
                }
                nodes.Remove(selected);
                Destroy(selected.gameObject);
                selected = null;
                mode = Mode.defaultMode;
            }
        }
    }

    /// <summary>
    /// selects nodes
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    /// <param name="Node node"> requires a node </param>
    public void SelectNode(Node node)
    {
        prevSelected = selected;
        selected = node;
        node.label.SelectSelf();
    }

    /// <summary>
    /// creates nodes
    /// determines spawn position, sets parents
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void CreateNode()
    {
        if(selected != null)
        {
            Spawnposition = selected.gameObject.transform;
        }
        else
        {
            Spawnposition = root.gameObject.transform;
        }
        Vector3 currentPosition = Spawnposition.position;
        Vector3 newPosition = new Vector3(currentPosition.x + 0.5f, currentPosition.y, currentPosition.z);
        GameObject node = Instantiate(NodePrefab, newPosition, Quaternion.identity);
        node.transform.SetParent(transform);
        Node nodeScript = node.GetComponent<Node>();
        nodeScript.mindmap = this;
        nodes.Add(nodeScript);
        
        if (selected != null)
        {
            nodeScript.parent = selected;
            selected.children.Add(nodeScript);
            nodeScript.transform.parent = nodeScript.parent.transform;
        }
    }

    /// <summary>
    /// changes the mode
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    /// <param name="int index"> requires the mode index </param>
    public void changeMode(int index)
    {
            switch (index){
                case 0: mode = Mode.defaultMode;
                    break;
                case 1: mode = Mode.ConnectMode;
                selected = null;
                        break;
                case 2: mode = Mode.EditMode;
                    break;
                case 3: mode = Mode.DeleteMode;
                break;
        }
    }
}
