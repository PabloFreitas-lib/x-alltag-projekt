using UnityEngine;

public class Connection : MonoBehaviour
{
    public Node from;
    public Node to;

    //LineRenderer - Ray to Parent
    [Header("Ray")]
    public LineRenderer lineRenderer;
    public Material rayMaterial;
    // Start is called before the first frame update
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
        if (from != null && to != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, from.transform.position);
            lineRenderer.SetPosition(1, to.transform.position);

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

    public void SetFromTo(Node from, Node to)
    {
        this.from = from;
        this.to = to;
    }

    public void DestroyConnection()
    {
        if(from.mindmap.mode == Mindmap.Mode.DeleteMode)
        {
            if (from.parent.GetComponent<Node>() != null)
            {
                to.parent = null;
                from.parent.children.Remove(to);
                from.mindmap.mode = Mindmap.Mode.defaultMode;
                Destroy(this);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scissors"))
        {
            DestroyConnection();
        }
    }
}
