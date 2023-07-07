using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUIHider : MonoBehaviour
{

    public Transform cameraTransform;
    public GameObject gobject;
    public GameObject ui;
    
    // Update is called once per frame
    void Update()
    {
        {
            // Berechne den Vektor zwischen dem Objekt und der Kamera
            Vector3 objectToCamera = cameraTransform.position - gobject.transform.position;

            // Normalisiere den Richtungsvektor des Objekts
            Vector3 objectForward = gobject.transform.forward.normalized;

            // Normalisiere den Richtungsvektor von der Kamera zum Objekt
            Vector3 cameraForward = objectToCamera.normalized;

            // Berechne das Dot-Produkt der beiden normalisierten Vektoren
            float dotProduct = Vector3.Dot(objectForward, cameraForward);

            Debug.Log(objectToCamera);
            Debug.Log(objectForward);
            Debug.Log(cameraForward);

            // Überprüfe, ob das Objekt zur Kamera zeigt (Dot-Produkt nahe bei 1.0)
            if (dotProduct > 0.1f)
            {
                ui.SetActive(true);
                // Das Objekt zeigt zur Kamera
                Debug.Log("Das Objekt zeigt zur Kamera.");
            }
            else
            {
                ui.SetActive(false);
                // Das Objekt zeigt nicht zur Kamera
                Debug.Log("Das Objekt zeigt nicht zur Kamera.");
            }
        }


    }
}
