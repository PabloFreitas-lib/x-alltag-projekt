using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class Selection_checker : MonoBehaviour
{

    [SerializeField]
    private XRGrabInteractable m_Interactable;
    [SerializeField]
    [Tooltip("The controller holder of core feature controller script.")]
    private GameObject _coreController;

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
    public enum PossibleHand
    {
        LEFT,
        RIGHT,
        BOTH,
        NONE
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = _coreController.GetComponent<core_feature_controller>();
        if (controller == null)
        {
            throw new MissingComponentException("core feature controller not found in controller holder object.");
        }
    }

    // Update is called once per frame
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
