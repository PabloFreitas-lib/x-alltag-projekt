using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainManager : MonoBehaviour
{
    // Terrains and Inspector bool
    public Terrain mountainTerrain;
    public bool activateMountain;

    public Terrain forestTerrain;
    public bool activateForest;

    public Terrain desertTerrain;
    public bool activateDesert;

    private void Start()
    {
        // Disable all terrains initially
        mountainTerrain.gameObject.SetActive(false);
        forestTerrain.gameObject.SetActive(false);
        desertTerrain.gameObject.SetActive(false);

    }

    private void Update()
    {
        // Check the boolean values and activate the corresponding terrains
        // unchecks instantly and one is always active
        if (activateMountain)
        {
            mountainTerrain.gameObject.SetActive(true);
            activateMountain = false;

            activateForest = false; 
            forestTerrain.gameObject.SetActive(false);

            activateDesert = false; 
            desertTerrain.gameObject.SetActive(false);
        }
        if (activateForest)
        {
            forestTerrain.gameObject.SetActive(true);
            activateForest = false;

            activateMountain = false; 
            mountainTerrain.gameObject.SetActive(false);

            activateDesert = false; 
            desertTerrain.gameObject.SetActive(false);
        }
        if (activateDesert)
        {
            desertTerrain.gameObject.SetActive(true);
            activateDesert = false;

            activateForest = false; 
            forestTerrain.gameObject.SetActive(false);

            activateMountain = false; 
            mountainTerrain.gameObject.SetActive(false);
        }
    }
}