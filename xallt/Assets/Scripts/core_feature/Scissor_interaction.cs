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

    [SerializeField]
    [Range(0,2)]
    private float m_zOffset;
    [SerializeField]
    [Range(0, 2)]
    private float m_xOffset;
    [SerializeField]
    [Range(0, 2)]
    private float m_yOffset;

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
    /// Reference to xr setup to calculate the object position
    /// </summary>
    private GameObject m_XROrigin;

    [SerializeField]
    [Tooltip("The name of xr-origin game-object")]
    private string m_XRSetupName;

    /// <summary>
    /// Joints needed for this interaction
    /// </summary>
    private static XRHandJointID m_index_distal = XRHandJointID.IndexDistal;
    private static XRHandJointID m_middle_distal = XRHandJointID.MiddleDistal;
    private static XRHandJointID m_palm = XRHandJointID.Palm;

    public Scissor_interaction() : base(GetJointList())
    {
    }

    private static List<XRHandJointID> GetJointList()
    {
        List<XRHandJointID> joints = new List<XRHandJointID>();
        joints.Add(m_index_distal);
        joints.Add(m_middle_distal);
        joints.Add(m_palm);
        return joints;
    }

    public override void updateInteraction()
    {
        //getting pose data
        if(!(necessaryJointData.TryGetValue(m_index_distal, out XRHandJoint index) 
            && necessaryJointData.TryGetValue(m_middle_distal, out XRHandJoint middle)
            && necessaryJointData.TryGetValue(m_palm, out XRHandJoint palm)))
        {
            return;
        }

        if(!(index.TryGetPose(out Pose indexPose) 
            && middle.TryGetPose(out Pose middlePose)
            && palm.TryGetPose(out Pose palmPose)))
        {
            return;
        }
        
        float distance = Vector3.Distance(indexPose.position, middlePose.position);
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

        //apply new pos
        Vector3 origin = m_XROrigin.transform.position;
        Vector3 originToPivot = origin - palmPose.position;
        Vector3 offset = new Vector3(m_xOffset, m_yOffset, m_zOffset);
        //pivot.transform.position = origin + palmPose.position;

        //apply new rotation
        pivot.transform.position = origin + palmPose.position;
        //pivot.transform.SetPositionAndRotation(origin + palmPose.position + offset, palmPose.rotation);

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
