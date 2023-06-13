using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Metadata
    public uint id { get; private set; }
    public DateTime creationDate;

    //Display information
    public string text;
    public Color userColor;
        //Lists or variables for each type of appendable data

    //Model
    public Node parent;                       //empty parent -> treat node as root
    public List<Node> children = new List<Node>();

    public Node(uint pId, DateTime pCreationDate)
    {
        id = pId;
        creationDate = pCreationDate;
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
