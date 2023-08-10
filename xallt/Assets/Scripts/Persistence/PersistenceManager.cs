using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


/// <summary>
/// Manager Class for all persistence functions. Class which is used as interface to call function implemented in savesystem.
/// Extra Class afforded, because you cant use function with parameters  in UI buttons
/// Because persistence functions are static you can use them directly without creating an instance of savesystem.
/// </summary>
/// <author> Noah Horn </author>
public class PersistenceManager : MonoBehaviour
{
    /// <summary>
    ///  calls SaveWhiteboard function. Uses currently used whiteboard from Whiteboardmarker class as parameter
    ///  
    /// </summary>
    /// <author> Noah Horn </author>
    public void saveWhiteboardUI()
    {
        LightController lights = GameObject.Find("Lights").GetComponent<LightController>();
        WhiteboardMarker whiteboardMarkerInstance = GameObject.Find("Marker").GetComponent<WhiteboardMarker>();

        if (whiteboardMarkerInstance != null)
            {
                FindObjectOfType<SaveSystem>().SaveWhiteboard(whiteboardMarkerInstance.get_Whiteboard());
        }
        else
        {
            Debug.Log("WhiteboardMarker nicht initialisiert");
        }           
    }
}
