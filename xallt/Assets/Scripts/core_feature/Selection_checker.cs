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

    private bool m_lastSelected = false;

    private core_feature_controller controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = _coreController.GetComponent<core_feature_controller>();
        if(controller == null)
        {
            throw new MissingComponentException("core feature controller not found in controller holder object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(m_Interactable.isSelected && !m_lastSelected)
        {
            if(controller != null)
            {
                Debug.Log("object selection detected");
                if (leftSelected.action.WasPerformedThisFrame())
                {
                    Debug.Log("left selection detected");
                    controller.activateGameObject(gameObject, UnityEngine.XR.Hands.Handedness.Left);
                    m_lastSelected = true;
                }
                else if (rightSelected.action.WasPerformedThisFrame())
                {
                    Debug.Log("right selection detected");
                    controller.activateGameObject(gameObject, UnityEngine.XR.Hands.Handedness.Right);
                    m_lastSelected = true;
                }
            }
        }
        if(!m_Interactable.isSelected && m_lastSelected)
        {
            m_lastSelected = false;
        }
    }
}
