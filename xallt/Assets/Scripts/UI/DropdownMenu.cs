using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script sets up a dropdown menu with two options ("Lampe 1" and "Lampe 2") and attaches it to a GameObject in a Unity scene
/// </summary>
/// <author> Maya, (Buesra) </author>

public class DropdownMenu : MonoBehaviour
{
    //Create a List of new Dropdown options
    List<string> m_DropOptions = new List<string> { "Lampe 1", "Lampe 2" };
    
    //This is the Dropdown
    Dropdown m_Dropdown;

    void Start()
    {
        //Fetch the Dropdown GameObject the script is attached to
        m_Dropdown = GetComponent<Dropdown>();
        
        //Clear the old options of the Dropdown menu
        m_Dropdown.ClearOptions();
        
        //Add the options created in the List above
        m_Dropdown.AddOptions(m_DropOptions);
    }
}