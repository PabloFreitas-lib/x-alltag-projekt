using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script provides functionality for object selection using a dropdown menu.
/// It allows the user to select an object from the dropdown
/// </summary>
/// <author> Maya, (Buesra) </author>

public class ObjectSelector : MonoBehaviour
{
    // Dropdown UI component for object selection
    public Dropdown dropdown;
    
    // Array of objects to select from
    public GameObject[] objectsToSelect;

    
    /// <summary>
    /// Called when the script is initialized or enabled.
    /// </summary>
    /// <author> Maya </author>
    private void Start()
    {
    
    /// Add a listener to the dropdown's onValueChanged event to call the OnDropdownValueChanged method whenever the selected value changes.
    dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    
    }

    /// <summary>
    /// Called when the selected value of the dropdown changes.
    /// </summary>
    /// <param name="index">The index of the selected item in the dropdown.</param>
    /// <author> Maya </author>
    private void OnDropdownValueChanged(int index)
    {
        if (index >= 0 && index < objectsToSelect.Length)
        {
            GameObject selectedObject = objectsToSelect[index];
            
            // Handle the selected object by instantiating it in the scene.
            Instantiate(selectedObject);
        }
    }
}