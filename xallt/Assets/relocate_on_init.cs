using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class relocate_on_init : MonoBehaviour
{
    public InputActionProperty leftController;
    public Transform leftAnchor;
    public Transform xrOrigin; //to set x,y pos
    public GameObject mainCamera; //to set rotation
    public GameObject cameraOffset; //to set z offset
    private float offset = 0.7f; //hardcoded!!!!!!
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            {
            Debug.Log("controller at:" + leftController.action.ReadValue<Vector3>());
            Debug.Log("HMD at:" + xrOrigin.position);
            Vector3 distanceLeftHMD = xrOrigin.position - leftController.action.ReadValue<Vector3>();
            Debug.Log("Distance:" + distanceLeftHMD);
            xrOrigin.position = new Vector3(leftAnchor.position.x + distanceLeftHMD.x, xrOrigin.position.y, leftAnchor.position.z + distanceLeftHMD.z + offset);
        }
    }
}
