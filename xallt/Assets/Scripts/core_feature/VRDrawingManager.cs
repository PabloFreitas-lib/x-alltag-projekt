using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// This class is used to describe the behavior of the VR drawing manager.
/// </summary>
/// <author> Celina Dadschun, Fabian Schmurr </author>
public class VRDrawingManager : MonoBehaviour
{
    public uint id;
    [Header("Pen Properties")]
    public Transform tip;
    public Material drawingMaterial;
    public Material tipMaterial;
    [Range(0.01f, 0.1f)]
    public float penWidth = 0.01f;
    public Color[] penColors;

    [Header("Drawing Properties")]
    public LineRenderer _currentDrawing;
    private int _index;
    private int _currentColorIndex;
    private bool _isDrawing;
    private Vector3 _previousPosition;

    [SerializeField]
    private MeshBuffer meshBuffer;

    /// <summary>
    /// This function is called when an object is initialized.
    /// </summary>
    /// <author> Celina Dadschun </author>
    private void Start()
    {
        _currentColorIndex = 0;
        tipMaterial.color = penColors[_currentColorIndex];
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    /// <author> Fabian Schmurr </author>
    private void Update()
    {
        if(_isDrawing)
        {
            Draw();
        }
    }

    /// <summary>
    /// This method is used to draw a line.
    /// </summary>
    /// <author> Celina Dadschun </author>
    private void Draw()
    {
        if (_currentDrawing == null)
        {
            _index = 0;
            _currentDrawing = gameObject.AddComponent<LineRenderer>();
            _currentDrawing.material = drawingMaterial;
            _currentDrawing.startColor = _currentDrawing.endColor = penColors[_currentColorIndex];
            _currentDrawing.startWidth = _currentDrawing.endWidth = penWidth;
            _currentDrawing.positionCount = 1;
            _currentDrawing.SetPosition(0, tip.position);
        }
        else if(_index == 0 && _currentDrawing.positionCount == 0)
        {
            _currentDrawing.positionCount = 1;
            _currentDrawing.SetPosition(0, tip.position);
        }
        else
        {
            var currentPos = _currentDrawing.GetPosition(_index);
            if (Vector3.Distance(currentPos, tip.position) > 0.01f)
            {
                _index++;
                _currentDrawing.positionCount = _index + 1;
                _currentDrawing.SetPosition(_index, tip.position);
            }
        }
    }

    /// <summary>
    /// This function starts the drawing by setting _isDrawing to true.
    /// </summary>
    /// <author> Celina Dadschun </author>
    public void StartDrawing()
    {
        _isDrawing = true;
        Draw();
    }


    /// <summary>
    /// This function end the drawing by setting _isDrawing to false.
    /// </summary>
    /// <author> Celina Dadschun </author>
    public void StopDrawing()
    {
        _isDrawing = false;
    }

    /// <summary>
    /// This function switches the color of the pen.
    /// </summary>
    /// <author> Celina Dadschun </author>
    private void SwitchColor()
    {
        if (_currentColorIndex == penColors.Length - 1)
        {
            _currentColorIndex = 0;
        }
        else
        {
            _currentColorIndex++;
        }
        tipMaterial.color = penColors[_currentColorIndex];
    }

    private bool bakeCurrentDrawing()
    {
        if(_index < 1)
        {
            return false;
        }
        if(_currentDrawing == null)
        {
            return false;
        }

        //get mesh from current drawing relative to line renderer
        Mesh drawing = new Mesh();
        _currentDrawing.BakeMesh(drawing, true);
        meshBuffer.addBakedDrawing(drawing);
        return true;
    }
    
    /// <summary>
    /// This function clears the drawing.
    /// </summary>
    /// <author> Celina Dadschun </author>
    public void ClearDrawing()
    {
        _currentDrawing.positionCount = 0;
        _index = 0;
        if(_currentDrawing != null)
        {
            Destroy(_currentDrawing);
        }
    }

    public void setIsDrawing(bool actionInProgress)
    {
        _isDrawing = actionInProgress;
        if (!_isDrawing && bakeCurrentDrawing())
        {
            _currentDrawing.positionCount = 1;
            _index = 0;
        }
    }
}
