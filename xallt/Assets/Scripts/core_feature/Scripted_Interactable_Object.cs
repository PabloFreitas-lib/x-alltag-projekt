using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

/// <summary>
/// Parent class of all objects performing an scripted interaction based of hand-tracking data based in plugin XRHand
/// Author: Fabian Schmurr
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Scripted_Interactable_Object : MonoBehaviour
{

    /// <summary>
    /// Indicates the current hand controlling the interactable object
    /// </summary>
    private Handedness controllingHand = Handedness.Invalid;
    
    /// <summary>
    /// Last transform information of activated object
    /// </summary>
    private Transform lastTransformBeforeActivation;

    /// <summary>
    /// Joint data needed for interaction
    /// </summary>
    protected Dictionary<XRHandJointID, XRHandJoint> necessaryJointData;

    /// <summary>
    /// Joint indices that are necessary for interaction
    /// </summary>
    public readonly List<XRHandJointID> handJointIDs;

    [SerializeField]
    private Rigidbody rigidbody;

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="ArgumentException">If necessary hand-joint ID's are not set in Editor</exception>
    public Scripted_Interactable_Object(List<XRHandJointID> necessaryJoints)
    {
        if(necessaryJoints == null || necessaryJoints.Count == 0)
        {
            throw new ArgumentException("List of necessary joints mustn't be empty or null");
        }
        handJointIDs = necessaryJoints;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

/// <summary>
/// Method that tries to activate an given object
/// </summary>
/// <param name="joints">Necessary hand-tracking data</param>
/// <param name="handedness">Indicator in which hand the object should be held in</param>
/// <exception cref="ArgumentException">If joints dictionary length is not matching initial joint index list od
/// given handedness has value Invalid.</exception>
    public void Activate(Dictionary<XRHandJointID, XRHandJoint> joints, Handedness handedness)
    {
        if(handedness != Handedness.Invalid)
        {
        
            //check if length of given dictionary matches the length of pre-defined joint id list
            if (joints.Count == handJointIDs.Count)
            {
                lastTransformBeforeActivation = transform;
                controllingHand = handedness;
                necessaryJointData = joints;

                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            } 
            else
            {
                throw new ArgumentException("Length of join-dictionary must match the initial list of necessary joint indices");
            }
        }
        else
        {
            throw new ArgumentException("Handedness can't be Invalid.");
        }
    }

    public void updateData(Dictionary<XRHandJointID, XRHandJoint> joints)
    {
        if(joints.Count == handJointIDs.Count)
        {
            necessaryJointData = joints;
        }
    }

    /// <summary>
    /// Method to deactivate the interaction of an object
    /// </summary>
    /// <param name="moveBack">If true the deactivated object will be set back to it's origin before interaction</param>
    public void deactivate(bool moveBack = false)
    {
        controllingHand = Handedness.Invalid;
        if (moveBack)
        {
            moveBackToOrigin();
        }
        necessaryJointData.Clear();
        rigidbody.position = transform.position;
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        
    }

    /// <summary>
    /// Moves the game-object back to the position stored in lastTransformBeforeActivation
    /// </summary>
    /// <exception cref="NotImplementedException">Not implemented yet</exception>
    void moveBackToOrigin()
    {
        throw new NotImplementedException();
    }
    public abstract void updateInteraction();

}
