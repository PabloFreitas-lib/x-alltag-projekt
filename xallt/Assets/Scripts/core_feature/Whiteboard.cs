using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///    sets whiteboard texture according to the whiteboard marker
/// </summary>
/// <author> Sophia Gommeringer & Celina Dadschun </author>

public class Whiteboard : MonoBehaviour
{
    // variables to describe characteristics of whiteboard texture
    public string id;
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
        GameObject markerObject = GameObject.Find("Pen_Interactable");
        WhiteboardMarker marker = markerObject.GetComponent<WhiteboardMarker>();
        marker.addPath(id);

    }


    /// <summary>
    ///    load and open existing whiteboard drawing
    /// </summary>
    /// <author> Celina Dadschun </author>
    /// <param name="whichFilePath"> file path of to be opened png </param>
    public void LoadDrawing(string whichFilePath)
    {
        // open file-choosing dialog
        whichFilePath = EditorUtility.OpenFilePanel("Select Drawing", "", "png");

        // check if file is selected
        if (!string.IsNullOrEmpty(whichFilePath))
        {
            // load png of selected file
            byte[] fileData = System.IO.File.ReadAllBytes(whichFilePath);

            // create new Texture2D-object
            Texture2D loadedTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
            loadedTexture.LoadImage(fileData);

            // transfer pixels of loaded Texture2D-object onto the whiteboard-texture
            drawingTexture.SetPixels(loadedTexture.GetPixels());
            drawingTexture.Apply();

            // update whiteboard-image to display changes
            whiteboardImage.texture = drawingTexture;
        }
    }
}
