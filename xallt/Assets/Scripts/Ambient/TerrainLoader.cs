using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainLoader : MonoBehaviour
{
    public TerrainPyramid terrain;
    public string terrainName;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pyramid"))
        {
            terrain = other.GetComponent<TerrainPyramid>();
            Debug.Log("test");
            terrain.isOpen = true;
            //map = terrain.map;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pyramid"))
        {
            terrain = other.GetComponent<TerrainPyramid>();
            terrain.isOpen = false;
            //map = null;
        }
    }
}