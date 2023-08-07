using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to buffer all drawings of interactable pen in form single gameobjects containing a Mesh, MeshRenderer and MeshFilter 
/// </summary>
/// <author>Fabian Schmurr</author>
public class MeshBuffer : MonoBehaviour
{
    /// <summary>
    /// List that stores the single parts of drawing
    /// </summary>
    private List<GameObject> meshes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method that swappes all given normals of given mesh component
    /// </summary>
    /// <param name="drawing">Mesh of which the normals get swapped</param>
    /// <returns>Array containing the new swapped normals</returns>
    /// <author>Fabian Schmurr</author>
    private Vector3[] swapNormals(Mesh drawing)
    {
        Vector3[] swappedNormals = drawing.normals;
        for (int i = 0; i < swappedNormals.Length - 1; i++)
        {
            swappedNormals[i] = -swappedNormals[i];
            
        }
        return swappedNormals;
      
    }

    /// <summary>
    /// Adds a given mesh to the buffer-list. Therefore the mesh is added to a gameobject 
    /// </summary>
    /// <param name="mesh">Mesh to buffer</param>
    /// <param name="material">Material of drawing used in LineRenderer</param>
    /// <author>Fabian Schmurr</author>
    private void addMeshToBuffer(Mesh mesh, Material material)
    {
        //create new GameObject with MeshRenderer
        GameObject meshHolder = new GameObject();
        MeshRenderer renderer = meshHolder.AddComponent<MeshRenderer>();
        MeshFilter filter = meshHolder.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        renderer.material = material;
        meshes.Add(meshHolder);
    }

    /// <summary>
    /// Copies a given mesh
    /// </summary>
    /// <param name="mesh">Mesh component to copy</param>
    /// <returns>The copied mesh</returns>
    /// <author>Fabian Schmurr</author>
    private Mesh copyMesh(Mesh mesh)
    {
        Mesh newMesh = new Mesh();
        newMesh.vertices = mesh.vertices;
        newMesh.triangles = mesh.triangles;
        newMesh.uv = mesh.uv;
        newMesh.normals = mesh.normals;
        newMesh.colors = mesh.colors;
        newMesh.tangents = mesh.tangents;
        return newMesh;
    }

    /// <summary>
    /// Adds a mesh to the mesh buffer
    /// </summary>
    /// <param name="drawing">The drawing to buffer</param>
    /// <param name="material">Material of drawing, used in LineRenderer component</param>
    /// <author>Fabian Schmurr</author>
    public void addBakedDrawing(Mesh drawing, Material material)
    {
        //add front side mesh
        addMeshToBuffer(drawing, material);
        Vector3[] swappedNormals = swapNormals(drawing);
        Mesh newMesh = copyMesh(drawing);
        newMesh.normals = swappedNormals;
        addMeshToBuffer(newMesh, material);
    }
}
