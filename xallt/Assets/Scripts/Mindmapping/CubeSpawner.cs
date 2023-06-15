using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CubeSpawner : MonoBehaviour
{
    public GameObject filePrefab; // Prefab of the cube to spawn
    public bool isSpawned;
    public Collider detectionCollider;
    public GameObject mindmap;
    public Transform mindMapSpawnPosition;

    private void Update()
    {
       Collider[] colliders = Physics.OverlapBox(detectionCollider.bounds.center, detectionCollider.bounds.extents, detectionCollider.transform.rotation);
       
        if (colliders.Length == 1)
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
        GameObject cube = Instantiate(filePrefab, transform.position, Quaternion.identity);
        Instantiate(mindmap,mindMapSpawnPosition.position, Quaternion.identity);
        cube.GetComponent<File>().map = mindmap.GetComponent<Mindmap>();
        isSpawned = true;
    }
}

