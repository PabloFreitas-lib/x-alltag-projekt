using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

/// <summary>
///    This class is responsible for relocating the player in the scene
/// </summary>
/// <author> Fabian Schmurr</author>
public class RelocationOnInit : MonoBehaviour
{

    //Variables used for calculations
    //anchor in vr-world
    public GameObject leftAnchor;
    //to set x,y pos of player
    public Transform xrOrigin;
    //to set rotation
    public GameObject mainCamera;
    //to set z offset
    public GameObject cameraOffset;

    /// <summary>
    /// List of hand-joints of which the position-data is used for relocation/calibration
    /// </summary>
    public readonly List<XRHandJointID> handLandMarks = new List<XRHandJointID>()
    {
        XRHandJointID.IndexTip,
    };

    private bool relocated = false;

    private Pose relativeIndexPose;

    private Gesture relocateGesture;

    public Gesture.GestureType typeForRelocation;

    [SerializeField]
    [Range(0, 2)]
    private float gestureTime = 0.5f;

    [SerializeField]
    private GestureRecognizer gestureRecognizer;
    [SerializeField]
    private float m_xOffset;
    [SerializeField]
    private float m_yOffset = 0.1f;
    [SerializeField]
    private float m_zOffset;

    // Start is called before the first frame update
    void Start()
    {
        gestureRecognizer.OnGestureDetected.AddListener(OnGestureDetected);
        gestureRecognizer.OnGestureReleased.AddListener(OnGestureReleased);
    }

    private void OnGestureReleased(Gesture arg0)
    {
        relocateGesture = null;
    }

    private void OnGestureDetected(Gesture arg0)
    {
        if(arg0.type == Gesture.GestureType.STONE && arg0.handedness == Handedness.Right)
        {
            relocateGesture = arg0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!relocated)
        {
            checkForRelocation();
        }
    }

    private void checkForRelocation()
    {
       if(relocateGesture != null && relocateGesture.getTimePerformed() > gestureTime)
        {
            relocatePlayer();
            relocated = true;
        }
    }

    private void relocatePlayer()
    {
        Vector3 originPos = xrOrigin.transform.position;
        Quaternion originRot = xrOrigin.transform.rotation;
        Vector3 offset = new Vector3(m_xOffset, m_yOffset, m_zOffset);
        Vector3 indexWorldPos = relativeIndexPose.GetTransformedBy(new Pose(originPos, originRot)).position + offset;
        
        Vector3 distanceIndexHeadset = mainCamera.transform.position - indexWorldPos;
        Vector3 finalHeadsetPos = leftAnchor.transform.position + distanceIndexHeadset;
        
        
        float eulerYDiff = leftAnchor.transform.rotation.eulerAngles.y - mainCamera.transform.eulerAngles.y;
        Vector3 cameraRotation = cameraOffset.transform.rotation.eulerAngles;
        cameraRotation.y = cameraRotation.y += eulerYDiff;
        xrOrigin.transform.Rotate(new Vector3(0, 1, 0), eulerYDiff);
        Debug.Log(finalHeadsetPos);
        //xrOrigin.transform.position = finalHeadsetPos;

    }

    public bool updateData(Dictionary<XRHandJointID, XRHandJoint> jointsMap, Handedness handedness)
    {
        if(jointsMap.Count != handLandMarks.Count)
        { 
            return false;
        }

        if(!(jointsMap.TryGetValue(XRHandJointID.IndexTip, out XRHandJoint index) &&
            index.TryGetPose(out Pose indexPose)))
        {
            return false;
        }

        relativeIndexPose = indexPose;

        return true;
    }
}
