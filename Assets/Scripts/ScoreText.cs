using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameManager.State showDuringState = GameManager.State.Playing;
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
        CheckStateAndShowScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        scoreText.text = ScoreManager.Instance.CurrentScore.ToString();
    }

    public void CheckStateAndShowScore()
    {
        if (GameManager.Instance.CurrentState != showDuringState)
        {
            scoreText.enabled = false;
        }
        else
        {
            scoreText.enabled = true;
            UpdateText();
        }
    }
}
