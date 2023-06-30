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
    private float maxAngle = 40;

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
    private float minDistance = 0.025f;

    /// <summary>
    /// Max distance between index-distal and middle-distal joints
    /// </summary>
    private float maxDistance = 0.055f;

    /// <summary>
    /// Last Angle between scissor parts
    /// </summary>
    private float lastAngle;

    /// <summary>
    /// Joints needed for this interaction
    /// </summary>
    private static XRHandJointID m_index_distal = XRHandJointID.IndexDistal;
    private static XRHandJointID m_middle_distal = XRHandJointID.MiddleDistal;
    private XRHandJoint indexDistalJoint;
    private XRHandJoint middleIndexJoint;
    private Pose currentDistalIndexJointPose;
    private Pose currentDistalMiddleJointPose;

    public Scissor_interaction() : base(GetJointList())
    {
    }

    private static List<XRHandJointID> GetJointList()
    {
        List<XRHandJointID> joints = new List<XRHandJointID>();
        joints.Add(m_index_distal);
        joints.Add(m_middle_distal);
        return joints;
    }

    public override void updateInteraction()
    {
        
        necessaryJointData.TryGetValue(m_index_distal, out indexDistalJoint);
        necessaryJointData.TryGetValue(m_middle_distal, out middleIndexJoint);
        indexDistalJoint.TryGetPose(out currentDistalIndexJointPose);
        middleIndexJoint.TryGetPose(out currentDistalMiddleJointPose);

        float distance = Vector3.Distance(currentDistalIndexJointPose.position, currentDistalMiddleJointPose.position);
        distance = Mathf.Clamp(distance, 0, maxAngle);
        //remapping values https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        float angle = (distance - minDistance) / (0f - minDistance) * (maxAngle - maxDistance) + maxDistance;
        if (ninja)
        {
            untereKlinge.transform.RotateAround(pivot.transform.position, Vector3.up, -angle / 2);
            obereKlinge.transform.RotateAround(pivot.transform.position, Vector3.up, angle / 2);
            return;
        }

        float delta = angle - lastAngle;

        if (lastAngle - delta < 0 && lastAngle + delta > maxAngle)
        { return; }
        untereKlinge.transform.RotateAround(pivot.transform.position, pivot.transform.forward, -delta / 2);
        obereKlinge.transform.RotateAround(pivot.transform.position, pivot.transform.forward, delta / 2);
        lastAngle = angle;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
