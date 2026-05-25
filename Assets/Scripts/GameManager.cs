using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This script holds the game state and other scripts will subscribe to OnGameStateChanged to check the current gamestate
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private State _currentState;
    public UnityEvent OnGameStateChanged;
    public State CurrentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            OnGameStateChanged?.Invoke();
        }
    }

    void Awake()
    {
        SetInstance();
        _currentState = State.StartMenu;
    }
    void Start()
    {
        PlayerPrefs.DeleteKey("ScreenWidth");
        PlayerPrefs.DeleteKey("ScreenHeight");
        PlayerPrefs.Save();
    }


    void SetInstance()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("Game Manager already exists");
            Destroy(this.gameObject);
        }
    }

    public enum State
    {
        StartMenu, Playing, Paused, GameOver
    }

    public void SetStatePlay()
    {
        CurrentState = State.Playing;
    }

    public void SetStateStart()
    {
        CurrentState = State.StartMenu;
    }
}
