using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameManager.State showDuringState = GameManager.State.Playing;
    void Start()
    {
        UpdateText();
        CheckStateAndShowScore();
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
