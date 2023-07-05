using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandVelocity : XRRayInteractor
{
    public Transform target; // Das Zielobjekt
    public float normalizationRange = 1f; // Der Bereich zur Normalisierung der Geschwindigkeit
    public float deadZone = 0.2f;
    public float sensitivity = 10;

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
        float speed = Vector3.Dot(velocity, direction);
        normalizedSpeed = -Mathf.Clamp(speed * sensitivity / normalizationRange, -1f, 1f);
        if(normalizedSpeed <= deadZone && normalizedSpeed > -deadZone)
        {
            normalizedSpeed = 0;
        }
        TranslateAnchor(rayOriginTransform, attachTransform, normalizedSpeed);
    }

    private void Update()
    {
        Debug.Log(normalizedSpeed);

    }

}
