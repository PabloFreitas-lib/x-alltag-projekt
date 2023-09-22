using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Katana : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fruit"))
        {
            Debug.Log(collider.gameObject + "SLICED");
            Slice(collider.gameObject);
            Destroy(collider.gameObject);
        }
    }
    public void Slice(GameObject target)
    {
        SlicedHull hull = target.Slice(transform.position, transform.up);
        if(hull != null)
        {
            Material mat = target.GetComponent<MeshRenderer>().material;
            GameObject upperHull = hull.CreateUpperHull(target, mat);
            upperHull.AddComponent<Rigidbody>();
            upperHull.AddComponent<fruit>();
            GameObject lowerHull = hull.CreateLowerHull(target, mat);
            lowerHull.AddComponent<Rigidbody>();
            lowerHull.AddComponent<fruit>();

        }
    }
}