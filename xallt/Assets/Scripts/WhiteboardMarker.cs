using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class WhiteboardMarker : MonoBehaviour
{
    [FormerlySerializedAs("_tip")] [SerializeField] private Transform tip;
    [FormerlySerializedAs("_penSize")] [SerializeField] private int penSize = 5;

    public List<string> paths = new List<string>();

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;

    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;
    private int _drawingId;

    void Start()
    {
        _renderer = tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
        _tipHeight = tip.localScale.y;
    }

    public void addPath(string id)
    {
        paths.Add(id);
    }

    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        // Generate a new unique drawing ID
        _drawingId = GenerateUniqueId();
        if (Physics.Raycast(tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (penSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (penSize / 2));

                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x) return;

                if (_touchedLastFrame)
                {
                    _whiteboard.drawingTexture.SetPixels(x, y, penSize, penSize, _colors);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.drawingTexture.SetPixels(lerpX, lerpY, penSize, penSize, _colors);
                    }

                    transform.rotation = _lastTouchRot;

                    _whiteboard.drawingTexture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }
        
        _touchedLastFrame = false;
    }
    
    /**
     * SaveDrawing() leitet das Speichern des Whiteboards ein, indem SaveTexture() aufgerufen wird.
     */
    public void SaveDrawing()
    {
        Debug.Log("Drawing saved");
        // Speichern der Zeichnung, z. B. mit der SaveTexture-Methode
        SaveTexture(_whiteboard.drawingTexture);
    }
    
    /**
     * SaveTexture() speichert eine Textur im .png-Format auf einem gewünschten Pfad.
     */
    private void SaveTexture(Texture2D texture)
    {
        string fileName = "Drawing_"+ _drawingId +".png";
        string filePath = Application.persistentDataPath + "/" + fileName;

        //byte[] pngData = _whiteboard.drawingTexture.EncodeToPNG();
        byte[] textureBytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, textureBytes);

        Debug.Log("Drawing saved to: " + filePath);
    }
    
    /**
     * GenerateUniqueId() generiert eine einzigartige ID.
     */
    private int GenerateUniqueId()
    {
        // Generate a unique ID here (e.g., using a timestamp, random number, etc.)
        return System.DateTime.Now.GetHashCode();
    }

    /**
     * Mithilfe von ClearDrawing() kann ein Whiteboard von einer Zeichnung befreit werden.
     */
    public void ClearDrawing()
    {
        //_whiteboard = null;
        Color[] emptyColors = new Color[_whiteboard.drawingTexture.width * _whiteboard.drawingTexture.height];
        for (int i = 0; i < emptyColors.Length; i++)
        {
            emptyColors[i] = Color.white; // Setze die Farbe auf Weiß (transparent)
        }

        // Überschreibe die gesamte Zeichnung auf dem Whiteboard mit der leeren Farbe
        _whiteboard.drawingTexture.SetPixels(emptyColors);
        _whiteboard.drawingTexture.Apply();
    }
    
    
}
