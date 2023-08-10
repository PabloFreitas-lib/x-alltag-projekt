using System;
using UnityEngine;
/// <summary>
///    This class is responsible to animate the movement of objects flying from a start to end point
///    The animated object must implement <see cref="ScriptedInteractableObject"/>
/// </summary>
/// <author> Fabian Schmurr</author>
[RequireComponent(typeof(ScriptedInteractableObject))]
public class MoveAnimation : MonoBehaviour
{
    /// <summary>
    /// Describes the animation type
    /// Currently <see cref="AnimationType.EASEIN"/> looks the best 
    /// </summary>
    public AnimationType easing = AnimationType.EASEIN;

    /// <summary>
    /// Event that is called if the animation has ended
    /// </summary>
    public Action OnAnimationEnd;

    /// <summary>
    /// The Material for LineRenderer to draw the path of object
    /// </summary>
    public Material drawingMaterial;

    /// <summary>
    /// For my understanding this should be the start and end color, but it does not affect the color at all LOL
    /// </summary>
    public Color pathColor;

    /// <summary>
    /// Time after the animation ends, this is therefore also the time the animated abject is flying
    /// </summary>
    public float maxAnimationTime = 1.5f;

    /// <summary>
    /// Width of drawn line of line-renderer
    /// </summary>
    public float animationWidth = 0.02f;

    /// <summary>
    /// Start/End rotation for animation
    /// </summary>
    private Quaternion m_endRotation;
    private Quaternion m_startRotation;

    /// <summary>
    /// End position for animation
    /// </summary>
    private Vector3 m_endPosition;

    /// <summary>
    /// Indicates whether the animation is running or not
    /// </summary>
    private bool m_isRunning = false;

    /// <summary>
    /// Threshold at which the animation stops (If current distance < threshhold). 
    /// This can be used to prevent glitching behaviour near the hand
    /// </summary>
    private float animationDistanceEndThreshhold = 0.005f;
    
    /// <summary>
    /// Time since animation is running
    /// </summary>
    private float timeAnimating = float.NegativeInfinity;

    /// <summary>
    /// Distance to final position
    /// </summary>
    private float remainingDistance = float.MaxValue;

    /// <summary>
    /// Position where the object was before animation started
    /// </summary>
    private float startDistance = float.NaN;
    private int lineIndex = 0;
    
    /// <summary>
    /// Reference to animated object
    /// </summary>
    private ScriptedInteractableObject objectToMove;

    private LineRenderer renderer;

    /// <summary>
    /// Indicates whether the current animation is due to a detach or select operation
    /// </summary>
    private AnimationAction animationAction;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    /// <author> Fabian Schmurr</author>
    void Start()
    {
    }

    /// <summary>
    /// Describes the animation progress type. Currently EASEIN looks the best
    /// </summary>
    /// <author> Fabian Schmurr</author>
    public enum AnimationType
    {
        LINEAR,
        EASEIN,
        EASEOUT,
        EASEINOUT
    }

    /// <summary>
    /// Indicates which action was performed that started the animation
    /// </summary>
    /// <author> Fabian Schmurr</author>
    public enum AnimationAction
    {
        SELECT,
        DETACH
    }

    /// <summary>
    /// Starts the animation of an object
    /// </summary>
    /// <author> Fabian Schmurr</author>
    /// <param name="objectToMove">The object that should be moved to a new position</param>
    /// <param name="action">Whether the animation is due to detach or select action. This is important
    /// because the end-position varies if the object is moving to the hand, since its an scripted_interaction
    /// and the final position therefore a specific joint at the hand <see cref="ScriptedInteractableObject.getFinalTransform(in AnimationAction, out Vector3, out Quaternion)"/></param>
    public void startAnimation(ScriptedInteractableObject objectToMove, AnimationAction action)
    {
        if (!m_isRunning)
        {
            m_isRunning = true;
            this.objectToMove = objectToMove;
            objectToMove.getFinalTransform(action, out m_endPosition, out m_endRotation);
            m_startRotation = objectToMove.transform.rotation;
            startDistance = Vector3.Distance(objectToMove.transform.position, m_endPosition);
            animationAction = action;
            setupRenderer();
        }
    }

    /// <summary>
    /// Set up line renderer to draw the line of animation
    /// </summary>
    /// <author> Fabian Schmurr</author>
    private void setupRenderer()
    {
        renderer = gameObject.GetComponent<LineRenderer>();
        renderer.material = drawingMaterial;
        renderer.startColor = renderer.endColor = pathColor;
        renderer.startWidth = renderer.endWidth = animationWidth;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    /// <author> Fabian Schmurr</author>
    void Update()
    {
        if (m_isRunning)
        {
            if (timeAnimating > maxAnimationTime)
            {
                endAnimation();
            }
            else
            {

                if (remainingDistance <= animationDistanceEndThreshhold)
                {
                    endAnimation();
                }
                else
                {
                    if(objectToMove.getFinalTransform(animationAction, out m_endPosition, out m_endRotation))
                    { animate(); }
                    
                }
            }
        }
    }

    /// <summary>
    /// Ends the animation and resets the values of MoveAnimation instance
    /// </summary>
    /// <author> Fabian Schmurr</author>
    private void endAnimation()
    {
        m_isRunning = false;
        timeAnimating = float.NegativeInfinity;
        remainingDistance = float.MaxValue;
        startDistance = float.NaN;
        m_startRotation = Quaternion.identity;
        if (renderer != null)
        {
            renderer.positionCount = 0;
            renderer = null;
        }
        if (OnAnimationEnd != null)
        {
            OnAnimationEnd.Invoke();
        }
    }

    /// <summary>
    /// Animates the object by using the current time and the therefore resulting percental progress   
    /// </summary>
    /// <author> Fabian Schmurr</author>
    private void animate()
    {
        if (timeAnimating == float.NegativeInfinity)
        {
            timeAnimating = 0;
        }
        timeAnimating += Time.deltaTime;
        //calculate forward vector
        Vector3 forward = m_endPosition - objectToMove.transform.position;
        remainingDistance = Vector3.Magnitude(forward);

        Vector3 forwardNorm = Vector3.Normalize(forward);

        double progress = getTimeProgress();

        float animationPosDif = (float)progress - (1 - remainingDistance / startDistance);
        objectToMove.transform.rotation = Quaternion.Slerp(m_startRotation, m_endRotation, (float)progress);
        objectToMove.transform.position += forwardNorm * animationPosDif;
        drawNextPoint();
    }

    /// <summary>
    /// Calculates the progress of the animation of given time under influence of the defined animation time.
    /// </summary>
    /// <author> Fabian Schmurr</author>
    /// <returns>Progress of animation</returns>
    private float getTimeProgress()
    {
        float x = timeAnimating / maxAnimationTime;
        double progress = 0;

        ///functions for easing are found on https://easings.net/ (GPL-3 License)
        switch (easing)
        {
            case AnimationType.LINEAR:
                progress = Mathf.Lerp(0, 1, x);
                break;
            case AnimationType.EASEIN:
                progress = x * x * x;
                break;
            case AnimationType.EASEOUT:
                progress = 1 - Math.Pow(1 - x, 3); ;
                break;
            case AnimationType.EASEINOUT:
                progress = x < 0.5 ? 4 * x * x * x : 1 - Math.Pow(-2 * x + 2, 3) / 2;
                break;
            default:
                break;
        }

        return (float)progress;
    }

    /// <summary>
    /// Draws the next point of the line renderer
    /// </summary>
    /// <author> Fabian Schmurr</author>
    private void drawNextPoint()
    {
        renderer.positionCount = lineIndex + 1;
        renderer.SetPosition(lineIndex, objectToMove.transform.position);
        lineIndex++;
    }
}



