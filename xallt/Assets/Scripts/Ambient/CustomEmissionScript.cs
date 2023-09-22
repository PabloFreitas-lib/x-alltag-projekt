using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEmissionScript : MonoBehaviour
{
    // Reference to the material
    public Material targetMaterial; // Assign the material in the Inspector

    private void Start()
    {
        // Set the initial emission color (optional)
        SetEmissionColor(Color.black);
    }

    // Method to set the emission color
    public void SetEmissionColor(Color color)
    {
        if (targetMaterial == null)
        {
            Debug.LogWarning("No target material assigned.");
            return;
        }

        // Set the emission color property
        targetMaterial.SetColor("_EmissionColor", color);

        // Enable emission to make it visible
        targetMaterial.EnableKeyword("_EMISSION");
    }
}