using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class fruit : MonoBehaviour
{
    float randomRotationX;
    float randomRotationY;
    float randomRotationZ;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Destroy), 5f);
        randomRotationX = Random.Range(-5f, 5f);
        randomRotationY = Random.Range(-5f, 5f);
        randomRotationZ = Random.Range(-5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 4);
        transform.eulerAngles += new Vector3(0, randomRotationY, randomRotationZ);
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
