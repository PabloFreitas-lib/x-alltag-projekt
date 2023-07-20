

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoInfoGamerHubAssets
{


/// <summary>
///   This script is responsible for changing the color of a game object's material
/// </summary>
/// <author> Maya, (Buesra) </author>
    public class DemoColorPicker : MonoBehaviour
    {
        /// <summary>
        /// This method sets the color of the game object's material to the specified newColor.
        /// </summary>
        /// <author> Maya </author>
        /// <param name="color"> The new color value. </param>
        public void SetColor(Color newColor)
        {
            // Retrieve the MeshRenderer component attached to the game object and change its material color to newColor
            GetComponent<MeshRenderer>().material.color = newColor;
            
            // Log the new color to the Unity console for debugging purposes
            Debug.Log(newColor);
        }


    }
}
