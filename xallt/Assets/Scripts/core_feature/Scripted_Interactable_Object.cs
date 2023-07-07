using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Hands;
using Vector3 = UnityEngine.Vector3;

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
    protected Handedness controllingHand = Handedness.Invalid;
    
    /// <summary>
    /// Last transform information of activated object
    /// </summary>
    protected Quaternion lastRotationBeforeActivation;
    protected Vector3 lastPositionBeforeActivation;

    /// <summary>
    /// Joint data needed for interaction
    /// </summary>
    protected Dictionary<XRHandJointID, XRHandJoint> necessaryJointData;

    /// <summary>
    /// Joint indices that are necessary for interaction
    /// </summary>
    public readonly List<XRHandJointID> handJointIDs;

    /// <summary>
    /// Used  to update the physic constraints for this object
    /// </summary>
    [SerializeField]
    [Tooltip("The rigidbody of game-object.")]
    private Rigidbody rigidbody;

    /// <summary>
    /// Object that animates the select and detach behavior of the object 
    /// </summary>
    private MoveAnimation moveAnimation;

    /// <summary>
    /// Event that is triggered when the object reached its designated position
    /// </summary>
    public Action OnObjectPositioned;
    
    /// <summary>
    /// Material that is used by the line renderer to display the animation
    /// </summary>
    public Material animationMaterial;

    /// <summary>
    /// Indicates if an object should be moved back to it's last origin
    /// </summary>
    public bool moveBack = false;


    /// <summary>
    /// Constructor that only checks if given list isn't empty
    /// </summary>
    /// <param name="necessaryJoints">List if needed joint indices to get the joint pose information</param>
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
                lastPositionBeforeActivation = transform.position;
                lastRotationBeforeActivation = transform.rotation;
                controllingHand = handedness;
                necessaryJointData = joints;
                //update rigidbody so the object is solely controlled by hand data
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
                moveAnimation = gameObject.AddComponent<MoveAnimation>();
                moveAnimation.drawingMaterial = animationMaterial;
                moveAnimation.startAnimation(this, MoveAnimation.AnimationAction.SELECT);
                moveAnimation.OnAnimationEnd += () => objectSpecificActivation();
                moveAnimation.OnAnimationEnd += OnAnimationEnd;

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

    private void OnAnimationEnd()
    {
        if(moveAnimation != null)
        {
            moveAnimation.OnAnimationEnd -= OnAnimationEnd;
            moveAnimation = null;
        }
        if(OnObjectPositioned != null)
        {
            OnObjectPositioned.Invoke();
        }
    }

    /// <summary>
    /// Updates the data used for interaction
    /// </summary>
    /// <param name="joints">Dictionary of all joint data. Must be matching the joint indices</param>
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
    public void deactivate()
    {
        controllingHand = Handedness.Invalid;
        objectSpecificDeactivation();
        //start animation and update the physic constraints after the end of the animation
        if (moveBack)
        {
            moveBackToOrigin();
        }
        else
        {
            objectSpecificDeactivation();
            UpdatePhysics();
            Destroy(moveAnimation);
        }
        necessaryJointData.Clear();        
    }


    /// <summary>
    /// Moves the game-object back to the position stored in lastTransformBeforeActivation
    /// </summary>
    /// <exception cref="NotImplementedException">Not implemented yet</exception>
    void moveBackToOrigin()
    {
        Collider[] colList = transform.GetComponentsInChildren<Collider>();
        if (colList != null && colList.Count() > 0)
        {
            foreach(Collider col in colList) 
            {
                col.enabled = false;
            }
        }
        moveAnimation =  gameObject.AddComponent<MoveAnimation>();
        moveAnimation.startAnimation(this, MoveAnimation.AnimationAction.DETACH);
        //update the physics and then destroy the MoveAnimation object after the object reached its position
        moveAnimation.OnAnimationEnd+= UpdatePhysics;
        moveAnimation.OnAnimationEnd += () => Destroy(moveAnimation);
    }

    /// <summary>
    /// Updates the physic constraints of rigidbody component
    /// </summary>
    private void UpdatePhysics()
    {
        Collider[] colList = transform.GetComponentsInChildren<Collider>();
        if (colList != null && colList.Count() > 0)
        {
            foreach (Collider col in colList)
            {
                col.enabled = true;
            }
        }
        rigidbody.position = transform.position;
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
    }

    /// <summary>
    /// This method  performs individual stuff if deactivating an object
    /// </summary>
    protected abstract void objectSpecificDeactivation();


    /// <summary>
    /// This method  performs individual stuff if activating an object
    /// </summary>
    protected abstract void objectSpecificActivation();

    /// <summary>
    /// Updates the displayed animation in relation to the current hand joint data 
    /// </summary>
    public abstract void updateInteraction();

    /// <summary>
    /// Returns the final position and rotation the object should currently be displayed at. If the object is currently in hand these values represents
    /// the current holding transform for th object in hand.
    /// </summary>
    /// <param name="animationAction">Defines whether the final transform should be the hand or at the last known transform before selection</param>
    /// <param name="finalPosition">Designated position of object</param>
    /// <param name="finalRotation">Designated rotation of object</param>
    /// <returns></returns>
    public abstract bool getFinalTransform(in MoveAnimation.AnimationAction animationAction, out Vector3 finalPosition, out UnityEngine.Quaternion finalRotation);
}