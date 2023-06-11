using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Metadata
    private static uint id;
    private static DateTime creationDate;

    //Display information
    public string text;
    public Color userColor;
        //Lists or variables for each type of appendable data

    //Model
    public GameObject parent;                       //empty parent -> treat node as root
    public List<GameObject> children;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
