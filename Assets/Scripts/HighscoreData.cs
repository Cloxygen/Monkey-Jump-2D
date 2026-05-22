using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "HighscoreData", menuName = "Game/HighscoreData")]
public class HighscoreData : ScriptableObject
{
    public List<long> HighscoresList = new List<long>();
    private const int MaximumScoresCount = 10;

    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "highscores.json");

    public void AddNewHighscore(long scoreValue)
    {
        if (HighscoresList == null)
        {
            InitializeDefaultHighscoresList();
        }

        HighscoresList.Add(scoreValue);
        SortHighscoresDescending();

        if (HighscoresList.Count > MaximumScoresCount)
        {
            HighscoresList.RemoveAt(MaximumScoresCount);
        }

        SaveHighscoresToPersistentStorage();
    }

    public void LoadHighscoresFromPersistentStorage()
    {
        if (File.Exists(SaveFilePath))
        {
            try
            {
                string jsonContent = File.ReadAllText(SaveFilePath);
                JsonUtility.FromJsonOverwrite(jsonContent, this);
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning("Failed to load highscores: " + exception.Message);
                InitializeDefaultHighscoresList();
                return;
            }
        }
        else
        {
            InitializeDefaultHighscoresList();
        }

        EnsureHighscoresListIsValid();
    }

    public void SaveHighscoresToPersistentStorage()
    {
        string jsonContent = JsonUtility.ToJson(this, true);
        File.WriteAllText(SaveFilePath, jsonContent);
    }

    private void SortHighscoresDescending()
    {
        HighscoresList.Sort((firstScore, secondScore) => secondScore.CompareTo(firstScore));
    }

    private void InitializeDefaultHighscoresList()
    {
        HighscoresList.Clear();
        for (int i = 0; i < MaximumScoresCount; i++)
        {
            HighscoresList.Add(0);
        }
    }

    private void EnsureHighscoresListIsValid()
    {
        if (HighscoresList == null)
        {
            InitializeDefaultHighscoresList();
            return;
        }

        while (HighscoresList.Count < MaximumScoresCount)
        {
            HighscoresList.Add(0);
        }

        if (HighscoresList.Count > MaximumScoresCount)
        {
            HighscoresList.RemoveRange(MaximumScoresCount, HighscoresList.Count - MaximumScoresCount);
        }
    }
}
