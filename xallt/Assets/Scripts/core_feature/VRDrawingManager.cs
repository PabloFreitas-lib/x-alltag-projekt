using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
/// This class is used to describe the behavior of the VR drawing manager.
/// </summary>
/// <author> Authors </author>
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

    [Header("Hands & Interactable")]
    [SerializeField]
    private XRGrabInteractable interactable;
    [SerializeField]
    private InputActionReference leftSelect;
    [SerializeField]
    private InputActionReference rightSelect;

    private Handedness _drawingHand = Handedness.Invalid;

    /// <summary>
    /// This function is called when an object is initialized.
    /// </summary>
    /// <author> Authors </author>
    private void Start()
    {
        _currentColorIndex = 0;
        tipMaterial.color = penColors[_currentColorIndex];
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    /// <author> Authors </author>
    private void Update()
    {
        if (_drawingHand == Handedness.Left)
        {
            _isDrawing = leftSelect.action.inProgress; 
        }
        else if (_drawingHand == Handedness.Right)
        {
            _isDrawing = rightSelect.action.inProgress;
        }
        else if (_drawingHand == Handedness.Invalid)
        {
            _isDrawing = false;
        }
        if(_isDrawing)
        {
            Draw();
        }
        
        /*if (_interactable.isSelected)
        {
            StartDrawing();
        }
        else
        {
            StopDrawing();
        }*/
    }

    /// <summary>
    /// This function is called when the object is activated by the core feature controller.
    /// </summary>
    /// <author> Authors </author>
    /// <param name="hand">Handedness of the hand that activated the object</param>
    public void SetDrawingHand(Handedness hand)
    {
        if (hand != Handedness.Invalid)
        {
            _drawingHand = hand;
        }
    }

     /// <summary>
    /// Die Draw()-Methode wird genutzt, um eine 3D-Linie im Raum zu zeichnen.
    /// </summary>
    /// <author> Authors </author>
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
    /// <author> Authors </author>
    public void StartDrawing()
    {
        _isDrawing = true;
        Draw();
    }


    /// <summary>
    /// This function end the drawin by setting _isDrawing to false.
    /// </summary>
    public void StopDrawing()
    {
        _isDrawing = false;
    }

    /// <summary>
    /// This function switches the color of the pen.
    /// </summary>
    /// <author> Authors </author>
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
    
    /// <summary>
    /// This function clears the drawing.
    /// </summary>
    /// <author> Authors </author>
    public void ClearDrawing()
    {
        _currentDrawing.positionCount = 0;
        _index = 0;
    }

}
