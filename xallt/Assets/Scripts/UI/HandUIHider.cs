using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the main class that contains the logic for hiding or showing the UI based on the object's orientation with respect to the camera.
/// </summary>
/// <author> Maya, Dmitry, ? , (Buesra) </author>

public class HandUIHider : MonoBehaviour
{

    //Reference to the transform component of the camera
    public Transform cameraTransform;
    
    //Reference to the game object to be checked
    public GameObject gobject;
    
    //Reference to the UI game object
    public GameObject ui;
    
    // Update is called 
    void Update()
    {
        {
            
            // Calculate the vector between the object and the camera
            Vector3 objectToCamera = cameraTransform.position - gobject.transform.position;

            // Normalize the forward direction vector of the object
            Vector3 objectForward = gobject.transform.forward.normalized;

            // Normalize the forward direction vector from the camera to the object
            Vector3 cameraForward = objectToCamera.normalized;

            // Calculate the dot product of the two normalized vectors
            float dotProduct = Vector3.Dot(objectForward, cameraForward);

            Debug.Log(objectToCamera);
            Debug.Log(objectForward);
            Debug.Log(cameraForward);

            // Check if the object is facing the camera (dot product close to 1.0)
            if (dotProduct > 0.1f)
            {
                ui.SetActive(true);
                
                // The object is facing the camera
                Debug.Log("Das Objekt zeigt zur Kamera.");
            }
            else
            {
                ui.SetActive(false);
                
                // The object is not facing the camera
                Debug.Log("Das Objekt zeigt nicht zur Kamera.");
            }
        }


    }
}
