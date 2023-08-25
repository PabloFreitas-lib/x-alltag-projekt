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

/// <summary>
/// Class that can detect and save a performed gesture during runtime
/// </summary>
///  <author>Fabian Schmurr</author
public class GestureRecognizer : MonoBehaviour
{
    /// <summary>
    /// List of gesture that should be checked for by recognizer
    /// </summary>
    [FormerlySerializedAs("gestures")]
    [SerializeField]
    private List<Gesture> savedGestures;
    
    /// <summary>
    /// Max deviation per joint that is allowed.
    /// If a deviation above this threshold occurs, a gesture is not detected
    /// </summary>
    [SerializeField] private float threshhold = 0.08f;

    /// <summary>
    /// This value is used if a gesture gets saved and indicates
    /// with which hand the gesture to save was performed
    /// </summary>
    [SerializeField] private Handedness handForGesture;
    
    /// <summary>
    /// This value is used if a gesture gets saved an defines the type of the saved gesture.
    /// </summary>
    [SerializeField] private Gesture.GestureType type;
    
    /// <summary>
    /// List of hand-joints of which the position-data gets saved and is later used to detect a gesture
    /// </summary>
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

    private List<Vector3> righthandLandmarksPosition;

    private Gesture lastGestureRecognized;

    public GestureEvent OnGestureDetected = new GestureEvent();
    public GestureEvent OnGestureReleased = new GestureEvent();

    [System.Serializable]
    public class GestureEvent : UnityEvent<Gesture> { }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Gesture gesture = checkForGesture();
        if (gesture != null)
        {
            //new gesture was recognized and before there wasn't a gesture recognized
            if (lastGestureRecognized == null && OnGestureDetected != null)
            {
                OnGestureDetected.Invoke(gesture);
            }
            else if (!lastGestureRecognized.Equals(gesture) && OnGestureReleased != null)
            {
                //inform components that a new gesture is performed
                OnGestureReleased.Invoke(lastGestureRecognized);
                OnGestureDetected.Invoke(gesture);
            }

            lastGestureRecognized = gesture;
        }
        else
        {
            //inform components that the last gesture is no longer performed 
            if (lastGestureRecognized != null)
            {
                if (OnGestureDetected != null)
                {
                    OnGestureReleased.Invoke(lastGestureRecognized);
                }
                lastGestureRecognized = null;
            }
        }
    }

    /// <summary>
    /// This method is used to update the current joint position data, of both left and right hand
    /// </summary>
    /// <param name="jointsMap"></param> A map  containing the HandJointId´s  as keys
    /// and XRHandJoint objects as values. Mapping must be correct so the right data is saved correct!
    /// <param name="handedness"></param> Indicates whether the given information symbols the left or right hand
    /// <returns>true</returns> data was updated correct
    /// <author>Fabian Schmurr</author>
    public bool updateData(Dictionary<XRHandJointID, XRHandJoint> jointsMap, Handedness handedness)
    {
        //check if dictionary has correct length
        if (jointsMap.Count != handLandMarks.Count)
        {
            return false;
        }

        List<Vector3> jointPositions = new List<Vector3>();
        //check if palm information can be accessed
        if (!(jointsMap.TryGetValue(XRHandJointID.Palm, out XRHandJoint palmJoint) &&
              palmJoint.TryGetPose(out Pose palmPose)))
        {
            return false;
        }

        Pose palm = new Pose();
        //check for given map if each necessary joint-id is present and it´s value can be accessed 
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

        //save extracted data for left or right hand
        if (handedness == Handedness.Left)
        {
            leftHandLandmarksPosition = jointPositions;
        }
        else if (handedness == Handedness.Right)
        {
            righthandLandmarksPosition = jointPositions;
        }

        return true;
    }

    /// <summary>
    /// Calcuated the relative vector between given point. Pretty simple
    /// </summary>
    /// <param name="pose"></param>
    /// <param name="palmPose"></param>
    /// <returns>The vector between given poses</returns>
    /// <author>Fabian Schmurr</author>
    private Vector3 transformJointRelativeToPalm(Pose pose, Pose palmPose)
    {
        return pose.position - palmPose.position;
    }

    /// <summary>
    /// Checks if a gesture was performed. In Theory here could happen some more things
    /// </summary>
    /// <returns>Gesture that was performed last frame</returns> is null if no gesture was detected
    /// No author necessary at this pint ;D
    public Gesture checkForGesture()
    {
        return getNearestGesture(getPossibleResults());
    }

    //This method is mostly copied from
    //https://github.com/jorgejgnz/HandTrackingGestureRecorder/blob/master/GestureRecognizer.cs
    /// <summary>
    /// Method that creates a list of detected gestures.
    /// </summary>
    /// <returns>List of results. Each Result indicates how big the error of detected Gesture was</returns>
    /// <author>Fabian Schmurr</author>
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
            if (gestureError >= 0)
            {
                results.Add(new Result(gesture, gestureError));
            }
        }
        // If we've found something, we'll return it
        // If we haven't found anything, we return it anyway (newly created object)
        return results;
    }

    /// <summary>
    /// Method that calculates the error of given gesture. This means basically the deviation of saved vertices
    /// of a gesture and the current vertices of actual hand tracking data
    /// </summary>
    /// <param name="gesture">Gesture to detect</param>
    /// <returns>The calculated error. -1 if hand data was not present or gesture was discarded due to a too high
    /// deviation</returns>
    /// <author>Fabian Schmurr</author>
    private float calculateGestureError(Gesture gesture)
    {
        float error;
        List<Vector3> joinData = null;
        Vector3 hand;
        //use data for left or right hand. This is decided by gesture object
        if (leftHandLandmarksPosition != null && gesture.handedness == Handedness.Left && leftHandLandmarksPosition.Count == gesture.joints.Count)
        {
            joinData = leftHandLandmarksPosition;
        }
        else if (righthandLandmarksPosition != null && gesture.handedness == Handedness.Right && righthandLandmarksPosition.Count == gesture.joints.Count)
        {
            joinData = righthandLandmarksPosition;
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
            //TODO calibrate  

            //Vector3 fingerRelativePos = hand.transform.InverseTransformPoint(fingers[f].transform.position);
            float difference = Vector3.Magnitude(joinData[f]) - Vector3.Magnitude(gesture.joints[f]);
            // If at least one finger does not enter the threshold we discard the gesture
            //TODO do not throw Gesture away if one single finger was over threshold 
            if (Mathf.Abs(difference) > gesture.threshold)
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
            gesture.resetTimePerformed();
            return -1f;
        }

        return sumDistances;
    }

    /// <summary>
    /// Method that returns the best detected gesture. Meaning the gesture with the smallest error
    /// </summary>
    /// <param name="results">List of found results</param>
    /// <returns>Gesture object with the smallest calculated error</returns>
    private Gesture getNearestGesture(List<Result> results)
    {
        Gesture gesture = null;
        if(results.Count > 0)
        {
            Result bestResult = results[^1];
            if (results.Count > 1)
            {
                results.Sort((x, y) => y.error.CompareTo(x.error));
                bestResult = results[^1];
            }

            gesture = bestResult._gesture;
            gesture.increaseTimePerformed(Time.deltaTime);
        }

        return gesture;
    }

    /// <summary>
    /// Method that saves a gesture that is currently performed and creates a new gameobject that holds
    /// the gesture. A bit nasty.
    /// </summary>
    /// <author>Fabian Schmurr</author>
    private void SaveAsGesture()
    {
        List<Vector3> joints;
        if (handForGesture == Handedness.Left)
        {
            joints = leftHandLandmarksPosition;
        }
        else if (handForGesture == Handedness.Right)
        {
            joints = righthandLandmarksPosition;
        }
        else
        {
            return;
        }


        GameObject gestureObj = new GameObject();
        gestureObj.AddComponent<Gesture>();
        Gesture toSafe = gestureObj.GetComponent<Gesture>();
        toSafe.joints = joints;
        toSafe.type = type;
        toSafe.handedness = handForGesture;
        toSafe.threshold = threshhold;
    }


    /// <summary>
    /// Class that represents a single Result calculated by the recognizer.
    /// </summary>
    /// <author>Fabian Schmurr</author>
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

    /// <summary>
    /// Dialog in editor that is used to define a gesture and save it while game is paused
    /// </summary>
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
