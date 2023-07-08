using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Samples.VisualizerSample;
using UnityEngine.XR.Management;

/// <summary>
/// Script that works as a controller between objects controlled directly by
/// hand-tracking and their selection
/// Main Author: Fabian Schmurr
/// Additional authors: Jaap
/// </summary>
public class core_feature_controller : MonoBehaviour
{


    /// <summary>
    /// The current object that is being hold by the player in left hand
    /// </summary>
    private Scripted_Interactable_Object m_leftHandObj = null;

    /// <summary>
    /// The current object that is being hold by the player in right hand
    /// </summary>
    private Scripted_Interactable_Object m_rightHandObj = null;

    private bool m_leftObjInInteractionPosition = false;
    private bool m_rightObjInInteractionPos = false;

    /// <summary>
    /// Reference to left hand data
    /// </summary>
    private XRHand m_leftHandRef;

    /// <summary>
    /// Reference to right hand data
    /// </summary>
    private XRHand m_rightHandRef;

    [SerializeField]
    [Tooltip("Name of XR main object")]
    private string m_XRObjectName;

    [SerializeField]
    [Tooltip("Pinch gesture to detach the left object")]
    private InputActionReference m_detachPinchLeft;

    [SerializeField]
    [Tooltip("Pinch gesture to detach the right object")]
    private InputActionReference m_detachPinchRight;

    /// <summary>
    /// Script to control which hands should currently be displayed or not
    /// </summary>
    private SeperateHandVisualizer m_handVisualizerScipt;


    // Start is called before the first frame update
    void Start()
    {
        XRHandSubsystem m_Subsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRHandSubsystem>();

        //check if loaded system exists 
        if (m_Subsystem != null)
            m_Subsystem.updatedHands += OnHandUpdate;


        // get reference to HandVisualizer Script
        if (m_XRObjectName != null)
        {
            GameObject xr = GameObject.Find(m_XRObjectName);
            if (xr != null)
            {
                m_handVisualizerScipt = xr.GetComponentInChildren<SeperateHandVisualizer>();
                if (m_handVisualizerScipt == null)
                {
                    throw new MissingComponentException("XR-object does not contain SeperateHandvisualizer script.");
                }
            }
            else
            {
                throw new MissingComponentException("XR Object of given name not found.");
            }
        }
    }

    /// <summary>
    /// Method that performs actions every time an hand-update occurred
    /// </summary>
    void OnHandUpdate(XRHandSubsystem subsystem,
                      XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags,
                      XRHandSubsystem.UpdateType updateType)
    {
        switch (updateType)
        {
            case XRHandSubsystem.UpdateType.Dynamic:
                //update right hand reference if it#s correctly updated
                if (updateSuccessFlags.HasFlag(XRHandSubsystem.UpdateSuccessFlags.All))
                {
                    m_leftHandRef = subsystem.leftHand;
                    m_rightHandRef = subsystem.rightHand;
                }
                else if (updateSuccessFlags.HasFlag(XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose | XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints))
                {
                    m_leftHandRef = subsystem.leftHand;
                }

                //update right hand reference if it#s correctly updated
                else if (updateSuccessFlags.HasFlag(XRHandSubsystem.UpdateSuccessFlags.RightHandJoints | XRHandSubsystem.UpdateSuccessFlags.RightHandRootPose))
                {
                    m_rightHandRef = subsystem.rightHand;
                }
                break;

            case XRHandSubsystem.UpdateType.BeforeRender:
                // Update visual objects that use hand data
                if (updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.All)
                {
                    //I know pretty duplicated code, but I couldn't get it to work in a different way
                    if (m_leftHandObj != null)
                    {
                        updateLeft();
                    }
                    if (m_rightHandObj != null)
                    {
                        updateRigth();
                    }
                }
                else if (updateSuccessFlags.HasFlag(XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose | XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints))
                {
                    if (m_leftHandObj != null)
                    {
                        updateLeft();
                    }
                }
                else if (updateSuccessFlags.HasFlag(XRHandSubsystem.UpdateSuccessFlags.RightHandJoints | XRHandSubsystem.UpdateSuccessFlags.RightHandRootPose))
                {
                    if (m_rightHandObj != null)
                    {
                        updateRigth();
                    }
                }
                break;
        }
    }

private void updateLeft()
    {
        //no need for checking if != null because this object has already been proofed
        m_leftHandObj.updateData(getHandDataDictionary(Handedness.Left, m_leftHandObj.handJointIDs));
        if (m_leftObjInInteractionPosition)
        {
            m_leftHandObj.updateInteraction();
        }
    }

    private void updateRigth()
    {
        //no need for checking if != null because this object has already been proofed
        m_rightHandObj.updateData(getHandDataDictionary(Handedness.Right, m_rightHandObj.handJointIDs));
        if (m_rightObjInInteractionPos)
        {
            m_rightHandObj.updateInteraction();
        }
    }

    /// <summary>
    /// Method to activates an intractable GameObject if selecting hand is free
    /// </summary>
    /// <param name="obj">Reference to object that should be activated
    /// </param>
    /// <param name="selectingHand">The Hand that selected the object</param>
    /// <returns>True if activation was successful</returns>
    public bool activateGameObject(GameObject obj, Handedness selectingHand)
    {
        if (obj == null)
        {
            throw new NullReferenceException("Given selected object is null.");
        }

        //object needs to have a configured interaction scripted extending Scripted_Interactable_Object
        Scripted_Interactable_Object scripted_Interactable_Object = obj.GetComponent<Scripted_Interactable_Object>();
        if (obj.GetComponent<Scripted_Interactable_Object>() == null)
        {
            throw new MissingComponentException("selected object does not contain scripted interactable object");
        }

        //can't snap an object to an invalid hand
        if (selectingHand == Handedness.Invalid)
        {
            Debug.Log("invalid hand");
            return false;
        }

        //left hand has selected and is free of any objects
        if (selectingHand == Handedness.Left && m_leftHandObj == null && obj != m_rightHandObj)
        {
            try
            {
                scripted_Interactable_Object.Activate(getHandDataDictionary(Handedness.Left, scripted_Interactable_Object.handJointIDs), Handedness.Left);
                makeHandInvisible(Handedness.Left);
                m_leftHandObj = scripted_Interactable_Object;
                //adding function to 
                scripted_Interactable_Object.OnObjectPositioned += OnLeftInInteractionPosition;
                return true;
            }
            catch (ArgumentException exception)
            {
                Debug.Log(exception);
                return false;
            }
        }
        //right hand has selected and is free of any objects
        if (selectingHand == Handedness.Right && m_rightHandObj == null && obj != m_leftHandObj)
        {
            try
            {
                scripted_Interactable_Object.Activate(getHandDataDictionary(Handedness.Right, scripted_Interactable_Object.handJointIDs), Handedness.Right);
                makeHandInvisible(Handedness.Right);
                m_rightHandObj = scripted_Interactable_Object;
                scripted_Interactable_Object.OnObjectPositioned += OnRightInInteractionPosition;
                return true;
            }
            catch (ArgumentException exception)
            {
                Debug.Log(exception);
                return false;
            }
        }
        return false;
    }

    private void OnRightInInteractionPosition()
    {
        m_rightObjInInteractionPos = true;
        m_rightHandObj.OnObjectPositioned -= OnRightInInteractionPosition;
    }

    private void OnLeftInInteractionPosition()
    {
        m_leftObjInInteractionPosition = true;
        m_leftHandObj.OnObjectPositioned -= OnLeftInInteractionPosition;
    }


    /// <summary>
    /// Method that disables the interaction of object currently being held in given hand
    /// </summary>
    /// <param name="hand">Either Left or Right, Invalid will cause a false return</param>
    /// <returns>If disabling interaction was successful</returns>
    public bool disableGameObject(Handedness hand)
    {
        // can't deactivate an invalid hand lol
        if (hand == Handedness.Invalid)
        {
            return false;
        }

        // object in left hand should be deactivated and is existing
        if (hand == Handedness.Left && m_leftHandObj != null)
        {
            m_leftHandObj.deactivate();
            m_leftHandObj = null;
            m_leftObjInInteractionPosition = false;
            makeHandVisible(Handedness.Left);
            return true;
        }

        //// object in right hand should be deactivated and is existing
        if (hand == Handedness.Right && m_rightHandObj != null)
        {
            m_rightHandObj.deactivate();
            m_rightHandObj = null;
            m_rightObjInInteractionPos = false;
            makeHandVisible(Handedness.Right);
            return true;
        }

        return false;
    }

    /// <summary>
    /// This method deactivates the rendering of chosen hand
    /// </summary>
    /// <param name="hand">whether left or right</param>
    /// <exception cref="NotImplementedException"></exception>
    private void makeHandVisible(Handedness hand)
    {
        if (hand == Handedness.Left)
        {
            m_handVisualizerScipt.drawLeftMeshes = true;
        }
        if (hand == Handedness.Right)
        {
            m_handVisualizerScipt.drawRightMeshes = true;
        }
    }

    /// <summary>
    /// This method activates the rendering of chosen hand
    /// </summary>
    /// <param name="hand">whether left or right</param>
    /// <exception cref="NotImplementedException"></exception>
    private void makeHandInvisible(Handedness hand)
    {
        if (hand == Handedness.Left)
        {
            m_handVisualizerScipt.drawLeftMeshes = false;
        }
        if (hand == Handedness.Right)
        {
            m_handVisualizerScipt.drawRightMeshes = false;
        }
    }

    /// <summary>
    /// Gets all needed joint data in form of a dictionary
    /// </summary>
    /// <param name="handedness">Whether left or right</param>
    /// <param name="jointIds">ID's of necessary hand joints</param>
    /// <returns>Dictionary containing all hand joints of given indices.</returns>
    private Dictionary<XRHandJointID, XRHandJoint> getHandDataDictionary(Handedness handedness, List<XRHandJointID> jointIds)
    {
        if (handedness != Handedness.Invalid && jointIds != null)
        {
            Dictionary<XRHandJointID, XRHandJoint> joints = new Dictionary<XRHandJointID, XRHandJoint>();
            //Add all needed joints of left hand to dictionary
            if (handedness == Handedness.Left)
            {
                foreach (var jointId in jointIds)
                {
                    joints.Add(jointId, m_leftHandRef.GetJoint(jointId));
                }
            }
            //same for the right hand
            else
            {
                foreach (var jointId in jointIds)
                {
                    joints.Add(jointId, m_rightHandRef.GetJoint(jointId));
                }
            }
            return joints;
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        //check for detach gesture
        if (m_detachPinchLeft.action.inProgress)
        {
            disableGameObject(Handedness.Left);
        }
        if (m_detachPinchRight.action.inProgress)
        {
            disableGameObject(Handedness.Right);
        }
    }
}
