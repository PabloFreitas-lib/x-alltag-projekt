using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
/// <summary>
/// Scripted scissor-interaction based on earlier script cutting.cs by Laura Gietschel and Fabian Schmurr
/// Author Fabian Schmurr
/// </summary>
public class Scissor_interaction : Scripted_Interactable_Object
{
    /// <summary>
    /// Maximal opening angle between the two blade-components
    /// </summary>
    [Tooltip("The maximal opening angle between the two blade parts")]
    [Range(20, 60)]
    [SerializeField]
    private float maxAngle = 37;

    /// <summary>
    /// defined by testing around
    /// </summary>
    private float m_zOffset = 0.05f;
    private float m_xOffset = 0.01f;
    private float m_yOffset = 0.01f;
    private float m_rotationYOffset = 48.5f;
    private float m_rotationXOffset = 17.1f;
    private float m_rotationZOffset = 0.0f;

    private float cutThreshold = 5;

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

    [Tooltip("Try it if you´re a real ninja.")]
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

    public Action OnScissorsCut;

    /// <summary>
    /// Joints needed for this interaction
    /// </summary>
    private static XRHandJointID m_index_distal = XRHandJointID.IndexDistal;
    private static XRHandJointID m_middle_distal = XRHandJointID.MiddleDistal;
    private static XRHandJointID m_palm = XRHandJointID.Palm;
    private static XRHandJointID m_procimalMiddle = XRHandJointID.MiddleProximal;
    private static XRHandJointID m_wrist = XRHandJointID.Wrist;

    /// <summary>
    /// Only passing the indices list to base constructor
    /// </summary>
    public Scissor_interaction() : base(GetJointList())
    {
    }

    private static List<XRHandJointID> GetJointList()
    {
        ///creating list of static defined hand joint indices
        List<XRHandJointID> joints = new List<XRHandJointID>();
        joints.Add(m_index_distal);
        joints.Add(m_middle_distal);
        joints.Add(m_palm);
        joints.Add(m_procimalMiddle);
        joints.Add(m_wrist);
        return joints;
    }

    /// <summary>
    /// Method that updates the scripted interaction by using the current hand joint data
    /// </summary>
    public override void updateInteraction()
    {
        //getting joint references
        if(!(necessaryJointData.TryGetValue(m_index_distal, out XRHandJoint index) 
            && necessaryJointData.TryGetValue(m_middle_distal, out XRHandJoint middle)
            && necessaryJointData.TryGetValue(m_palm, out XRHandJoint palm)
            && necessaryJointData.TryGetValue(m_procimalMiddle, out XRHandJoint procimalMiddle)
            &&  necessaryJointData.TryGetValue(m_wrist, out XRHandJoint wrist)))
        {
            return;
        }
        //getting poses of joints
        if(!(index.TryGetPose(out Pose indexPose) 
            && middle.TryGetPose(out Pose middlePose)
            && palm.TryGetPose(out Pose palmPose)
            && procimalMiddle.TryGetPose(out Pose proximalMiddlePose)
            && wrist.TryGetPose(out Pose wristPose)))
        {
            return;
        }

        //distance between distal index and middle finger
        float distance = Vector3.Distance(indexPose.position, middlePose.position);
        distance = Mathf.Clamp(distance, 0, maxAngle);
        moveBlades(distance);

        //apply new pos
        Vector3 origin = m_XROrigin.transform.position;
        //transforming the offset from local in world space
        Vector3 offset = new Vector3(m_xOffset, m_yOffset, m_zOffset);
        Quaternion palmRotationInHeadsetSpace = palmPose.rotation;
        Vector3 offsetFromHand = palmRotationInHeadsetSpace * offset;
        Vector3 pointingDirection = palmPose.position - wristPose.position;

        //Forward vector for scissors
        Quaternion scissorsForward = Quaternion.LookRotation(pointingDirection, palmPose.rotation * Vector3.up);
        Quaternion rotationOffset = Quaternion.Euler(m_rotationXOffset, m_rotationYOffset, m_rotationZOffset);

        //applying rotation and position
        pivot.transform.rotation = rotationOffset * scissorsForward;
        pivot.transform.position = palmPose.GetTransformedBy(new Pose(origin, m_XROrigin.transform.rotation)).position + offsetFromHand;
    }

    /// <summary>
    /// Rotates the blades of scissors correctly, this method is also invoking the OnCut Action
    /// </summary>
    /// <param name="distance">Distance between distal joints</param>
    private void moveBlades(float distance)
    {
        //remapping values https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        float angle = (distance - minDistance) / (0f - minDistance) * (maxAngle - maxDistance) + maxDistance;

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
            OnScissorsCut.Invoke();
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        //getting reference to XR origin
        m_XROrigin = GameObject.Find(m_XRSetupName);
        if(m_XROrigin == null)
        {
            throw new NullReferenceException("Could not found the xr setup with given name.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
