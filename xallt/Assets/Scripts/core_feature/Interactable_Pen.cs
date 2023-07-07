using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

[RequireComponent(typeof(VRDrawingManager))]
public class Interactable_Pen : Scripted_Interactable_Object
{

    private static XRHandJointID m_indexDistalJointID = XRHandJointID.IndexDistal;
    private static XRHandJointID m_indexProximalJointId = XRHandJointID.IndexProximal;
    private GameObject m_XROrigin;
    [SerializeField]
    [Tooltip("The name of xr-origin game-object")]
    private string m_XRSetupName;

    public Interactable_Pen() : base(GetJointList()) { }

    private static List<XRHandJointID> GetJointList()
    {
        List<XRHandJointID> joints = new List<XRHandJointID>()
        {m_indexDistalJointID,
        m_indexProximalJointId
        };
        return joints;
    }


    [SerializeField]
    [Range(-0.1f, 0.1f)]
    private float m_zOffset = 0f;
    [SerializeField]
    [Range(-0.1f, 0.1f)]
    private float m_xOffset = 0f;
    [SerializeField]
    [Range(-0.1f, 0.1f)]
    private float m_yOffset = 0f;
    [SerializeField]
    [Range(-30, 30)]
    private float m_rotationYOffset = 0f;
    [SerializeField]
    [Range(-30, 30)]
    private float m_rotationXOffset = 0f;
    [SerializeField]
    [Range(-30, 30)]
    private float m_rotationZOffset = 0.0f;

    private VRDrawingManager drawingManager;


    public override bool getFinalTransform(in MoveAnimation.AnimationAction animationAction, out Vector3 finalPosition, out Quaternion finalRotation)
    {
        finalPosition = Vector3.zero;
        finalRotation = Quaternion.identity;
        if (animationAction == MoveAnimation.AnimationAction.SELECT)
        {
            //getting pose data
            if (!(necessaryJointData.TryGetValue(m_indexDistalJointID, out XRHandJoint indexDistal)
                && necessaryJointData.TryGetValue(m_indexProximalJointId, out XRHandJoint middleProcimal)))
            {
                return false;
            }

            if (!(indexDistal.TryGetPose(out Pose indexProximalPose)
                && middleProcimal.TryGetPose(out Pose indexDistalPose)))
            {
                return false;
            }


            //apply new pos
            Vector3 origin = m_XROrigin.transform.position;
            //transforming the offset from local in world space
            Vector3 offset = new Vector3(m_xOffset, m_yOffset, m_zOffset);
            Quaternion palmRotationInHeadsetSpace = indexProximalPose.rotation;
            Vector3 offsetFromHand = palmRotationInHeadsetSpace * offset;
            Vector3 pointingDirection = indexProximalPose.position - indexDistalPose.position;
            //apply new pos
            //transforming the offset from local in world space
            finalPosition = indexProximalPose.GetTransformedBy(new Pose(origin, m_XROrigin.transform.rotation)).position + offsetFromHand;

            //Forward vector for scissors
            Quaternion scissorsForward = Quaternion.LookRotation(pointingDirection, indexDistalPose.rotation * Vector3.up);
            Quaternion rotationOffset = Quaternion.Euler(m_rotationXOffset, m_rotationYOffset + 90, m_rotationZOffset);
            //Quaternion rotationOffset = Quaternion.Euler(m_rotationXOffset, m_rotationYOffset, m_rotationZOffset);

            finalRotation = rotationOffset * scissorsForward;

            return true;
        }
        if(animationAction == MoveAnimation.AnimationAction.DETACH)
        {
            finalPosition = lastPositionBeforeActivation;
            finalRotation = lastRotationBeforeActivation;
            return true;
        }
        return false;
    }

    public override void updateInteraction()
    {
        getFinalTransform(MoveAnimation.AnimationAction.SELECT, out Vector3 finalPos, out Quaternion finalRot);
        transform.rotation = finalRot;
        transform.position = finalPos;
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

    protected override void objectSpecificDeactivation()
    {      
        if (drawingManager != null)
        {
            drawingManager.SetDrawingHand(Handedness.Invalid);
            drawingManager.ClearDrawing();
        }
    }

    protected override void objectSpecificActivation()
    {
        drawingManager = GetComponent<VRDrawingManager>();
        drawingManager.SetDrawingHand(controllingHand);
    }
}
