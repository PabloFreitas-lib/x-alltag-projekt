using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
/// This class is used to check if an object is selected and if so, it will be activated by the core feature controller.
/// </summary>
/// <author> Fabian Schmurr </author>
[RequireComponent(typeof(XRGrabInteractable))]
public class Selection_checker : MonoBehaviour
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
    [Tooltip("Defines in which hand this object can be hold.")]
    private PossibleHand handToHold = PossibleHand.BOTH;

    private bool m_lastSelected = false;

    private core_feature_controller controller;

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
    /// This function is called when the object is activated by the core feature controller.
    /// </summary>
    /// <exception cref="MissingComponentException"> if <see cref="core_feature_controller"/> couldn't be found </exception>
    /// <author> Fabian Schmurr </author>
    void Start()
    {
        controller = _coreController.GetComponent<core_feature_controller>();
        if (controller == null)
        {
            throw new MissingComponentException("core feature controller not found in controller holder object.");
        }
    }


    /// <summary>
    /// This function is called every frame and checks if the object is selected and if so, it will be activated by the core feature controller.
    /// </summary>
    /// <author> Fabian Schmurr </author>
    void Update()
    {
        if(handToHold == PossibleHand.NONE)
        { return; }
        if (m_Interactable.isSelected && !m_lastSelected)
        {
            if (controller != null)
            {
                if(handToHold == PossibleHand.LEFT || handToHold == PossibleHand.BOTH)
                {
                    if (leftSelected.action.WasPressedThisFrame())
                    {
                        if (controller.activateGameObject(gameObject, UnityEngine.XR.Hands.Handedness.Left))
                        {
                            m_lastSelected = true;
                        }
                    }
                }
                if(handToHold == PossibleHand.RIGHT || handToHold == PossibleHand.BOTH)
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
        }
        else if (!m_Interactable.isSelected && m_lastSelected)
        {
            m_lastSelected = false;
        }
    }
}
