using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

/// <summary>
/// Script that works as a controller between objects controlled directly by
/// hand-tracking and their selection
/// Author: Fabian Schmurr
/// </summary>
public class core_feature_controller : MonoBehaviour
{


    /// <summary>
    /// The current object that is being hold by the player in left hand
    /// </summary>
    private GameObject m_leftHandObj;

    /// <summary>
    /// The current object that is being hold by the player in right hand
    /// </summary>
    private GameObject m_rightHandObj;

    /// <summary>
    /// Reference to left hand data
    /// </summary>
    private XRHand m_leftHandRef;

    /// <summary>
    /// Reference to right hand data
    /// </summary>
    private XRHand m_rightHandRef;


    // Start is called before the first frame update
    void Start()
    {
        XRHandSubsystem m_Subsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRHandSubsystem>();

        //check if loaded system exists 
        if (m_Subsystem != null)
            m_Subsystem.updatedHands += OnHandUpdate;
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
            //check if relocate action has already started
            case XRHandSubsystem.UpdateType.Dynamic:
                //update right hand reference if it#s correctly updated
                if (updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints || updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.All)
                {
                    m_leftHandRef = subsystem.leftHand;
                }

                //update right hand reference if it#s correctly updated
                if (updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.RightHandJoints || updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.All)
                {
                    m_rightHandRef = subsystem.rightHand;
                }
                break;

            case XRHandSubsystem.UpdateType.BeforeRender:
                // Update visual objects that use hand data
                if (updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints || updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.All)
                {
                    if (m_leftHandObj != null)
                    {
                        //no need for checking if != null because this object has already been proofed
                        m_leftHandObj.GetComponent<Scripted_Interactable_Object>().updateInteraction();
                    }
                }
                if (updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.RightHandJoints || updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.All)
                {
                    if (m_rightHandObj != null)
                    {
                        //no need for checking if != null because this object has already been proofed
                        m_rightHandObj.GetComponent<Scripted_Interactable_Object>().updateInteraction();
                    }
                }
                break;
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
            return false;
        }

        //object needs to have a configured interaction scripted extending Scripted_Interactable_Object
        Scripted_Interactable_Object scripted_Interactable_Object = obj.GetComponent<Scripted_Interactable_Object>();
        if (obj.GetComponent<Scripted_Interactable_Object>() == null)
        {
            return false;
        }

        //can't snap an object to an invalid hand
        if (selectingHand != Handedness.Invalid)
        {
            return false;
        }
        //left hand has selected and is free of any objects
        if (selectingHand == Handedness.Left && m_leftHandObj == null)
        {
            try
            {
                scripted_Interactable_Object.Activate(getHandDataDictionary(Handedness.Left, scripted_Interactable_Object.handJointIDs), selectingHand);
                makeHandInvisible(Handedness.Left);
                return true;
            }
            catch (ArgumentException exception) 
            {
                Debug.Log(exception);
                return false; 
            }            
        }
        //right hand has selected and is free of any objects
        if (selectingHand == Handedness.Right && m_rightHandObj == null)
        {
            try
            {
                scripted_Interactable_Object.Activate(getHandDataDictionary(Handedness.Right, scripted_Interactable_Object.handJointIDs), selectingHand);
                makeHandInvisible(Handedness.Right);
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


    /// <summary>
    /// Method that disables the interaction of object currently being held in given hand
    /// </summary>
    /// <param name="hand">Either Left or Right, Invalid will cause a false return</param>
    /// <returns>If disabling interaction was successful</returns>
    public bool disableGameObject(Handedness hand)
    {
        // can't deactivate a invalid hand lol
        if (hand == Handedness.Invalid)
        {
            return false;
        }

        // object in left hand should be deactivated and is existing
        if (hand == Handedness.Left && m_leftHandObj != null)
        {
            m_leftHandObj.GetComponent<Scripted_Interactable_Object>().deactivate();
            m_leftHandObj = null;
            makeHandVisible(Handedness.Left);
            return true;
        }

        //// object in right hand should be deactivated and is existing
        if (hand == Handedness.Right && m_rightHandObj != null)
        {
            m_leftHandObj.GetComponent<Scripted_Interactable_Object>().deactivate();
            m_rightHandObj = null;  
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
        throw new NotImplementedException();
    }

    /// <summary>
    /// This method activates the rendering of chosen hand
    /// </summary>
    /// <param name="hand">whether left or right</param>
    /// <exception cref="NotImplementedException"></exception>
    private void makeHandInvisible(Handedness hand)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets all needed joint data in form of a dictionary
    /// </summary>
    /// <param name="handedness">Whether left or right</param>
    /// <param name="jointIds">ID's of necessary hand joints</param>
    /// <returns>Dictionary containing all hand joints of given indices.</returns>
    private Dictionary<XRHandJointID, XRHandJoint> getHandDataDictionary(Handedness handedness, List<XRHandJointID> jointIds)
    {
        if(handedness != Handedness.Invalid && jointIds != null)
        {
            Dictionary<XRHandJointID, XRHandJoint> joints = new Dictionary<XRHandJointID, XRHandJoint> ();
            //Add all needed joints of left hand to dictionary
            if(handedness == Handedness.Left)
            {
                foreach(var jointId in jointIds)
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

    }
}
