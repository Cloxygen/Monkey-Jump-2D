using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject hiscoresMenu;
    State _currentState = State.Main;
    public State CurrentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            OnMenuStateChanged?.Invoke();
        }
    }
    public UnityEvent OnMenuStateChanged;

    // Start is called before the first frame update
    void Start()
    {
        SetInstance();
        CheckStateAndShowMenu();
        OnMenuStateChanged.AddListener(HandleStartMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void CheckStateAndShowMenu()
    {
        startMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.State.GameOver:
                gameOverMenu.SetActive(true);
                break;
            case GameManager.State.StartMenu:
                HandleStartMenu();
                break;
            default:
                startMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                hiscoresMenu.SetActive(false);
                break;
        }
    }

    void HandleStartMenu()
    {
        switch (CurrentState)
        {
            case State.Main:
                startMenu.SetActive(true);
                hiscoresMenu.SetActive(false);
                break;
            case State.Hiscores:
                startMenu.SetActive(false);
                hiscoresMenu.SetActive(true);
                break;
            default:
                startMenu.SetActive(false);
                hiscoresMenu.SetActive(false);
                break;
        }
    
    }

    void SetInstance()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("Menu Manager already exists");
            Destroy(this.gameObject);
        }
    }

    public enum State
    {
        Main, Hiscores
    }

    public void SetStateHiscores()
    {
        CurrentState = State.Hiscores;
    }

    public void SetStateMain()
    {
        CurrentState = State.Main;
    }
}
