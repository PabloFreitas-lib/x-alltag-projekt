using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 5;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;

    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;
    private int _drawingId = 0;

    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
        _tipHeight = _tip.localScale.y;
    }

    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        // Generate a new unique drawing ID
        _drawingId = GenerateUniqueId();
        if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize / 2));

                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x) return;

                if (_touchedLastFrame)
                {
                    _whiteboard.drawingTexture.SetPixels(x, y, _penSize, _penSize, _colors);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.drawingTexture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
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
    
    public void SaveDrawing()
    {
        Debug.Log("Drawing saved");
        // Speichern der Zeichnung, z. B. mit der SaveTexture-Methode
        SaveTexture(_whiteboard.drawingTexture);
    }
    
    private void SaveTexture(Texture2D texture)
    {
        string fileName = "Drawing_"+ _drawingId +".png";
        string filePath = Application.persistentDataPath + "/" + fileName;

        //byte[] pngData = _whiteboard.drawingTexture.EncodeToPNG();
        byte[] textureBytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, textureBytes);

        Debug.Log("Drawing saved to: " + filePath);
    }
    
    private int GenerateUniqueId()
    {
        // Generate a unique ID here (e.g., using a timestamp, random number, etc.)
        return System.DateTime.Now.GetHashCode();
    }

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
