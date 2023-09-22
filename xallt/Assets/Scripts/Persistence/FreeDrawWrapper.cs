using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper-Class for serialization of a free draw scene element in JSON.
/// </summary>
/// <author> Jakob Kern</author> 
public class FreeDrawWrapper : MonoBehaviour
{
    public Vector3[] vectors;
    public Color color;

    /// <summary>
    /// Called by SaveFreeDraw(VRDrawingManager manager) to extract a managers current LineRenderer.
    /// Persitent-relevant data contains an identifier, the color and the vector positions of a free draw.
    /// </summary>
    /// <author> Jakob Kern </author>
    /// <param name="manager"> Contains the LineRender whose information is to be persisted. </param>
    public FreeDrawWrapper(LineRenderer renderer)
    {
        color = renderer.startColor;

        vectors = new Vector3[renderer.positionCount];
        renderer.GetPositions(vectors);
    }
}

