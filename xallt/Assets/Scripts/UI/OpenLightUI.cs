using UnityEngine;


/// <summary>
/// This script allows toggling the visibility of a Canvas UI element 
/// </summary>
/// <author> Maya , (Buesra)</author> 
public class OpenLightUI : MonoBehaviour
{
    // Reference to the Canvas component
    public Canvas canvas;
    
    // Flag to track whether the canvas is enabled or disabled
    private bool isCanvasEnabled;


    /// <summary>
    /// Sets the Canvas GameObject to be disabled when the game starts.
    /// </summary>
    /// <author> Maya </author>
    private void Start()
    {
        canvas.gameObject.SetActive(false);
        //canvas.enabled = isCanvasEnabled;
    }
    
    
    /// <summary>
    /// This method is automatically called when the mouse button is pressed down.
    /// </summary>
    /// <author> Maya </author>
    private void OnMouseDown()
    {
        //isCanvasEnabled = !isCanvasEnabled;
        //canvas.enabled = isCanvasEnabled;
    }
    
    
    /// <summary>
    /// Toggles the active state of the GameObject to which this script is attached.
    /// If the GameObject is currently active, it will be deactivated, and vice versa.
    /// </summary>
    /// <author> Maya </author>
    public void UIsetActive()
    {
        gameObject.SetActive(!gameObject.active);
    }
}