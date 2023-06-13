using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Metadata
    public uint id { get; private set; }
    public DateTime creationDate { get; private set; }

    //Display information
    public string text { get; private set; }
    public Color userColor { get; private set; }
    //Lists or variables for each type of appendable data

    //Model
    public Node parent;                       //empty parent -> treat node as root
    public List<Node> children = new List<Node>();

    public Node(uint pId, string pText, DateTime pCreationDate, float[] pUserColor)
    {
        id = pId;
        creationDate = pCreationDate;
        text = pText;

        userColor.r = pUserColor[0];
        userColor.g = pUserColor[1];
        userColor.b = pUserColor[2];
        userColor.a = pUserColor[3];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
