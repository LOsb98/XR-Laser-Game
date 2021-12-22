using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BezierSpline _spline;

    [SerializeField] private ObjectSpawner _objectSpawner;

    [SerializeField] private int _startingLives;

    [SerializeField] private Text _livesText;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _finalScoreText;

    [SerializeField] private Text _remainingObjectsText;

    [SerializeField] private GameObject _failureUI;

    [SerializeField] private GameObject _successUI;

    private int _currentLevel = 1;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

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
            _finalScoreText.text = $"Final score:\n{_score}";
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

            if (_remainingLives <= 0)
            {
                GameOver();
            }
        }
    }

    private int _remainingObjects;
    public int RemainingObjects
    {
        get
        {
            return _remainingObjects;
        }
        set
        {
            _remainingObjects = value;

            _remainingObjectsText.text = $"Remaining Objects: {_remainingObjects}";

            if (_remainingObjects <= 0)
            {
                LevelComplete();
            }

        }
    }

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

        StartNewLevel();
    }

    public void StartNewLevel()
    {
        _spline.RandomizePoints();

        //Calculating values which will gradually increase difficulty
        RemainingObjects = 5 + _currentLevel;

        float newPathCompletionSpeed = 20 - (_currentLevel / 2);
        newPathCompletionSpeed = Mathf.Clamp(newPathCompletionSpeed, 5f, 20);

        float newObjectDelay = 5 - (_currentLevel / 3);
        newObjectDelay = Mathf.Clamp(newObjectDelay, 0.8f, 5f);

        _objectSpawner.InitializeObjectSpawner(newObjectDelay, newPathCompletionSpeed);

        _objectSpawner.enabled = true;

        TurnOffUI();
    }

    public void ResetScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentScene);
    }

    public void PlayerDestroyedObject()
    {
        Score++;
        RemainingObjects--;
    }
    
    public void LevelComplete()
    {
        StopSpawningObjects();
        _successUI.SetActive(true);

        _currentLevel++;
    }

    public void GameOver()
    {
        StopSpawningObjects();
        _failureUI.SetActive(true);
    }

    private void StopSpawningObjects()
    {
        _objectSpawner.ClearObjects();
        _objectSpawner.enabled = false;
    }

    private void TurnOffUI()
    {
        //A bit wasteful to manually set these to false like this, though not a common operation so won't have an impact on performance
        //Could create a SetActiveSafe() extension method
        _failureUI.SetActive(false);
        _successUI.SetActive(false);
    }
}
