using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper-Class for serialization of fileCubes in JSON. Part of CUP.
/// </summary>
/// <author> Jakob Kern </author> 
[System.Serializable]
public class FileWrapper
{
    public string name;
    public Color color;
    public Vector3 position;

    /// <summary>
    /// Called by CUP Constructor to get a serializable fileCube representation.
    /// </summary>
    /// <author> Autoren </author>
    /// <param name="file"> The File to be serialized. </param>
    public FileWrapper(File file)
    {
        name = file.name;
        color = file.GetComponent<ColorChanger>().objectColor; // Access ColorChanger Script of file
        position = file.gameObject.transform.position;
    }
}