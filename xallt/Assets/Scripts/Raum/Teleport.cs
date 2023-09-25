using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class is to teleport to the terrace.
/// </summary>
/// <author> Mert Kaynak </author>

public class Teleport : MonoBehaviour
{
    // GameObject, which is getting teleported.
    public GameObject gameObject;
    // EmptyObject to teleport to.
    public GameObject goalLocation;
    public GameObject startLocation;
    // The goal teleport position (Will be set automatically).
    private Vector3 teleportPosition;
    private bool teleported = false;

    // Start is called before the first frame update
    void Start()
    {
        teleportPosition = goalLocation.transform.position;
        

    }

    public void teleportToPosition()
    {
        
        if (teleported)
        {
            teleportPosition = startLocation.transform.position;
        }
        else
        {
            teleportPosition = goalLocation.transform.position;
        }
        teleported = !teleported;
        gameObject.transform.position = teleportPosition;
    }
}
