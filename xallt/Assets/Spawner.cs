using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] fruitPrefabs;
    public Transform[] spawnPoints;
    public float spawnRate;
    public float timer;
    public float spawnForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(timer <= 0)
       {
            SpawnRandomFruit();
            timer = spawnRate;
       }
       timer -= Time.deltaTime;
    }

    public void SpawnRandomFruit()
    {
        int randomFruit = Random.Range(0, 10);
        int randomSpawn = Random.Range(0, 4);
        float randomForceFactor = Random.Range(0.7f, 1.3f);
        int randomDirection = Random.Range(-30, 30);

        Transform spawn = spawnPoints[randomSpawn];
        Vector3 angles = spawn.transform.eulerAngles;
        angles.y += randomDirection;
        spawn.transform.eulerAngles = angles;
        GameObject fruit = Instantiate(fruitPrefabs[randomFruit], spawn.position, Quaternion.identity, null);
        fruit.GetComponent<Rigidbody>().AddForce(spawn.transform.up * spawnForce  * randomForceFactor);

        Vector3 angles1 = spawn.transform.eulerAngles;
        angles1.y -= randomDirection;
        spawn.transform.eulerAngles = angles1;
    }
}
