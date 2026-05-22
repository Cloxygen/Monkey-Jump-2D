using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [SerializeField] int initialPointsPerBanana = 10;
    [SerializeField] int pointsPerBananaIncrement = 10;
    [SerializeField] HighscoreData highscoreDataContainer;

    public UnityEvent onScoreChanged = new UnityEvent();
    private long _currentScore;
    private int _pointsPerBanana;

    public long CurrentScore {
        get { return _currentScore; } 
        private set 
        {
            _currentScore = value;
            onScoreChanged?.Invoke();
        } 
    }

    public int PointsPerBanana
    {
        get { return _pointsPerBanana; }
    }

    public List<long> Highscores
    {
        get
        {
            if (highscoreDataContainer != null)
            {
                return highscoreDataContainer.HighscoresList;
            }
            return new List<long>();
        }
    }

    void Awake()
    {
        SetInstance();
        if (highscoreDataContainer == null)
        {
            highscoreDataContainer = ScriptableObject.CreateInstance<HighscoreData>();
        }
        InitializeHighscores();
        _pointsPerBanana = initialPointsPerBanana;
    }

    void SetInstance()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("Score Manager Already exists");
            Destroy(this.gameObject);
        }
    }

    private void InitializeHighscores()
    {
        if (highscoreDataContainer != null)
        {
            highscoreDataContainer.LoadHighscoresFromPersistentStorage();
        }
    }

    public void IncrementScore()
    {
        CurrentScore += _pointsPerBanana;
        _pointsPerBanana += pointsPerBananaIncrement;
    }

    public void DoubleScore()
    {
        CurrentScore *= 2;
    }

    public void CheckStateAndSaveScore()
    {
        if (GameManager.Instance.CurrentState == GameManager.State.GameOver)
        {
            if (highscoreDataContainer != null)
            {
                highscoreDataContainer.AddNewHighscore(_currentScore);
            }
        }
    }

    public void CheckStateAndResetScore()
    {
        if (GameManager.Instance.CurrentState == GameManager.State.StartMenu)
        {
            _currentScore = 0;
            _pointsPerBanana = initialPointsPerBanana;
        }
    }
}
