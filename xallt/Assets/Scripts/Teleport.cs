using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    // GameObject, which is getting teleported.
    public GameObject gameObject;
    // EmptyObject to teleport to.
    public GameObject goalLocation;
    // The goal teleport position (Will be set automatically).
    private Vector3 teleportPosition;

    // Start is called before the first frame update
    void Start()
    {
        teleportPosition = goalLocation.transform.position;

    }

    public void teleportToPosition()
    {
        gameObject.transform.position = teleportPosition;
    }
}
