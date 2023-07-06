using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

public class Interactable_Pen : Scripted_Interactable_Object
{

    private static XRHandJointID m_indexDistalJointID = XRHandJointID.IndexDistal;
    private static XRHandJointID m_indexProximalJointId = XRHandJointID.IndexProximal;
    private GameObject m_XROrigin;
    [SerializeField]
    [Tooltip("The name of xr-origin game-object")]
    private string m_XRSetupName;

    public Interactable_Pen(List<XRHandJointID> necessaryJoints) : base(GetJointList()) { }


    private static List<XRHandJointID> GetJointList()
    {
        List<XRHandJointID> joints = new List<XRHandJointID>();
        joints.Add(m_indexDistalJointID);
        joints.Add(m_indexProximalJointId);

        return joints;
    }

    public override bool getFinalTransform(in MoveAnimation.AnimationAction animationAction, out Vector3 finalPosition, out Quaternion finalRotation)
    {
        throw new NotImplementedException();
    }

    public override void updateInteraction()
    {
        //getting pose data
        if (!(necessaryJointData.TryGetValue(m_indexDistalJointID, out XRHandJoint indexDistal)
            && necessaryJointData.TryGetValue(m_indexProximalJointId, out XRHandJoint middleProcimal)))
        {
            return;
        }

        if (!(indexDistal.TryGetPose(out Pose indexProximalPose)
            && middleProcimal.TryGetPose(out Pose indexDistalPose)))
        {
            return;
        }

        //apply new pos
        Vector3 origin = m_XROrigin.transform.position;
        //transforming the offset from local in world space
        //Vector3 offsetFromHand = palmRotationInHeadsetSpace * offset;
        Vector3 pointingDirection = indexProximalPose.position - indexDistalPose.position;

        //Forward vector for scissors
        Quaternion scissorsForward = Quaternion.LookRotation(pointingDirection, indexDistalPose.rotation * Vector3.up);
        //Quaternion rotationOffset = Quaternion.Euler(m_rotationXOffset, m_rotationYOffset, m_rotationZOffset);

        transform.rotation = scissorsForward;
        transform.position = indexDistalPose.GetTransformedBy(new Pose(origin, m_XROrigin.transform.rotation)).position;
    }

    // Start is called before the first frame update
    void Start()
    {
        //getting reference to XR origin
        m_XROrigin = GameObject.Find(m_XRSetupName);
        if (m_XROrigin == null)
        {
            throw new NullReferenceException("Could not found the xr setup with given name.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
