using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// rotates text in viewing direction (of the camera), makes text readable from every position
/// </summary>
/// <author> Mailin </author>


public class Rotation : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {        
        /// <summary>
        /// rotates text in viewing direction (of the camera)
        /// </summary>
        /// <author> Mailin </author>
        transform.LookAt(Camera.main.transform);

        /// <summary>
        /// disables rotation in x and z direction (otherwise text will appear stretched)
        /// </summary>
        /// <author> Dmitry </author>
        transform.rotation = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);

        /// <summary>
        /// mirrors text (or text will be inversed)
        /// </summary>
        /// <author> Mailin </author>
        transform.rotation *= Quaternion.Euler(0f, 180f, 0f);


    }
}
