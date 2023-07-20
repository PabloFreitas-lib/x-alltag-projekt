using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class is responsible for creating cubes in the scene of the UI when a button is pressed.
/// </summary>
/// <author> Fiona, (Buesra) </author>

public class CreateCubes : MonoBehaviour
{
    // The prefab used to create cubes
    public GameObject cubePrefab;
    
    //The parent transform under which the cubes will be placed
    public Transform cubeParent;
    
    //The Variable for the Cubesize
    public Vector3 cubeScale = Vector3.one; 
    
    
    //Reference to the button component if the button has been pressed
    private Button button;
    private bool buttonPressed = false;

    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component attached to this object
        button = GetComponent<Button>();
        
        // Add a listener to the button's click event.
        button.onClick.AddListener(TriggerButtonPress);    
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed)
                {
                    CreateCube();
                    buttonPressed = false;
                }
    }
    

    /// <summary>
    /// Event handler for the button click event
    /// </summary>
    /// <author> Fiona </author>
    /// <param name="buttonPressed"> If Button pressed true </param>
    private void TriggerButtonPress()
        {
            buttonPressed = true;
        }

    
    /// <summary>
    /// Creates a new cube in the scene
    /// </summary>
    /// <author> Fiona </author>
    private void CreateCube()
        {
            //Instantiate a new cube using the prefab
            GameObject newCube = Instantiate(cubePrefab, cubeParent);
            
            // Set the scale of the cube
            newCube.transform.localScale = cubeScale; 
        }

}
