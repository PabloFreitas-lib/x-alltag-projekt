using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float cameraDistance = 0f; // Distance between the camera and the player
    public float cameraHeight = 0f; // Height of the camera above the player
    public float cameraSensitivity = 5f; // Sensitivity of the camera rotation

    private float yaw = 0f; // Rotation around the y-axis
    private float pitch = 0f; // Rotation around the x-axis

    void LateUpdate()
    {
        // Move the camera to the player's position plus an offset
        transform.position = playerTransform.position + new Vector3(0f, cameraHeight, -cameraDistance);

        // Calculate the rotation based on mouse movement
        yaw += cameraSensitivity * Input.GetAxis("Mouse X");
        pitch -= cameraSensitivity * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Limit the pitch rotation to prevent camera flipping

        // Apply the rotation to the camera
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // Rotate the player to match the camera's y-axis rotation
        playerTransform.eulerAngles = new Vector3(0f, yaw, 0f);
    }
}
