using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to represent a gesture. Gives information about the defined type
/// and the time for that the gesture is currently performed
/// </summary>
/// <author>Fabian Schmurr</author>
public class Gesture : MonoBehaviour
{
    /// <summary>
    /// List of position data that is used to detect a gesture
    /// </summary>
    public List<Vector3> joints;

    /// <summary>
    /// Type of this gesture
    /// </summary>
    public GestureType type;

    /// <summary>
    /// Which hand is performing this gesture
    /// </summary>
    public UnityEngine.XR.Hands.Handedness handedness;

    /// <summary>
    /// Indicates for how long this gesture is currently performed
    /// </summary>
    private float timePerfored = 0f;

    [SerializeField]
    public float threshold = 0.1f;

    /// <summary>
    /// Overloading of Equals-Method, so equality is only based on the defined type
    /// </summary>
    /// <param name="other"></param>
    /// <returns>true</returns> if equal
    /// <author>Fabian Schmurr</author>
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

    public Gesture(GestureType type, List<Vector3> joints, UnityEngine.XR.Hands.Handedness handedness, float threshold)
    {
        this.joints = joints;
        this.type = type;
        this.handedness = handedness;
        this.threshold = threshold;
    }

    public float getTimePerformed()
    { return timePerfored; }

    public void resetTimePerformed()
    {
        timePerfored = 0f;
    }
    public void increaseTimePerformed(float delta)
    {
        timePerfored += delta;
    }

    /// <summary>
    /// Possible types of a gesture
    /// </summary>
    /// <author>Fabian Schmurr</author
    public enum GestureType
    {
        DETACH,
        GRAB_PEN,
        GRAB_SCISSORS,
        DRAW
    }
}
