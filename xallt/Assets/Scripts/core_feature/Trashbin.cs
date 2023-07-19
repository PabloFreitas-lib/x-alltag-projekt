using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is used to describe the behavior of the trashbin.
/// </summary>
/// <author> Authors </author>
public class Trashbin : MonoBehaviour
{
    /// <summary>
    /// This function is called when an object enters the trigger of the trashbin.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TrashObject>() != null)
        {
            Debug.Log("Object destroyed");
            Destroy(other.gameObject);
        }
    }
    /// <summary>
    /// This is function is called when an object is initialized.
    /// </summary>
    /// <author> Authors </author>
    void Start()
    {
        
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    /// <author> Authors </author>
    void Update()
    {
        
    }
}
