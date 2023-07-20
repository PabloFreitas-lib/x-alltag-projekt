using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// eases pulling closer objects
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>

public class HandVelocity : XRRayInteractor
{
    //
    public Transform target; // Das Zielobjekt
    public float normalizationRange = 1f; // Der Bereich zur Normalisierung der Geschwindigkeit
    public float deadZone = 0.2f;
    public float sensitivity = 10;

    public float pullDistance = 25;
    public float pushDistance = 60;

    private Rigidbody rb;
    private Vector3 previousPosition;
    public float normalizedSpeed;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector3 velocity = (currentPosition - previousPosition) / Time.fixedDeltaTime;
        previousPosition = currentPosition;

        Vector3 direction = (target.position - transform.position).normalized;
        //Debug.Log(direction);
        //Debug.Log(target.position - transform.position);
        float distance = Vector3.Distance(transform.position, target.position);
        distance *= 100;
        float speed = Vector3.Dot(velocity, direction);
        //Debug.Log(speed);
        normalizedSpeed = -Mathf.Clamp(speed * sensitivity / normalizationRange, -1f, 1f);
        if(normalizedSpeed <= deadZone && normalizedSpeed > -deadZone)
        {
            normalizedSpeed = 0;
        }
        if(distance < pullDistance)
        {
            normalizedSpeed = -1;
        }
        else if(distance > pushDistance)
        {
            normalizedSpeed = 1;
        }
        TranslateAnchor(rayOriginTransform, attachTransform, normalizedSpeed);
    }

    private void Update()
    {
        //Debug.Log(normalizedSpeed);

    }

}
