using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBoard : MonoBehaviour
{
    public int score;
    private DartsGame dartsGame;
    // Start is called before the first frame update
    void Start()
    {
        dartsGame = FindObjectOfType<DartsGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dart"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            dartsGame.ChangeScore(score);
        }
    }
}
