using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///    sets whiteboard texture according to the whiteboard marker
/// </summary>
/// <author> Sophia Gommeringer & Celina Dadschun </author>

public class Whiteboard : MonoBehaviour
{
    //private m_ID and getter method as string id
    [SerializeField] private string m_ID; 
    public string id => m_ID;
    // variables to describe characteristics of whiteboard texture
    public Texture2D drawingTexture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    // reference to RawImage-component of whiteboard
    public RawImage whiteboardImage;


    /// <summary>
    ///    initialize whiteboard, assign texture and wait for marker input
    /// </summary>
    /// <author> Sophia Gommeringer & Celina Dadschun </author>
    void Start()
    {
        //Assign GUID unique id to whiteboard
        m_ID = Guid.NewGuid().ToString();
        // assign texture
        var r = GetComponent<Renderer>();
        drawingTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = drawingTexture;
        // iterates over whole texture & sets color in array to white/transparent
        Color[] emptyColors = new Color[drawingTexture.width * drawingTexture.height];
        for (int i = 0; i < emptyColors.Length; i++)
        {
            emptyColors[i] = Color.white;
        }
        // overwrite the whole drawing on the whiteboard with the empty color
        drawingTexture.SetPixels(emptyColors);
        drawingTexture.Apply();

        // inititlize marker, that can draw on whiteboard
        WhiteboardMarker marker = FindObjectOfType<WhiteboardMarker>();
        // GameObject markerObject = GameObject.Find("Pen_Interactable");
        // WhiteboardMarker marker = markerObject.GetComponent<WhiteboardMarker>();
        marker.addPath(m_ID);

    }
}
