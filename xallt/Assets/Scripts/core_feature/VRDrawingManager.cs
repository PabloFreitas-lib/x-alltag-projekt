using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;

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

    private void Start()
    {
        _currentColorIndex = 0;
        tipMaterial.color = penColors[_currentColorIndex];
    }

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

    public void SetDrawingHand(Handedness hand)
    {
        if (hand != Handedness.Invalid)
        {
            _drawingHand = hand;
        }
    }
    
    /**
     * Die Draw()-Methode wird genutzt, um eine 3D-Linie im Raum zu zeichnen.
     */
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

    /*
     * StartDrawing() startet die Zeichnung. Wurde für das Starten des Zeichnens
     * mithilfe eine Buttons benötigt.
     */
    public void StartDrawing()
    {
        _isDrawing = true;
        Draw();
    }

    /**
     * StopDrawing() beendet eine Zeichnung, indem _isDrawing = false gesetzt wird.
     */
    public void StopDrawing()
    {
        _isDrawing = false;
    }

    /**
     * SwitchColor() verändert die Farbe des Stifts, allerdings muss dazu noch ein Mechanismus
     * entworfen werden, um das möglichst intuitiv und benutzerfreundlich zu gestalten.
     * SwitchColor() wird dementsprechend noch an keiner Stelle aufgerufen.
     */
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

    /**
     * ClearDrawing() wird aufgerufen, um die Szene von einer 3D-Zeichnung zu befreien.
     */
    public void ClearDrawing()
    {
        _currentDrawing.positionCount = 0;
        _index = 0;
    }

}
