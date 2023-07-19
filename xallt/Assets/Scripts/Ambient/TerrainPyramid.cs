using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPyramid : MonoBehaviour
{
    public string terrainName;
    public GameObject terrain;
    public bool isOpen;

    private void Start()
    {
        isOpen = false;
    }

    private void Update()
    {
        terrain.SetActive(isOpen);

    }
}
