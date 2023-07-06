using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour
{
    /// <summary>
    /// Describes the animation type
    /// </summary>
    public AnimationType easing = AnimationType.EASEINOUT;

    /// <summary>
    /// Event that is called if the maximal animation was reached
    /// </summary>
    public Action OnAnimationEnd;

    /// <summary>
    /// The Material for LineRenderer to draw the path of object
    /// </summary>
    public Material drawingMaterial;

    /// <summary>
    /// For my understanding this should be somehow the start and end color, but it does not affect the color at all LOL
    /// </summary>
    public Color pathColor;

    /// <summary>
    /// Time after the animation ends
    /// </summary>
    [Range(0, 5)]
    public float maxAnimationTime = 4f;

    /// <summary>
    /// Width of drawn line of line-renderer
    /// </summary>
    [Range(0, 1)]
    public float animationWidth = 0.7f;

    /// <summary>
    /// Actually not the endTransform for animation. Maybe it indicates the current transform. Tbh I forgot what this is xD
    /// But you know never change a running system
    /// </summary>
    private Transform m_endTransform;

    /// <summary>
    /// End rotation for animation
    /// </summary>
    private Quaternion m_endRotation;
    
    /// <summary>
    /// End position for animation
    /// </summary>
    private Vector3 m_endPosition;

    /// <summary>
    /// obvious
    /// </summary>
    private bool m_isRunning = false;

    /// <summary>
    /// Distance to stop animation
    /// </summary>
    private float animationEndThreshhold = 0.001f;
    
    /// <summary>
    /// current time since animation is running
    /// </summary>
    private float timeAnimating = float.NegativeInfinity;

    /// <summary>
    /// distance to final position
    /// </summary>
    private float remainingDistance = float.MaxValue;

    /// <summary>
    /// Position where the object was before animation started
    /// </summary>
    private float startDistance = float.NaN;
    private int lineIndex = 0;
    private Quaternion m_startRotation;
    private Scripted_Interactable_Object objectToMove;

    private LineRenderer renderer = null;

    private AnimationAction animationAction;


    // Start is called before the first frame update
    void Start()
    {
    }

    public enum AnimationType
    {
        LINEAR,
        EASEIN,
        EASEOUT,
        EASEINOUT
    }

    /// <summary>
    /// Indicated which action was performed that started the animation
    /// </summary>
    public enum AnimationAction
    {
        SELECT,
        DETACH
    }

    /// <summary>
    /// Starts an animation of object
    /// </summary>
    /// <param name="objectToMove">The object that should be moved to a new position</param>
    /// <param name="action">Whether the animation is due to detach or select action</param>
    public void startAnimation(Scripted_Interactable_Object objectToMove, AnimationAction action = AnimationAction.SELECT)
    {
        if (!m_isRunning)
        {
            m_isRunning = true;
            this.objectToMove = objectToMove;
            objectToMove.getFinalTransform(action, out m_endPosition, out m_endRotation);
            m_endTransform.position = m_endPosition;
            m_endTransform.rotation = m_endRotation;
            startDistance = Vector3.Distance(gameObject.transform.position, m_endTransform.position);
            animationAction = action;
        }
    }

/// <summary>
/// Set up line renderer
/// </summary>
    private void setupRenderer()
    {
        renderer = new GameObject().AddComponent<LineRenderer>();
        renderer.material = drawingMaterial;
        m_startRotation = transform.rotation;
        renderer.startColor = renderer.endColor = pathColor;
        renderer.startWidth = renderer.endWidth = animationWidth;
    }


    // Update is called once per frame
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

                if (remainingDistance <= animationEndThreshhold)
                {
                    endAnimation();
                }
                else
                {
                    objectToMove.getFinalTransform(animationAction, out m_endPosition, out m_endRotation);
                    m_endTransform.position = m_endPosition;
                    m_endTransform.rotation = m_endRotation;
                    animate();
                }
            }
        }
    }

    /// <summary>
    /// Ends the animation nothing more to say
    /// </summary>
    private void endAnimation()
    {
        m_isRunning = false;
        timeAnimating = float.NegativeInfinity;
        remainingDistance = float.MaxValue;
        m_startRotation = Quaternion.identity;
        if (renderer.gameObject != null)
        {
            Destroy(renderer.gameObject);
        }
        if (OnAnimationEnd != null)
        {
            OnAnimationEnd.Invoke();
        }
    }

    private void animate()
    {
        if (timeAnimating == float.NegativeInfinity)
        {
            timeAnimating = 0;
        }
        timeAnimating += Time.deltaTime;
        //calculate forward vector
        Vector3 forward = m_endTransform.position - transform.position;
        Quaternion rotationTowardsEnd = Quaternion.Inverse(transform.rotation) * m_endTransform.rotation;
        remainingDistance = Vector3.Magnitude(forward);

        Vector3 forwardNorm = Vector3.Normalize(forward);

        double progress = getTimeProgress();

        float animationPosDif = (float)progress - (1 - remainingDistance / startDistance);
        gameObject.transform.rotation = Quaternion.Slerp(m_startRotation, m_endTransform.rotation, (float)progress);
        gameObject.transform.position += forwardNorm * animationPosDif;
        drawNextPoint();
    }

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

    private void drawNextPoint()
    {
        renderer.positionCount = lineIndex + 1;
        renderer.SetPosition(lineIndex, gameObject.transform.position);
        lineIndex++;
    }
}



