using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper-Class for serialization of general software-state (CUP) in JSON.
/// </summary>
/// <author> Jakob Kern</author>    
[System.Serializable]
public class ComplexUserPrefsWrapper
{
    //Central Lighting values
    public Color lightColor;
    public float lightIntensity;

    //Files and Whiteboards existent in the scene
    public FileWrapper[] files;
    public string[] whiteboards;


    /// <summary>
    ///    Called by SaveComplexUserPrefs() to easily encapsulate necessary game state data.
    /// </summary>
    /// <author> Jakob Kern </author>
    public ComplexUserPrefsWrapper()
    {
        LightController lights = GameObject.Find("Lights").GetComponent<LightController>();
        lightColor = lights.color;
        lightIntensity = lights.brightness;

        //Add Tags to acutal Files
        int f = 0;
        File[] filesToEncode = Resources.FindObjectsOfTypeAll<File>();
        files = new FileWrapper[filesToEncode.Length];

        foreach (File file in filesToEncode)
        {
            files[f] = new FileWrapper(file);
            f++;
        }

        GameObject markerObject = GameObject.Find("Pen_Interactable");
        WhiteboardMarker marker = markerObject.GetComponent<WhiteboardMarker>();
        whiteboards = marker.paths.ToArray();
    }
}