using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
        //private Vector3 startUpdateNormalPosition = Vector3.zero;
        public float moveSpeed = 1f;
        //public Vector3 KameraUp = Camera.main.transform.up;


    // Start is called before the first frame update
    void Start()
    {
        //
        //v = (transform.position - center.position);
        //startUpdateNormalPosition = GazeManager.Instance.Normal;


    }

    // Update is called once per frame
    void Update()
    {        
        //rotiert Text in Richtung Kamera
        transform.LookAt(Camera.main.transform);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);

        //transform.LookAt(Camera.main.transform.forward);

        //Versuch Text um Punkt zu rotieren:
        //transform.RotateAround(1,1,1);
        //(transform.position.x,transform.position.y-2,transform.position.z);
        //transform.position.y=transform.position.y-2;
        //transform.RotateAround(transform.position, startUpdateNormalPosition);
        //transform.Rotate(Vector3.right);

        //Text fliegt um mich rum (auch cool) (vllt spaeter fuer Avatare-Emojis nutzbar)
        //float moveAmount = moveSpeed * Time.deltaTime;
        //transform.Translate(moveAmount, 0f, 0f);

        //dreht Text um (sonst immer spiegelverkehrt)
        transform.rotation *= Quaternion.Euler(0f, 180f, 0f);


    }
}
