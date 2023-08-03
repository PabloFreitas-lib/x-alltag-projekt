using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
/// <summary>
/// Scripted scissor-interaction based on earlier script cutting.cs by Laura Gietschel and Fabian Schmurr
/// </summary>
/// <author> Fabian Schmurr, Laura Gietschel</author>
public class Scissor_interaction : Scripted_Interactable_Object
{
    /// <summary>
    /// Maximal opening angle between the two blade-components
    /// </summary>
    private float maxBladeAngle = 45;

    /// <summary>
    /// defined by testing around
    /// </summary>
    private float m_zOffset = 0.05f;
    private float m_xOffset = 0.01f;
    private float m_yOffset = 0.01f;
    private float m_rotationYOffset = 48.5f;
    private float m_rotationXOffset = 17.1f;
    private float m_rotationZOffset = 0.0f;

    /// <summary>
    /// Below this angle the cut events gets invoked
    /// </summary>
    private float cutThreshold = 3;

    /// <summary>
    /// Anchor to rotate the whole object around
    /// </summary>
    [SerializeField]
    private GameObject anchor;

    /// <summary>
    /// Lower part of scissor
    /// </summary>
    [Tooltip("Lower scissor-part")]
    [SerializeField]
    private GameObject untereKlinge;

    /// <summary>
    /// upper part of scissor
    /// </summary>
    [Tooltip("Upper scissor-part")]
    [SerializeField]
    private GameObject obereKlinge;

    /// <summary>
    /// Pivot point for rotation
    /// </summary>
    [Tooltip("Pivot point of scissor object")]
    [SerializeField]
    private GameObject pivot;

    [Tooltip("Try it if you�re a real ninja.")]
    public bool ninja = false;

    /// <summary>
    /// Min distance between index-distal and middle-distal joints
    /// </summary>
    private float minDistance = 0.03f;

    /// <summary>
    /// Max distance between index-distal and middle-distal joints
    /// </summary>
    private float maxDistance = 0.55f;

    /// <summary>
    /// Last Angle between scissor parts
    /// </summary>
    private float lastAngle;

    /// <summary>
    /// Reference to xr setup to calculate the object position
    /// </summary>
    private GameObject m_XROrigin;

    [SerializeField]
    [Tooltip("The name of xr-origin game-object")]
    private string m_XRSetupName;

    /// <summary>
    /// This action gets called if the conditions for a scissors cut are erfüllt
    /// </summary>
    public Action OnScissorsCut;

    /// <summary>
    /// Joints needed for this interaction
    /// </summary>
    private static XRHandJointID m_index_distal = XRHandJointID.IndexDistal;
    private static XRHandJointID m_middle_distal = XRHandJointID.MiddleDistal;
    private static XRHandJointID m_palm = XRHandJointID.Palm;
    private static XRHandJointID m_procimalMiddle = XRHandJointID.MiddleProximal;
    private static XRHandJointID m_procimalIndex = XRHandJointID.IndexProximal;
    private static XRHandJointID m_wrist = XRHandJointID.Wrist;

    /// <summary>
    /// Only passing the indices list to base constructor
    /// </summary>
    /// <author> Fabian Schmurr</author>
    /// <param name="jointList">List of joint indices</param>
    public Scissor_interaction() : base(GetJointList()){}


    /// <summary>
    /// Method that returns the list of joint indices
    /// </summary>
    /// <authors> Fabian Schmurr</authors>
    /// <returns>List of joint indices</returns>
    private static List<XRHandJointID> GetJointList()
    {
        ///creating list of static defined hand joint indices
        List<XRHandJointID> joints = new List<XRHandJointID>
        {
            m_index_distal,
            m_middle_distal,
            m_palm,
            m_procimalMiddle,
            m_procimalIndex,
            m_wrist
        };
        return joints;
    }

    /// <summary>
    /// Method that updates the scripted interaction by using the current hand joint data
    /// </summary>
    /// <authors> Fabian Schmurr, Laura Gietschel</authors>
    public override void updateInteraction()
    {
        getFinalTransform(MoveAnimation.AnimationAction.SELECT, out Vector3 finalPos, out Quaternion finalRot);
        //applying rotation and position
        pivot.transform.rotation = finalRot;
        pivot.transform.position = finalPos;

        if (!(necessaryJointData.TryGetValue(m_index_distal, out XRHandJoint indexDistalJoint)
        && necessaryJointData.TryGetValue(m_middle_distal, out XRHandJoint middleDistalJoint)
        && necessaryJointData.TryGetValue(m_procimalIndex, out XRHandJoint indexProcimalJoint)
        && necessaryJointData.TryGetValue(m_procimalMiddle, out XRHandJoint middleProcimalJoint)))
        { return; }

        if (!(indexDistalJoint.TryGetPose(out Pose indexDistalPose)
        && middleDistalJoint.TryGetPose(out Pose middleDistalPose)
        && indexProcimalJoint.TryGetPose(out Pose indexProcimalPose)
        && middleProcimalJoint.TryGetPose(out Pose middleProcimalPose)))
        { return; }

        //distance between distal index and middle finger
        float distance = Vector3.Distance(indexDistalPose.position, middleDistalPose.position);
        distance = Mathf.Clamp(distance, 0, maxBladeAngle);
        moveBlades(distance);
    }

    /// <summary>
    /// Rotates the blades of scissors correctly, this method is also invoking the OnCut Action
    /// </summary>
    /// <authors> Fabian Schmurr, Laura Gietschel</authors>
    /// <param name="distance">Distance between distal joints</param>
    private void moveBlades(float distance)
    {
        //remapping values https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        float angle = (distance - minDistance) / (0f - minDistance) * (maxBladeAngle - maxDistance) + maxDistance;

        if (ninja)
        {
            untereKlinge.transform.RotateAround(pivot.transform.position, Vector3.up, -angle / 2);
            obereKlinge.transform.RotateAround(pivot.transform.position, Vector3.up, angle / 2);
            return;
        }

        float delta = angle - lastAngle;

        //if (lastAngle - delta < 0 && lastAngle + delta > maxAngle)
        //{ return; }
        untereKlinge.transform.RotateAround(anchor.transform.position, anchor.transform.up, -delta / 2);
        obereKlinge.transform.RotateAround(anchor.transform.position, anchor.transform.up, delta / 2);
        lastAngle = angle;

        // Check for cut and ignoring negative angle
        if (Math.Abs(angle) < cutThreshold)
        {
            if (OnScissorsCut != null)
            {
                OnScissorsCut.Invoke();
            }
        }
    }


    /// <summary>
    /// Method that tries to get the reference of XROrigin
    /// </summary>
    /// <exception cref="NullReferenceException">If reference to XROrigin is null</exception>
    /// <authors> Fabian Schmurr, Laura Gietschel</authors>
    void Start()
    {
        //getting reference to XR origin
        m_XROrigin = GameObject.Find(m_XRSetupName);
        if(m_XROrigin == null)
        {
            throw new NullReferenceException("Could not found the xr setup with given name.");
        }

    }

    /// <summary>
    /// Method that is called once per frame
    /// </summary>
    /// <authors> Fabian Schmurr, Laura Gietschel</authors>
    void Update()
    {
    }

    /// <summary>
    /// Method that returns the final position and rotation of the object
    /// </summary>
    /// <authors> Fabian Schmurr</authors>
    /// <param name="animationAction">The animation action that is currently performed</param>
    /// <param name="finalPosition">The final position of the object</param>
    /// <param name="finalRotation">The final rotation of the object</param>
    /// <returns>True if the final position and rotation could be calculated, false otherwise</returns>
    public override bool getFinalTransform(in MoveAnimation.AnimationAction animationAction, out Vector3 finalPosition, out Quaternion finalRotation)
    {

        if (animationAction == MoveAnimation.AnimationAction.DETACH)
        {
            finalPosition = lastPositionBeforeActivation;
            finalRotation = lastRotationBeforeActivation;
            return true;
        }

        finalPosition = Vector3.zero;
        finalRotation = Quaternion.identity;

        //getting joint references
        if (!(necessaryJointData.TryGetValue(m_index_distal, out XRHandJoint index)
            && necessaryJointData.TryGetValue(m_middle_distal, out XRHandJoint middle)
            && necessaryJointData.TryGetValue(m_palm, out XRHandJoint palm)
            && necessaryJointData.TryGetValue(m_procimalMiddle, out XRHandJoint procimalMiddle)
            && necessaryJointData.TryGetValue(m_wrist, out XRHandJoint wrist)))
            {
                return false;
            }
        //getting poses of joints
        if (!(index.TryGetPose(out Pose indexPose)
            && middle.TryGetPose(out Pose middlePose)
            && palm.TryGetPose(out Pose palmPose)
            && procimalMiddle.TryGetPose(out Pose proximalMiddlePose)
            && wrist.TryGetPose(out Pose wristPose)))
            {
                return false;
            }

        //apply new pos
        Vector3 origin = m_XROrigin.transform.position;
        //transforming the offset from local in world space
        Vector3 offset = new Vector3(m_xOffset, m_yOffset, m_zOffset);
        Quaternion palmRotationInHeadsetSpace = palmPose.rotation;
        Vector3 offsetFromHand = palmRotationInHeadsetSpace * offset;
        Vector3 pointingDirection = palmPose.position - wristPose.position;
        finalPosition = palmPose.GetTransformedBy(new Pose(origin, m_XROrigin.transform.rotation)).position + offsetFromHand;

        //Forward vector for scissors
        Quaternion scissorsForward = Quaternion.LookRotation(pointingDirection, palmPose.rotation * Vector3.up);
        Quaternion rotationOffset = Quaternion.Euler(m_rotationXOffset, m_rotationYOffset, m_rotationZOffset);
        finalRotation = rotationOffset * scissorsForward;
        return true;
    }

    /// <summary>
    /// Method that is called when the object is activated
    /// </summary>
    /// <authors> Fabian Schmurr</authors>
    protected override void objectSpecificDeactivation()
    {
        /// Not implemented
    }

    /// <summary>
    /// Method that is called when the object is deactivated
    /// </summary>
    /// <authors> Fabian Schmurrl</authors>
    protected override void objectSpecificActivation()
    {
        /// Not implemented
    }
}