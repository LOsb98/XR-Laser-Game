using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _startingLives;

    [SerializeField] private Text _livesText;

    [SerializeField] private Text _scoreText;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        Score = 0;

        RemainingLives = _startingLives;
    }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            _scoreText.text = $"Score: {_score}";
        }
    }

    private int _currentStage;
    public int CurrentStage
    {
        get
        {
            return _currentStage;
        }
    }

    private int _remainingLives;
   public int RemainingLives
    {
        get
        {
            return _remainingLives;
        }
        set
        {
            _remainingLives = value;
            _livesText.text = $"Lives: {_remainingLives}";
        }
    }

    public void GameOver()
    {
        //Stop object spawning

        //Show game over UI
    }
}
