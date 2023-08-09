using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Loads and manages terrain pyramids in the scene based on trigger events.
/// </summary>
/// <author> Christoph Dreier, Mert Kaynak </author>
public class TerrainLoader : MonoBehaviour
{
    public TerrainPyramid terrain; // Reference to the current TerrainPyramid
    public string terrainName; // The name of the terrain

    // Start is called before the first frame update
    void Start()
    {
        // Code for initialization, if needed.
    }

    // Update is called once per frame
    void Update()
    {
        // Code for any continuous updates, if needed.
    }

    /// <summary>
    /// Called when the collider enters a trigger zone.
    /// If the collider is a pyramid, the terrain is set to the entered pyramid, and it's marked as open.
    /// </summary>
    /// <author> Christoph Dreier, Mert Kaynak </author>
    /// <param name="other"> The collider of the object that entered the trigger zone. </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pyramid"))
        {
            terrain = other.GetComponent<TerrainPyramid>();
            Debug.Log("test");
            terrain.isOpen = true; // Mark the terrain pyramid as open

            FindObjectOfType<AudioManager>().Play(terrainName); //play audio correlating to terrain
            //map = terrain.map; // Set map to the terrain pyramid's map (if needed).
        }
    }

    /// <summary>
    /// Called when the collider exits a trigger zone.
    /// If the collider is a pyramid, the terrain is set to the exited pyramid, and it's marked as closed.
    /// </summary>
    /// <author> Christoph Dreier, Mert Kaynak </author>
    /// <param name="other"> The collider of the object that exited the trigger zone. </param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pyramid"))
        {
            terrain = other.GetComponent<TerrainPyramid>();
            terrain.isOpen = false; // Mark the terrain pyramid as closed

            FindObjectOfType<AudioManager>().Stop(terrainName); //stop audio correlating to terrain
            //map = null; // Unset map (if needed).
        }
    }
}
