using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mindmap : MonoBehaviour
{
    public Node root;
    public Node selected;
    public Node prevSelected;
    [Header("Prefabs")]
    public Node NodePrefab;
    public GameObject connectionPrefab;
    [HideInInspector]public Transform Spawnposition;
    public List<Node> nodes;
    public Mode mode;

    public enum Mode
    {
        defaultMode = 0,
        ConnectMode = 1,
        EditMode = 2,
        DeleteMode = 3
    }

    public void DeleteNode()
    {
        if(selected != null)
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
            }
        }
    }
    public void SelectNode(Node node)
    {
        prevSelected = selected;
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
        Node node = Instantiate(NodePrefab, newPosition, Quaternion.identity);
        node.gameObject.transform.SetParent(transform);
        node.mindmap = this;
        nodes.Add(node);
        
        if (selected != null)
        {
            node.parent = selected;
            selected.children.Add(node);
            node.transform.parent = node.parent.transform; // nodeScript.transform.parent = nodeScript.parent.transform;
        }
    }

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
            }
    }
}
