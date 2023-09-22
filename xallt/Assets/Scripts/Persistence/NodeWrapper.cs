using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Wrapper-Class for serialization of a single mindmap node in JSON. Used to serialize a mindmap as a list of nodes.
/// </summary>
/// <author> Jakob Kern</author> 
[System.Serializable]
public class NodeWrapper
{
    public uint id { get; }
    public uint parentId { get; }

    public string text { get; }
    public Color userColor { get; }
    public Vector3 position { get; }
    public Vector3 size { get; }

    public uint[] childrenIds { get; }
    public uint[] destinationIds { get; }

    /// <summary>
    /// Called by SaveMindmap(Mindmap mindmap) to easily encapsulate a mindmaps nodes.
    /// A mindmap is primarily described by its name (which is its path) and a list of nodes.
    /// </summary>
    /// <author> Jakob Kern </author>
    /// <param name="node"> The node to be wrapped. </param>
    public NodeWrapper(Node node)
    {
        //Set simple parameters
        id = node.id;
        parentId = node.parent.id;
        text = node.label.text.text;
        position = node.transform.position;
        size = node.transform.localScale;

        //Encode userColor
        userColor = node.GetComponent<ColorChanger>().objectColor;

        //Encode childrenIds
        childrenIds = new uint[node.children.ToArray().Length];
        destinationIds = new uint[node.destinations.ToArray().Length];
        int i = 0;
        foreach (Node child in node.children)
        {
            childrenIds[i] = child.id;
            i++;
        }

        i = 0;
        foreach (Node destination in node.destinations)
        {
            childrenIds[i] = destination.id;
            i++;
        }
    }
}
