using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class cutting : MonoBehaviour
{
    public float maxAngle = 40;
    public GameObject untereKlinge;
    public GameObject obereKlinge;
    public GameObject pivot;
    public bool ninja = false;
    private float minDistance = 0.025f;
    private float maxDistance = 0.055f;
    private Pose currentDistalIndexJointPose;
    private Pose currentDistalMiddleJointPose;
    private bool dataLoaded = false;
    private float lastAngle;
    
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
                    var trackingIndexData = subsystem.leftHand.GetJoint(XRHandJointID.IndexDistal);
                    var trackingMiddleData = subsystem.leftHand.GetJoint(XRHandJointID.MiddleDistal);
                    dataLoaded = true;
                    if (trackingIndexData.TryGetPose(out currentDistalIndexJointPose))
                    { }
                    else dataLoaded = false;
                    if (trackingMiddleData.TryGetPose(out currentDistalMiddleJointPose))
                    { }
                    else dataLoaded = false;

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
        if(dataLoaded)
        {
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
    }
}
