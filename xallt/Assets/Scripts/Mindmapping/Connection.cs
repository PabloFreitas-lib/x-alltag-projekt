using UnityEngine;

/// <summary>
/// Connection lines between nodes and between child node and parent (creation and destruction)
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>

public class Connection : MonoBehaviour
{
    //Nodes
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

    /// <summary>
    /// moves line connection with nodes
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
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

    /// <summary>
    /// creates line connection
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
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

    /// <summary>
    /// setter for from and to nodes
    /// </summary>
    public void SetFromTo(Node from, Node to)
    {
        this.from = from;
        this.to = to;
    }

    /// <summary>
    /// destroys line and parent-child-relation
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
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

    /// <summary>
    /// beginning for destroy connection function called by scissors
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    /// <param name="Collider other"> needs collision as trigger </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scissors"))
        {
            DestroyConnection();
        }
    }
}
