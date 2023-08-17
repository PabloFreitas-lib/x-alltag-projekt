using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketballGame : MonoBehaviour
{
    public Text text;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        text.text = counter.ToString();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ball"))
        {
            Debug.Log(collider.gameObject);
            IncreaseScore();
        }
    }

    private void IncreaseScore()
    {
        counter++;
        text.text = counter.ToString();
    }
}
