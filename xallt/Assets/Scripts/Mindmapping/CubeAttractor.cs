using UnityEngine;

/// <summary>
/// makes cubes magnetic to file socket
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>

public class CubeAttractor : MonoBehaviour
{
    //force, radius, tag, color
    public float attractForce = 10f;
    public float attractRadius = 5f;
    public string objectTag;
    public Color gizmoColor;

    /// <summary>
    /// makes cube magnetic to file socket
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(objectTag))
            {
                Rigidbody cubeRigidbody = collider.GetComponent<Rigidbody>();
                if (cubeRigidbody != null)
                {
                    Vector3 direction = transform.position - cubeRigidbody.position;
                    float distance = direction.magnitude;
                    float attractStrength = 1f - Mathf.Clamp01(distance / attractRadius);
                    Vector3 force = direction.normalized * attractForce * attractStrength;

                    cubeRigidbody.AddForce(force);
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, attractRadius);
    }
}

