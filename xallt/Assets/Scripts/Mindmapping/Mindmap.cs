using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mindmap : MonoBehaviour
{
    public Node root;
    public Node selected;
    public GameObject NodePrefab;
    [HideInInspector]public Transform Spawnposition;
    public List<Node> nodes;

    public void DeleteNode()
    {
        if(selected != null)
        {
            if (!selected.isRoot)
            {
                selected.parent.GetComponent<Node>().children.Remove(selected.gameObject);
                nodes.Remove(selected);
                Destroy(selected.gameObject);
                selected = null;
            }
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
        Vector3 currentPosition = Spawnposition.position;
        Vector3 newPosition = new Vector3(currentPosition.x + 0.5f, currentPosition.y, currentPosition.z);
        GameObject node = Instantiate(NodePrefab, newPosition, Quaternion.identity);
        node.transform.SetParent(transform);
        Node nodeScript = node.GetComponent<Node>();
        nodeScript.mindmap = this;
        nodes.Add(nodeScript);
        
        if (selected != null)
        {
            nodeScript.parent = selected.gameObject;
            selected.children.Add(nodeScript.gameObject);
            nodeScript.transform.parent = nodeScript.parent.transform;
        }
    }
}
