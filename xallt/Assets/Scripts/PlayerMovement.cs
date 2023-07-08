using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public new Transform camera;
    public Vector3 offset;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = camera.transform.position + offset;
    }
}
