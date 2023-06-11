using UnityEngine;
using System.Collections.Generic;

public class RayToParent : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public GameObject parentNode; 
    public Material rayMaterial;


    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        lineRenderer.material = rayMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, parentNode.transform.position);
        
    }
}