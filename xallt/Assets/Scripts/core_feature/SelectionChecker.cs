using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
/// This class is used to check if an object is selected and if so, it will be activated by the core feature controller.
/// </summary>
/// <author> Fabian Schmurr </author>
[RequireComponent(typeof(XRGrabInteractable))]
public class SelectionChecker : MonoBehaviour
{
    [SerializeField]
    private XRGrabInteractable m_Interactable;
    [SerializeField]

    [Tooltip("The controller holder of core feature controller script.")]
    private GameObject _coreController;

    /// <summary>
    /// Selection gesture for objects
    /// </summary>
    [SerializeField]
    [Tooltip("Left hand selection input")]
    InputActionReference leftSelected;

    [SerializeField]
    [Tooltip("Right hand selection input")]
    InputActionReference rightSelected;

    [SerializeField]
    [Tooltip("Gesture that is used to select an object")]
    private Gesture.GestureType selectGesture;

    [SerializeField]
    [Tooltip("Describes how the object an be selected")]
    private SelectionType selectionType;

    [SerializeField]
    [Tooltip("Defines in which hand this object can be hold.")]
    private PossibleHand handToHold = PossibleHand.BOTH;

    /// <summary>
    /// Time a gesture needs to be held until the object gets selected
    /// </summary>
    private float selectionGestureTime = 1f;

    private float currentSelectionTime = 0f;
    private bool m_lastSelected = false;

    private Gesture lastSelectionGesture;

    private CoreFeatureController controller;

    [SerializeField]
    [Tooltip("Only necessary if a gesture is used to activate this object.")]
    private GestureRecognizer _gestureRecognizer;

    /// <summary>
    /// Defines in which hand this object can be hold.
    /// </summary>
    /// <author> Fabian Schmurr </author>
    public enum PossibleHand
    {
        LEFT,
        RIGHT,
        BOTH,
        NONE
    }

    /// <summary>
    /// Enum that indicates how the object can be selected
    /// </summary>
    /// <author>Fabian Schmurr</author>
    public enum SelectionType
    {
        GESTURE,
        RAYCAST,
        BOTH
    }

    /// <summary>
    /// This function is called when the object is activated by the core feature controller.
    /// </summary>
    /// <exception cref="MissingComponentException"> if <see cref="core_feature_controller"/> couldn't be found </exception>
    /// <author> Fabian Schmurr </author>
    void Start()
    {
        controller = _coreController.GetComponent<CoreFeatureController>();
        if (controller == null)
        {
            throw new MissingComponentException("core feature controller not found in controller holder object.");
        }

        if ((selectionType == SelectionType.BOTH) || (selectionType == SelectionType.GESTURE))
        {
            _gestureRecognizer.OnGestureDetected.AddListener(OnGestureDetected);
            _gestureRecognizer.OnGestureReleased.AddListener(OnGestureReleased);
        }
    }

    private void OnGestureReleased(Gesture arg0)
    {
        //stop timer
        currentSelectionTime = 0;
        lastSelectionGesture = null;
    }

    private void OnGestureDetected(Gesture arg0)
    {
        //start selection timer if gesture was performed
        if (arg0.type == selectGesture &&
            (arg0.handedness == Handedness.Left && handToHold == PossibleHand.LEFT
             || arg0.handedness == Handedness.Right && handToHold == PossibleHand.RIGHT
             || (arg0.handedness == Handedness.Right || arg0.handedness == Handedness.Left)
             && handToHold == PossibleHand.BOTH))
        {
            lastSelectionGesture = arg0;
            currentSelectionTime += Time.deltaTime;
        }
    }


    /// <summary>
    /// This function is called every frame and checks if the object is selected and if so, it will be activated by the core feature controller.
    /// </summary>
    /// <author> Fabian Schmurr </author>
    void Update()
    {
        if (handToHold == PossibleHand.NONE)
        { return; }
        //in case that raycast-mode is enabled
        if (selectionType == SelectionType.RAYCAST)
        {
            checkForRaycastSelection();
        }
        else if (selectionType == SelectionType.BOTH)
        {
            checkForRaycastSelection();
            checkForGestureSelection();
        }
        else if (selectionType == SelectionType.GESTURE)
        {
            checkForGestureSelection();
        }
    }

    private void checkForGestureSelection()
    {
        Debug.Log("current pen timer:\t" + currentSelectionTime);
        if (!m_lastSelected && currentSelectionTime > 0)
        {
            currentSelectionTime += Time.deltaTime;
            if (currentSelectionTime > selectionGestureTime)
            {
                controller.activateGameObject(gameObject, lastSelectionGesture.handedness);
                m_lastSelected = true;
                currentSelectionTime = 0f;
            }
        }
        else if (m_lastSelected)
        {
            m_lastSelected = false;
        }
    }

    private void checkForRaycastSelection()
    {
        if (m_Interactable.isSelected && !m_lastSelected)
        {
            if (handToHold == PossibleHand.LEFT || handToHold == PossibleHand.BOTH)
            {
                if (leftSelected.action.WasPressedThisFrame())
                {
                    if (controller.activateGameObject(gameObject, UnityEngine.XR.Hands.Handedness.Left))
                    {
                        m_lastSelected = true;
                    }
                }
            }
            if (handToHold == PossibleHand.RIGHT || handToHold == PossibleHand.BOTH)
            {
                if (rightSelected.action.WasPressedThisFrame())
                {
                    if (controller.activateGameObject(gameObject, UnityEngine.XR.Hands.Handedness.Right))
                    {
                        m_lastSelected = true;
                    }
                }

            }
        }
        else if (!m_Interactable.isSelected && m_lastSelected)
        {
            m_lastSelected = false;
        }
    }
}