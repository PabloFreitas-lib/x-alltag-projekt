using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Selects node, establishes connections between child node and parent and node and node.
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>
public class Node : MonoBehaviour
{
    //Metadata
    public uint id;
    public Mindmap mindmap;

    //Display information
    public Label label;
    //Lists or variables for each type of appendable data

    //Model
    public Node parent;
    public List<Node> children;
    public List<Node> destinations;           //complete data model by reference to connection destinations of a node

    //Boolean
    public bool isRoot;

    /// <summary> 
    /// Gives nodes metadata.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin, Jakob </author>
    /// <param name="uint pId"> requires the node ID </param>
    /// <param name="string pText"> requires text </param>
    /// <param name="Color pUserColor"> requires Color </param>
    public Node(uint pId, string pText, Color pUserColor, Vector3 position, Vector3 size, Mindmap pmindmap)
    {
        id = pId;
        label.text.text= pText;
        this.GetComponent<ColorChanger>().objectColor = pUserColor;
        mindmap = pmindmap;
        transform.parent = mindmap.transform;
        transform.position = position;
        transform.localScale = size;
    }

    /// <summary>
    /// Calls the function ConnectToParent if the node isn't the root.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    private void Start()
    {
        label = GetComponent<Label>();
        if (!isRoot)
        {
            ConnectToParent();
        }
        
    }

    /// <summary>
    /// Connects node to parent.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    private void ConnectToParent()
    {
        GameObject connection = Instantiate(mindmap.connectionPrefab, transform.position, Quaternion.identity);
        if(parent != null)
        {
            connection.GetComponent<Connection>().SetFromTo(this, parent.GetComponent<Node>());
        }
        else
        {
            connection.GetComponent<Connection>().SetFromTo(this, mindmap.root);
        }
        connection.transform.parent = mindmap.transform;
    }

    /// <summary>
    /// Selects the node.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void SelectSelf()
    {
        if(mindmap != null)
            mindmap.SelectNode(this);
    }

    /// <summary>
    /// Sets connections between nodes.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void ConnectNodes()
    {
        if(mindmap.mode == Mindmap.Mode.ConnectMode && mindmap.prevSelected != null && mindmap.selected != null)
        {
            GameObject connection = Instantiate(mindmap.connectionPrefab, transform.position, Quaternion.identity);
            connection.GetComponent<Connection>().SetFromTo(mindmap.prevSelected, mindmap.selected);
            destinations.Add(mindmap.selected);               //complete data model by reference to connection destinations of a node
            connection.transform.parent = mindmap.transform;
            mindmap.mode = Mindmap.Mode.defaultMode;
        }
    }
}
