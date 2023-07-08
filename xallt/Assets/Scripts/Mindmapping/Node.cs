using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Metadata
    public uint id;
    public DateTime creationDate;
    public Mindmap mindmap;

    //Display information
    public string text;
    //Lists or variables for each type of appendable data

    //Model
    public Node parent;                       //empty parent -> treat node as root
    public List<Node> children;

    //Boolean
    public bool isRoot;

    public Node(uint pId, string pText, Color pUserColor)
    {
        id = pId;
        text = pText;
        //userColor = pUserColor;
    }


    private void Start()
    {
        if (!isRoot)
        {
            ConnectToParent();
        }
        
    }

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
    public void SelectSelf()
    {
        if(mindmap != null)
            mindmap.SelectNode(this);
        
    }

    public void ConnectNodes()
    {
        if(mindmap.mode == Mindmap.Mode.ConnectMode && mindmap.prevSelected != null && mindmap.selected != null)
        {
            GameObject connection = Instantiate(mindmap.connectionPrefab, transform.position, Quaternion.identity);
            connection.GetComponent<Connection>().SetFromTo(mindmap.prevSelected, mindmap.selected);
            connection.transform.parent = mindmap.transform;
            mindmap.mode = Mindmap.Mode.defaultMode;
        }
    }
}
