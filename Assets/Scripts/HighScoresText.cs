using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoresText : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> scoreTexts;

    void OnEnable()
    {
        PopulateHighScoreTexts();
    }

    private void PopulateHighScoreTexts()
    {
        if (ScoreManager.Instance == null || ScoreManager.Instance.Highscores == null)
        {
            return;
        }

        for (int i = 0; i < ScoreManager.Instance.Highscores.Count; i++)
        {
            if (i >= scoreTexts.Count)
            {
                break;
            }

            if (scoreTexts[i] != null)
            {
                scoreTexts[i].text = ScoreManager.Instance.Highscores[i].ToString();
            }
        }
    }
}
