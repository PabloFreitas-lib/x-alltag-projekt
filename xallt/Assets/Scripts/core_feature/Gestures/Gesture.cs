using System;
using System.Collections.Generic;
using UnityEngine;

public class Gesture : MonoBehaviour
{
        
    public List<Vector3> joints;

    public GestureType type;
    
    public UnityEngine.XR.Hands.Handedness handedness;

    protected bool Equals(Gesture other)
    {
        return base.Equals(other) && type == other.type && handedness == other.handedness;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Gesture)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), (int)type, (int)handedness);
    }

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
