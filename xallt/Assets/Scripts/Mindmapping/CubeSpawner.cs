using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CubeSpawner : MonoBehaviour
{
    public Transform rightHand;
    public InputActionReference pinchAction; // Reference to the Trigger input action
    public GameObject cubePrefab; // Prefab of the cube to spawn

    private bool isTriggerPressed = false;

    private void Start()
    {
        // Enable the trigger action
        pinchAction.action.Enable();
    }

    private void Update()
    {
        // Check if the trigger button is pressed
        if (pinchAction.action.triggered && !isTriggerPressed)
        {
            isTriggerPressed = true;
            SpawnCube();
        }
        else if (!pinchAction.action.triggered && isTriggerPressed)
        {
            isTriggerPressed = false;
        }
    }

    private void SpawnCube()
    {
        // Instantiate a cube prefab at the position of the hand controller
        GameObject cube = Instantiate(cubePrefab, rightHand.position, Quaternion.identity);
        // Parent the cube to the hand controller
        cube.transform.SetParent(transform);
    }

    private void OnDisable()
    {
        // Disable the trigger action
        pinchAction.action.Disable();
    }
}

