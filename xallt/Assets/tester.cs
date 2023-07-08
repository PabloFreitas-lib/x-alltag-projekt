using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{

    [SerializeField]
    Scissor_interaction scissors;
    // Start is called before the first frame update
    void Start()
    {
        if (scissors != null)
        {
            scissors.OnScissorsCut += onCut;
        }
    }

    private void onCut()
    {
        Debug.Log("Cut");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
