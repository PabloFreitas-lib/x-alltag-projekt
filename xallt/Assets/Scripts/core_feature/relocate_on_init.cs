using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;


public class relocate_on_init : MonoBehaviour
{
    //Gesture to start relocating
    public InputActionProperty startGesture;
    //Variables used for calculations
    //anchor in vr-world
    public GameObject leftAnchor;
    //to set x,y pos of player
    public Transform xrOrigin;
    //to set rotation
    public GameObject mainCamera;
    //to set z offset
    public GameObject cameraOffset;
    private bool pinched = false;
    private Vector3 startPointEdge;
    private Vector3 endPointEdge;
    //offset to position the headset correctly in scene
    private float offset = 0.7f;
    //minimum length of line to be drawn on table 
    private const float min_line_length = 0.07f;
    private Pose currentDistanJointPose;
    private bool leftHandDataLoaded = false;
    LineRenderer lineRenderer;


    /**
    * See https://docs.unity3d.com/Manual/xr_input.html
    **/
    //List to store reference to input-devices for left hand to get later the finger tip location
    List<UnityEngine.XR.InputDevice> leftHandControllers = new List<UnityEngine.XR.InputDevice>();
    //characteristics for left hand
    UnityEngine.XR.InputDeviceCharacteristics desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.HandTracking;



    // Start is called before the first frame update
    void Start()
    {
        /**get subsystem for hands to access it's data
         * See https://docs.unity3d.com/Packages/com.unity.xr.hands@1.1/manual/hand-data/xr-hand-access-data.html for more information
         **/
        XRHandSubsystem m_Subsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRHandSubsystem>();

        //check if loaded system excists 
        if (m_Subsystem != null)
            m_Subsystem.updatedHands += OnHandUpdate;

        //initialized LineRenderer (thx to group mindmap)
        /*
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        */
    }


    // Gets by the subsystem if a hand update occurs
    void OnHandUpdate(XRHandSubsystem subsystem,
                      XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags,
                      XRHandSubsystem.UpdateType updateType)
    {
        switch (updateType)
        {
            //check if relocate action has already started
            case XRHandSubsystem.UpdateType.Dynamic:
                // Update game logic that uses hand data, also check if all left hand joints were loaded
                if (updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints || updateSuccessFlags == XRHandSubsystem.UpdateSuccessFlags.All)
                {
                    //printDebug("left hand joints loaded");
                    var trackingData = subsystem.leftHand.GetJoint(XRHandJointID.IndexDistal);
                    if (trackingData.TryGetPose(out currentDistanJointPose))
                    {
                        updateDistalPose(currentDistanJointPose);
                        //printDebug("left index finger distal joint pos at:\t" + currentDistanJointPose.position);
                        leftHandDataLoaded = true;
                    }
                    else leftHandDataLoaded = false;

                }

                break;
            case XRHandSubsystem.UpdateType.BeforeRender:
                // Update visual objects that use hand data

                break;
        }


    }


    // Update is called once per frame
    void Update()
    {
        //check if ring finger got pinched this frame
        if (startGesture.action.WasPressedThisFrame())
        {
            if (!pinched)
            {
                printDebug("ring-pinch");
                if (leftHandDataLoaded)
                {
                    setFingerStartPoint();
                    printDebug("Finger start pos at:" + startPointEdge);
                    //TODO trigger some information to display what to do
                    pinched = true;
                }
                else
                {
                    return;
                }
            }
        }
        else if (startGesture.action.WasReleasedThisFrame())
        {
            pinched = false;
        }
        else if (pinched)
        {
            //abort action if tracking data is lost
            if (!leftHandDataLoaded)
            {
                printDebug("Left hand data lost");
                pinched = false;
            }

            else if (leftHandDataLoaded && trySetFingerEndPoint())
            {
                printDebug("End pos of line at: " + endPointEdge);
                updateOriginPosition();
                pinched = false;
            }
        }
    }

    private void updateOriginPosition()
    {
        //vector from real-world edge to hmd
        Vector3 distanceStartHMD = mainCamera.transform.position - startPointEdge;
        printDebug("Distance HMD-start:\t" + distanceStartHMD);     
        Vector3 newPos = leftAnchor.transform.position + new Vector3(distanceStartHMD.x + 0.3f, -0.4f, distanceStartHMD.z - 0.1f);
        
        cameraOffset.transform.position = newPos;
        printDebug("New pos:\t" + newPos);

    }

    private void updateDistalPose(Pose pose)
    {
        currentDistanJointPose = pose;
    }

    private bool trySetFingerEndPoint()
    {
        float distance = Vector3.Distance(startPointEdge, currentDistanJointPose.position);
        /*
        lineRenderer.SetPosition(0, originToWorld * startPointEdge);
        lineRenderer.SetPosition(1, originToWorld * currentDistanJointPose.position);
        */
        if (distance > min_line_length)
        {
            endPointEdge = currentDistanJointPose.position;
            printDebug("end pos:\t" + endPointEdge);
            return true;
        }
        return false;
    }

    private void setFingerStartPoint()
    {
        startPointEdge = currentDistanJointPose.position;
    }

    private bool getLeftHandedController()
    {
        //loading XR input devices by given characteristics
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandControllers);

        //check if an left handed input device was found
        if (leftHandControllers.Count > 0)
        {
            foreach (var device in leftHandControllers)
            {
                printDebug(string.Format("Found device with name:\t{0}\t and characteristic:\t{1}", device.name, device.characteristics));
                //make sure the found input device is not a tracked hand
                if (device.characteristics != UnityEngine.XR.InputDeviceCharacteristics.HandTracking)
                {
                    //check for required features for input device
                    Vector3 position;
                    Quaternion rotation;
                    if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out position))
                    {
                        printDebug("Position available:\t" + position);
                        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out rotation))
                        {
                            printDebug("Rotation available:\t" + rotation);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private void setXROrigin()
    {
        // printDebug("controller at:" + leftController.action.ReadValue<Vector3>());
        printDebug("HMD at:" + xrOrigin.position);
        //Vector3 distanceLeftHMD = xrOrigin.position - leftController.action.ReadValue<Vector3>();
        //printDebug("Distance:" + distanceLeftHMD);
        // xrOrigin.position = new Vector3(leftAnchor.position.x + distanceLeftHMD.x, xrOrigin.position.y, leftAnchor.position.z + distanceLeftHMD.z + offset);
    }

    private void printDebug(string message)
    {
        if (Debug.isDebugBuild)
        {
            Debug.Log(message);
        }
    }
}
