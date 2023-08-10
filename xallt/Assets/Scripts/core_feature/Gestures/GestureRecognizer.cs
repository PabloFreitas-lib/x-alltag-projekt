using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;
using UnityEngine.XR.Hands;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GestureRecognizer : MonoBehaviour
{
    [FormerlySerializedAs("gestures")] [SerializeField] 
    private List<Gesture> savedGestures;

    [SerializeField] private float threshhold = 0.05f;

    [SerializeField] private Handedness handForGesture;
    [SerializeField] private Gesture.GestureType type;
    public GameObject prefab;
    public readonly List<XRHandJointID> handLandMarks = new List<XRHandJointID>()
    {
        XRHandJointID.ThumbTip,
        XRHandJointID.IndexTip,
        XRHandJointID.MiddleTip,
        XRHandJointID.RingTip,
        XRHandJointID.LittleTip,
        XRHandJointID.Palm
    };

    private List<Vector3> leftHandLandmarksPosition;

    private List<Vector3> righthandLandmarks;

    private Vector3 leftHandRoot;
    private Vector3 rightHandRoot;

    public GestureEvent OnGestureDetected;
    
    public class GestureEvent : UnityEvent<Gesture>{}
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Gesture gesture = checkForGesture();
        if(gesture != null && OnGestureDetected != null)
        {
            OnGestureDetected.Invoke(gesture);
        }
    }

    public bool updateData(Dictionary<XRHandJointID, XRHandJoint> jointsMap, Handedness handedness)
    {
        //check if dictionary has correct length
        if (jointsMap.Count != handLandMarks.Count)
        {
            return false;
        }

        List<Vector3> jointPositions = new List<Vector3>();
        if (!(jointsMap.TryGetValue(XRHandJointID.Palm, out XRHandJoint palmJoint) && 
              palmJoint.TryGetPose(out Pose palmPose)))
        {
            return false;
        }

        Pose palm = new Pose();
        foreach (var index in handLandMarks)
        {
            if (index != XRHandJointID.Palm)
            {
                if (jointsMap.TryGetValue(index, out XRHandJoint joint) 
                    && joint.TryGetPose(out palm))
                {
                    Vector3 positionRelativeToPalm = transformJointRelativeToPalm(palm, palmPose);
                    jointPositions.Add(positionRelativeToPalm);
                }
                else
                {
                    return false;
                }
            }
        }

        if (handedness == Handedness.Left)
        {
            leftHandLandmarksPosition = jointPositions;
            leftHandRoot = palm.position;
        }
        else if(handedness == Handedness.Right)
        {
            righthandLandmarks = jointPositions;
            rightHandRoot = palm.position;
        }

        return true;
    }

    private Vector3 transformJointRelativeToPalm(Pose pose, Pose palmPose)
    {
        return pose.position - palmPose.position;
    }

    public Gesture checkForGesture()
    {
        return getNearestGesture(getPossibleResults());
    }

    //This method is mostly copied from
    //https://github.com/jorgejgnz/HandTrackingGestureRecorder/blob/master/GestureRecognizer.cs
    private List<Result> getPossibleResults()
    {
        bool discardGesture = false;
        float minSumDistances = Mathf.Infinity;
        Gesture bestCandidate = null;
        List<Result> results = new List<Result>();

        // For each gesture
        for (int g = 0; g < savedGestures.Count; g++)
        {
            Gesture gesture = savedGestures[g];
            if (gesture == null)
            {
                continue;
            }

            float gestureError = calculateGestureError(gesture);
            results.Add(new Result(gesture, gestureError));
        }

        if (results != null)
        {
            results.Sort();
        }
        // If we've found something, we'll return it
        // If we haven't found anything, we return it anyway (newly created object)
        return results;
    }

    private float calculateGestureError(Gesture gesture)
    {
        float error;
        List<Vector3> joinData = null;
        Vector3 hand;
        if (gesture.handedness == Handedness.Left && leftHandLandmarksPosition.Count == gesture.joints.Count)
        {
            joinData = leftHandLandmarksPosition;
        }
        else if (gesture.handedness == Handedness.Right && leftHandLandmarksPosition.Count == gesture.joints.Count)
        {
            joinData = righthandLandmarks;
        }
        else
        {
            return -1;
        }

        float sumDistances = 0f;
        bool discardGesture = false;

        // For each finger
        for (int f = 0; f < joinData.Count; f++)
        {
            //TODO make palm a center of coordinate system in this case 
            //Vector3 fingerRelativePos = hand.transform.InverseTransformPoint(fingers[f].transform.position);
            float difference = Vector3.Magnitude(joinData[f]) - Vector3.Magnitude(gesture.joints[f]);
            // If at least one finger does not enter the threshold we discard the gesture
            if (Mathf.Abs(difference) > threshhold)
            {
                discardGesture = true;
                break;
            }

            // If all the fingers entered, then we calculate the total of their distances
            sumDistances += Mathf.Pow(Mathf.Abs(difference), 2);
        }

        // If we have to discard the gesture, we skip it
        if (discardGesture)
        {
            return -1f;
        }

        return sumDistances;
    }

    private float calculateGestureErrorLeftHand(Gesture gesture)
    {
        throw new System.NotImplementedException();
    }


    private Gesture getNearestGesture(List<Result> results)
    {
        Gesture gesture = null;
        if (results.Count > 0)
        {
            results.Sort();
            Result bestResult = results[^1];
            gesture = bestResult._gesture;
        }
        return gesture;
    }
    
    
    private void SaveAsGesture()
    {
        List<Vector3> joints;
        if (handForGesture == Handedness.Left)
        {
            joints = leftHandLandmarksPosition;
        }
        else if (handForGesture == Handedness.Right)
        {
            joints = righthandLandmarks;
        }
        else
        {
            return;
        }
        savedGestures.Add(new Gesture(type, joints, handForGesture));
    }
    
    
    private class Result : IComparer<Result>
    {
        public readonly Gesture _gesture;
        public readonly float error;

        public Result(Gesture gesture, float error)
        {
            _gesture = gesture;
            this.error = error;
        }

        public int Compare(Result x, Result y)
        {
            if (x == null || y == null) return 0;
            if (x.error > y.error) return 1;
            if (x.error < y.error) return -1;
            return 0;
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(GestureRecognizer))]
    public class CustomInspectorGestureRecognizer : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GestureRecognizer gestureRecognizer = (GestureRecognizer)target;
            if (!GUILayout.Button("Save current gesture")) return;
            gestureRecognizer.SaveAsGesture();
        }
    }
#endif
}
