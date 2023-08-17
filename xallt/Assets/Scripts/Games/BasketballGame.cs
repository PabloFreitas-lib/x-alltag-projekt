using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketballGame : MonoBehaviour
{
    public Collider collider;
    public Text text;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        text.text = counter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ball"))
        {
            Debug.Log(collider.gameObject);
            increaseScore();
        }
    }

    private void increaseScore()
    {
        counter++;
        text.text = counter.ToString();
    }
}
