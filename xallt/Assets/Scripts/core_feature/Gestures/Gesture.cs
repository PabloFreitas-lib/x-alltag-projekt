using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Gesture : MonoBehaviour
{
        
    public readonly List<Vector3> joints;

    public readonly GestureType type;
    
    public readonly UnityEngine.XR.Hands.Handedness handedness;
    
    public Gesture(GestureType type, List<Vector3> joints, UnityEngine.XR.Hands.Handedness handedness)
    {
        this.joints = joints;
        this.type = type;
        this.handedness = handedness;
    }
    
    public enum GestureType
    {
        DETACH,
        GRAB_PEN,
        GRAB_SCISSORS,
        DRAW
    }
}
