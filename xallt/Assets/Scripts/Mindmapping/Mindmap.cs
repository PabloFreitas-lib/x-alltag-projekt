using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mindmap : MonoBehaviour
{
    public Node root;
    public string mapName;
    public Node selected;

    public void DeleteNode()
    {
        if(selected != null)
        {
            selected = null;
            Destroy(selected.gameObject);
        }
    }
    public void SelectNode(Node node)
    {
        selected = node;
    }
}
