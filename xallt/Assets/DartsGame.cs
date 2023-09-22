using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartsGame : MonoBehaviour
{
    public Text scoretext;

    public void ChangeScore(int score)
    {
        scoretext.text = score.ToString();
    }
}
