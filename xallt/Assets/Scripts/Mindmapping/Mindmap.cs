using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mindmap : MonoBehaviour
{
    public Node root;
    public string mapName;
    public Node selected;
    public GameObject NodePrefab;
    public Transform Spawnposition;

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
        GameObject node = Instantiate(NodePrefab, Spawnposition.position, Quaternion.identity);
    }
}
