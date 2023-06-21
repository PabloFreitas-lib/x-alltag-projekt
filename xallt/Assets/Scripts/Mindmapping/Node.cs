using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Metadata
    private static uint id;
    private static DateTime creationDate;

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
}
