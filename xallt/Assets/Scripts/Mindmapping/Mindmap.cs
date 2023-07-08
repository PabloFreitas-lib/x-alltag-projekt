using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mindmap : MonoBehaviour
{
    public Node root;
    public Node selected;
    public Node prevSelected;
    [Header("Prefabs")]
    public GameObject NodePrefab;
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
        if(selected != null && mode == Mode.DeleteMode)
        {
            if (!selected.isRoot)
            {
                if(selected.parent != null)
                {
                    selected.parent.GetComponent<Node>().children.Remove(selected.gameObject);
                }
                nodes.Remove(selected);
                Destroy(selected.gameObject);
                selected = null;
                mode = Mode.defaultMode;
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
