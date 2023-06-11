using UnityEngine;

public class move : MonoBehaviour
{
    private float speed = 2.0f;
    public GameObject character;
    public Transform cameraTransform; // Reference to the camera's transform

    void Update()
    {
        // Move the character based on WASD controls
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += cameraTransform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= cameraTransform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += cameraTransform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= cameraTransform.forward * speed * Time.deltaTime;
        }

        // Rotate the character to match the camera's rotation around the y-axis
        transform.rotation = Quaternion.Euler(new Vector3(0f, cameraTransform.eulerAngles.y, 0f));
    }
}
