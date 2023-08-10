using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanThrowingGame : MonoBehaviour
{
    public BoxCollider CanCounter;
    public int currentCount;
    public int startCount = 38;
    public Text countText;
    public GameObject CanGamePrefab;
    public Transform spawnPosition;
    public GameObject currentCanGame;

    // Start is called before the first frame update
    void Start()
    {
        CanCounter = GetComponent<BoxCollider>();
        currentCount = startCount;
        countText.text = currentCount.ToString();

 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dose"))
        {
            currentCount--;
            Destroy(other.gameObject);
            countText.text = currentCount.ToString();

            //Game over
            if(currentCount == 0)
            {
                countText.text = "You Won!";
                Invoke("StartNewGame",5f);
            }
        }
    }
    public void StartNewGame()
    {
        if(currentCanGame != null)
        {
            Debug.Log("Hallo");
            Destroy(currentCanGame.gameObject);
            
        }
        currentCanGame = Instantiate(CanGamePrefab, spawnPosition.position, new Quaternion(0,0,0,0), transform);
        currentCount = startCount;
        countText.text = currentCount.ToString();
    }
}
