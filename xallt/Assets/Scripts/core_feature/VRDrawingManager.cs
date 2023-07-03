using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using CommonUsages = UnityEngine.XR.CommonUsages;

public class VRDrawingManager : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip;
    public Material drawingMaterial;
    public Material tipMaterial;
    [Range(0.01f, 0.1f)]
    public float penWidth = 0.01f;
    public Color[] penColors;

    [FormerlySerializedAs("RightHandController")] [Header("Hands & Grabbable")]
    public Transform rightHandController;

    private LineRenderer _currentDrawing;
    private int _index;
    private int _currentColorIndex;
    private bool _isDrawing = false;
    private Vector3 _previousPosition;

    private void Start()
    {
        _currentColorIndex = 0;
        tipMaterial.color = penColors[_currentColorIndex];
    }

    private void Update()
    {
        StartDrawing();


        // if (3 finger pinch?)
        // { 
        // Draw();
        // }
        // else if (!3 finger pinch && _isDrawing)
        // {
        //     StopDrawing();
        // }
        //
        // if (_isDrawing)
        // {
        //     Vector3 currentPosition = rightHandController.transform.position;
        //
        //     if (Vector3.Distance(currentPosition, _previousPosition) > penWidth)
        //     {
        //         DrawLine();
        //     }
        //
        //     _previousPosition = currentPosition;
        // }
    }


    private void Draw()
    {
        if (_currentDrawing == null)
        {
            _index = 0;
            _currentDrawing = new GameObject().AddComponent<LineRenderer>();
            _currentDrawing.material = drawingMaterial;
            _currentDrawing.startColor = _currentDrawing.endColor = penColors[_currentColorIndex];
            _currentDrawing.startWidth = _currentDrawing.endWidth = penWidth;
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
    
    private void StartDrawing()
    {
        _isDrawing = true;
        Draw();
    }
    
    private void StopDrawing()
    {
        _isDrawing = false;
    }
    
    private void DrawLine()
    {
        _index = 0;
        _currentDrawing = new GameObject().AddComponent<LineRenderer>();
        _currentDrawing.material = drawingMaterial;
        _currentDrawing.startColor = _currentDrawing.endColor = penColors[_currentColorIndex];
        _currentDrawing.startWidth = _currentDrawing.endWidth = penWidth;
        _currentDrawing.positionCount = 1;
        _currentDrawing.SetPosition(0, tip.position);
    }

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
    
    public void ClearDrawing()
    {
        _currentDrawing.positionCount = 0;
    }
    
}
