using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuffer : MonoBehaviour
{
    [SerializeField]
    private List<Mesh> bufferedDrawings = new List<Mesh>();

    [SerializeField]
    private GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addBakedDrawing(Mesh drawing)
    {
        bufferedDrawings.Add(drawing);
        List<Vector3> vertices = new List<Vector3>();

        drawing.GetVertices(vertices);
        cube.transform.position = vertices[0];
    }
}
