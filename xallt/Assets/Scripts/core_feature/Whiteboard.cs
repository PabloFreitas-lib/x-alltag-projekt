
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Whiteboard : MonoBehaviour
{
    public string id;
    public Texture2D drawingTexture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    public RawImage whiteboardImage; // Referenz zum RawImage-Komponenten des Whiteboards

    void Start()
    {
        var r = GetComponent<Renderer>();
        drawingTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = drawingTexture;
        Color[] emptyColors = new Color[drawingTexture.width * drawingTexture.height];
        for (int i = 0; i < emptyColors.Length; i++)
        {
            emptyColors[i] = Color.white; // Setze die Farbe auf Weiß (transparent)
        }

        GameObject markerObject = GameObject.Find("Pen_Interactable");
        WhiteboardMarker marker = markerObject.GetComponent<WhiteboardMarker>();
        marker.addPath(id);

        // Überschreibe die gesamte Zeichnung auf dem Whiteboard mit der leeren Farbe
        drawingTexture.SetPixels(emptyColors);
        drawingTexture.Apply();
    }
    
    public void LoadDrawing(string whichFilePath)
    {
        // Öffne den Dateiauswahl-Dialog
        whichFilePath = EditorUtility.OpenFilePanel("Select Drawing", "", "png");

        // Überprüfe, ob eine Datei ausgewählt wurde
        if (!string.IsNullOrEmpty(whichFilePath))
        {
            // Lade das PNG-Bild von der ausgewählten Datei
            byte[] fileData = System.IO.File.ReadAllBytes(whichFilePath);

            // Erstelle ein neues Texture2D-Objekt
            Texture2D loadedTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
            loadedTexture.LoadImage(fileData);

            // Übertrage die Pixel des geladenen Texture2D-Objekts auf das Whiteboard-Texture
            drawingTexture.SetPixels(loadedTexture.GetPixels());
            drawingTexture.Apply();

            // Aktualisiere das Whiteboard-Image, um die Änderungen anzuzeigen
            whiteboardImage.texture = drawingTexture;
        }
    }
}
