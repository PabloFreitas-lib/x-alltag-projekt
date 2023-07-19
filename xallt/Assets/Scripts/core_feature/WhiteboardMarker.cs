
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Serialization;


/// <summary>
///    describes behaviour of whiteboard marker
/// </summary>
/// <author> Sophia Gommeringer & Celina Dadschun (& Noah Horn & Jakob Kern) </author>
public class WhiteboardMarker : MonoBehaviour
{
    
    public List<string> paths = new List<string>();

    // pen properties
    [FormerlySerializedAs("_tip")][SerializeField] private Transform tip;
    [FormerlySerializedAs("_penSize")][SerializeField] private int penSize = 5;
    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;

    // drawing properties
    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;
    private int _drawingId;


    /// <summary>
    ///    initialize variales 
    /// </summary>
    /// <author> Sophia Gommeringer </author>
    void Start()
    {
        _renderer = tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
        _tipHeight = tip.localScale.y;
    }

    /// <summary>
    ///    adds drawing id to paths
    /// </summary>
    /// <author> Noah Horn & Jakob Kern </author>
    /// <param name="id"> identification of drawing </param>
    public void addPath(string id)
    {
        paths.Add(id);
    }

    /// <summary>
    ///    calls Draw() method every frame
    /// </summary>
    /// <author> Sophia Gommeringer </author>
    void Update()
    {
        Draw();
    }
 
    private void Draw()
    {
        // generate a new unique drawing ID
        _drawingId = GenerateUniqueId();
        // checks if marker tip collides with whiteboard
        if (Physics.Raycast(tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                // if whiteboard reference is null, get it from the collided object
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                // get position of where marker collides with whiteboard
                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                // calculate pixel coordinates on whiteboard texture
                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (penSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (penSize / 2));

                // check if coordinates are within whiteboard
                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x) return;

                // if _touchedLastFrame is true, draw
                if (_touchedLastFrame)
                {
                    _whiteboard.drawingTexture.SetPixels(x, y, penSize, penSize, _colors);

                    // linear interpolation between points
                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.drawingTexture.SetPixels(lerpX, lerpY, penSize, penSize, _colors);
                    }

                    // freeze marker rotation when it collides with whiteboard
                    transform.rotation = _lastTouchRot;

                    _whiteboard.drawingTexture.Apply();
                }

                // store current touch position and rotation for next frame
                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }

        // if pen does not touch whiteboard set variable to false
        _touchedLastFrame = false;
    }

    /// <summary>
    ///    initiate saving of whiteboard, by calling SaveTexture()
    /// </summary>
    /// <author> Celina Dadschun </author>
    public void SaveDrawing()
    {
        Debug.Log("Drawing saved");
        // save drawing, eg. with SaveTexture-method
        SaveTexture(_whiteboard.drawingTexture);
    }

    // saves texture in .png-format on path
    private void SaveTexture(Texture2D texture)
    {
        string fileName = "Drawing_" + _drawingId + ".png";
        string filePath = Application.persistentDataPath + "/" + fileName;

        //byte[] pngData = _whiteboard.drawingTexture.EncodeToPNG();
        byte[] textureBytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, textureBytes);

        Debug.Log("Drawing saved to: " + filePath);
    }

    
    // generates unique id
    private int GenerateUniqueId()
    {
        // generate a unique ID here (e.g., using a timestamp, random number, etc.)
        return System.DateTime.Now.GetHashCode();
    }

    /// <summary>
    ///    clears drawing on whiteboard 
    /// </summary>
    /// <author> Celina Dadschun </author>
    public void ClearDrawing()
    {
        //_whiteboard = null;
        Color[] emptyColors = new Color[_whiteboard.drawingTexture.width * _whiteboard.drawingTexture.height];
        for (int i = 0; i < emptyColors.Length; i++)
        {
            // set color to white/transparent
            emptyColors[i] = Color.white; 
        }

        // overwrite whole drawing auf on whiteboard with empty color
        _whiteboard.drawingTexture.SetPixels(emptyColors);
        _whiteboard.drawingTexture.Apply();
    }
}
