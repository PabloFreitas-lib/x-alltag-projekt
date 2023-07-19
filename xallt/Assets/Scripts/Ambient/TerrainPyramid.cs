using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a terrain pyramid and controls its visibility based on the 'isOpen' flag.
/// </summary>
/// <author> Christoph Dreier, Mert Kaynak </author>
public class TerrainPyramid : MonoBehaviour
{
    public string terrainName; // The name of the terrain pyramid
    public GameObject terrain; // Reference to the terrain pyramid GameObject
    public bool isOpen; // Flag to indicate if the terrain pyramid is open

    private void Start()
    {
        isOpen = false; // Initialize the terrain pyramid as closed
    }

    private void Update()
    {
        terrain.SetActive(isOpen); // Set the visibility of the terrain pyramid based on the 'isOpen' flag
    }
}
