using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

/// <summary>
/// Spawns cubes/fíles.
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>
public class CubeSpawner : MonoBehaviour
{
    //prefab, collider, position
    public GameObject filePrefab; // Prefab of the cube to spawn
    public bool isSpawned;
    public Collider detectionCollider;
    public GameObject mindmap;
    public Transform mindMapSpawnPosition;

    private GameObject cube;
    bool moved = true;

    private void Update()
    {
       Collider[] colliders = Physics.OverlapBox(detectionCollider.bounds.center, detectionCollider.bounds.extents, detectionCollider.transform.rotation);
       
        if (colliders.Length == 1)
        {
            SpawnCube(transform);
        }

        if (cube != null)
            if (cube.transform.childCount >= 2 && cube.GetComponent<Rigidbody>().isKinematic)
            {
                cube.transform.parent = null;
                cube.GetComponent<Rigidbody>().isKinematic = false;
                moved = true;
            }
    }

    private void Start()
    {
        SpawnCube(transform);
    }

    /// <summary>
    /// Spawns cube on socket.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    /// <param name="Transform spawn Position"> requires spawn position </param>
    public void SpawnCube(Transform spawnPosition)
    {
        // Instantiate a cube prefab at the position of the hand controller
        GameObject cube = Instantiate(filePrefab, spawnPosition.position, Quaternion.identity);
        GameObject map = Instantiate(mindmap,mindMapSpawnPosition.position, Quaternion.identity);
        cube.GetComponent<File>().map = map.GetComponent<Mindmap>();
        isSpawned = true;
    }

    /// <summary>
    /// Spawns cube in hand UI.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    /// <param name="Transform spawn Position"> requires spawn position </param>
    public void SpawnCubeUI(Transform spawnPosition)
    {
        if (moved == true)
        {
            // Instantiate a cube prefab at the position of the hand controller
            this.cube = Instantiate(filePrefab, spawnPosition.position, Quaternion.identity);
            GameObject map = Instantiate(mindmap, mindMapSpawnPosition.position, Quaternion.identity);
            cube.GetComponent<File>().map = map.GetComponent<Mindmap>();
            isSpawned = true;
            cube.GetComponent<Rigidbody>().isKinematic = true;
            cube.transform.parent = spawnPosition;
            moved = false;
        }
        
    }
}

