using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CubeSpawner : MonoBehaviour
{
    // Reference to the Trigger input action
    public GameObject filePrefab; // Prefab of the cube to spawn
    public bool isSpawned;
    Collider detectionCollider;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(detectionCollider.bounds.center, detectionCollider.bounds.extents, detectionCollider.transform.rotation);

        if (colliders.Length == 0)
        {
            SpawnCube();
        }
    }

    private void Start()
    {
        SpawnCube();
    }

    private void SpawnCube()
    {
        // Instantiate a cube prefab at the position of the hand controller
        Instantiate(filePrefab, transform.position, Quaternion.identity);
        isSpawned = true;
    }
}

