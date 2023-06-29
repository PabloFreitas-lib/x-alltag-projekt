using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Metadata
    public int id;
    public DateTime creationDate;
    public Mindmap mindmap;

    //Display information
    public string text;
    //Lists or variables for each type of appendable data

    //Model
    public GameObject parent;                       //empty parent -> treat node as root
    public List<GameObject> children;

    //LineRenderer - Ray to Parent
    [Header("RayToParent")]
    public LineRenderer lineRenderer;
    public Material rayMaterial;

    //Boolean
    public bool isRoot;


    private void Start()
    {
        CreateLineRenderer();
    }
    private void Update()
    {
        UpdateLineRenderer();

    }

    private void UpdateLineRenderer()
    {
        if (parent != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, parent.transform.position);

        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void CreateLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        lineRenderer.material = rayMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
    }
    public void SelectSelf()
    {
        if(mindmap != null)
            mindmap.SelectNode(this);
        
    }

    public void ConnectNodes()
    {
        if(mindmap.mode == Mindmap.Mode.ConnectMode && mindmap.prevSelected != null && mindmap.selected != null)
        {
            GameObject connection = Instantiate(mindmap.connectionPrefab, transform.position, Quaternion.identity);
            connection.GetComponent<Connection>().SetFromTo(mindmap.prevSelected, mindmap.selected);
            connection.transform.parent = mindmap.transform;
            mindmap.mode = Mindmap.Mode.defaultMode;
        }
    }
}
