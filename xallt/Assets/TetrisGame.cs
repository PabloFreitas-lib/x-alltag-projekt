using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisGame : MonoBehaviour
{
    [Header("Game Properties")]
    public float gameDuration = 60f;
    public List<GameObject> tetrisPiecePrefabs;  // List of Tetris piece prefabs
    public Transform tetrisPieceSpawnPoint;
    public Transform basePosition;
    public GameObject piecesParent; // GameObject to store spawned objects
    [Header("UI")]
    public Text scoreText;
    public Text timerText;
    public Button restartButton; 
    public Button spawnButton;   

    private float timeLeft;
    private float highestPieceHeight;
    private int score = 0;
    private List<GameObject> tetrisPieces = new List<GameObject>();

    private void Start()
    {
        timeLeft = gameDuration;
        UpdateScoreText();
        UpdateTimerText();  
        SpawnTetrisPiece();

        restartButton.onClick.AddListener(StartNewGame);
        spawnButton.onClick.AddListener(SpawnTetrisPiece);
    

    }

    private void Update()
    {
        UpdateTimer();
        CalculateAndUpdateScore();
    }

    private void UpdateTimer()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerText();
            if (timeLeft <= 0)
            {
                StartNewGame();
            }
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
    }

    private void SpawnTetrisPiece()
    {
        GameObject newTetrisPiece = Instantiate(GetRandomTetrisPiecePrefab(), tetrisPieceSpawnPoint.position, Quaternion.identity);
        newTetrisPiece.transform.rotation = Quaternion.Euler(0f, 0, -90f);
        newTetrisPiece.transform.parent = piecesParent.transform;
        tetrisPieces.Add(newTetrisPiece);
    }

    private GameObject GetRandomTetrisPiecePrefab()
    {
        int randomIndex = Random.Range(0, tetrisPiecePrefabs.Count);
        return tetrisPiecePrefabs[randomIndex];
    }

    private void CalculateAndUpdateScore()
    {
        foreach (GameObject tetrisPiece in tetrisPieces)
        {
            float pieceHeight = tetrisPiece.transform.position.y;
            if (pieceHeight > basePosition.position.y)
            {
                highestPieceHeight = pieceHeight;
            }
        }

        float distance = ((highestPieceHeight - basePosition.position.y) * 5f);
        score = Mathf.FloorToInt(distance);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void StartNewGame()
    {
        // Restart the game logic here.
        foreach (GameObject tetrisPiece in tetrisPieces)
        {
            Destroy(tetrisPiece);
        }
        tetrisPieces.Clear();

        // Reset score and time left
        score = 0;
        timeLeft = gameDuration;

        UpdateScoreText();
        UpdateTimerText();;
        SpawnTetrisPiece();
    }
}
