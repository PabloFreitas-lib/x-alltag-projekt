using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelection : MonoBehaviour
{
    public Button Cans;
    public GameObject CanGame;
    public Button Darts;
    public GameObject DartGame;
    public Button Baskets;
    public GameObject BasketGame;
    public Button Stack;
    public GameObject StackGame;

    private List<Button> GameButtons;
    private List<GameObject> Games;

    public GameObject dart;
    public GameObject ball;

    public GameObject katanaR;
    public GameObject katanaL;

    private void Start()
    {
        GameButtons = new List<Button>();
        Games = new List<GameObject>();

        GameButtons.Add(Cans);
        GameButtons.Add(Darts);
        GameButtons.Add(Baskets);
        GameButtons.Add(Stack);

        Games.Add(CanGame);
        Games.Add(DartGame);
        Games.Add(BasketGame);
        Games.Add(StackGame);



    }
    public void SelectGame(int game)
    {
        for (int i = 0; i < Games.Count;i++)
        {
            if (i != game)
            {
                GameButtons[i].interactable = true;
            }
            else
            {
                GameButtons[i].interactable = false;
            }
            
        }
        for (int i = 0; i < Games.Count; i++)
        {
            if (i != (game))
            {
                Games[i].SetActive(false);
            }
            else
            {
                Games[i].SetActive(true);
            }

        }
    }

    public void SpawnObject()
    {
        Instantiate(dart,transform.position, Quaternion.identity);
    }

    public void ChangeNinjaMode()
    {
        katanaL.SetActive(!katanaL.activeSelf);
        katanaR.SetActive(!katanaR.activeSelf);
    }
}