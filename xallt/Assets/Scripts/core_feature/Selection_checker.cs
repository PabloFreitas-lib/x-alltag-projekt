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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(m_Interactable.isSelected && !m_lastSelected)
        {
            core_feature_controller core_Feature_Controller = GetComponent<core_feature_controller>();
            if(core_Feature_Controller != null)
            {
                if (leftSelected.action.IsPressed())
                {
                    core_Feature_Controller.activateGameObject(gameObject, UnityEngine.XR.Hands.Handedness.Left);
                }
                else if (rightSelected.action.IsPressed())
                {
                    core_Feature_Controller.activateGameObject(gameObject, UnityEngine.XR.Hands.Handedness.Right);
                }
            }
            m_lastSelected = true;
        }
        if(!m_Interactable.isSelected && m_lastSelected)
        {
            m_lastSelected = false;
        }
    }
}
