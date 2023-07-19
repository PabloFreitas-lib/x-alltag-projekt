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
        /// <param name="Camera.main.transform"> needs position of the camera </param>
        public transform.LookAt(Camera.main.transform);

        /// <summary>
        /// disables rotation in x and z direction (otherwise text will appear stretched)
        /// </summary>
        /// <author> Dmitry </author>
        /// <param name="0"> sets x-axis rotation to zero </param>
        /// <param name="transform.rotation.y"> allows y-axis rotation </param>
        /// <param name="0"> sets z-axis rotation to zero </param>
        /// <param name="transform.rotation.w"> allows w-axis rotation </param>
        public transform.rotation = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);

        /// <summary>
        /// mirrors text (or text will be inversed)
        /// </summary>
        /// <author> Mailin </author>
        /// <param name="0f"> keeps original x-orientation </param>
        /// <param name="180f"> flips y-orientation </param>
        /// <param name="0f"> keeps original z-orientation </param>

        public transform.rotation *= Quaternion.Euler(0f, 180f, 0f);


    }
}
