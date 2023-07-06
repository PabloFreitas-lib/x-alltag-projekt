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

    private GameObject cube;

    private void Update()
    {
       Collider[] colliders = Physics.OverlapBox(detectionCollider.bounds.center, detectionCollider.bounds.extents, detectionCollider.transform.rotation);
       
        if (colliders.Length == 1)
        {
            SpawnCube(transform);
        }

        if (cube != null)
            if (cube.transform.childCount == 2 && cube.GetComponent<Rigidbody>().isKinematic)
            {
                cube.transform.parent = null;
                cube.GetComponent<Rigidbody>().isKinematic = false;
            }
    }

    private void Start()
    {
        SpawnCube(transform);
    }

    public void SpawnCube(Transform spawnPosition)
    {
        // Instantiate a cube prefab at the position of the hand controller
        GameObject cube = Instantiate(filePrefab, spawnPosition.position, Quaternion.identity);
        GameObject map = Instantiate(mindmap,mindMapSpawnPosition.position, Quaternion.identity);
        cube.GetComponent<File>().map = map.GetComponent<Mindmap>();
        isSpawned = true;
    }

    public void SpawnCubeUI(Transform spawnPosition)
    {
        // Instantiate a cube prefab at the position of the hand controller
        this.cube = Instantiate(filePrefab, spawnPosition.position, Quaternion.identity);
        GameObject map = Instantiate(mindmap, mindMapSpawnPosition.position, Quaternion.identity);
        cube.GetComponent<File>().map = map.GetComponent<Mindmap>();
        isSpawned = true;
        cube.GetComponent<Rigidbody>().isKinematic = true;
        cube.transform.parent = spawnPosition;
    }
}

