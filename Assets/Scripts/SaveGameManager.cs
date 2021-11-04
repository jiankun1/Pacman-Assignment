using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    private const string scoreKey = "HighScore";
    private const string timeKey = "GameTime";
    private int currentHighScore;
    public int LoadScore()
    {
        if (PlayerPrefs.HasKey(scoreKey))
        {
            return PlayerPrefs.GetInt(scoreKey);
        }

        return 0;
    }

    public string LoadTime()
    {
        if (PlayerPrefs.HasKey(timeKey))
        {
            return PlayerPrefs.GetString(timeKey);
        }

        return "00:00:00";
    }

    public void SaveScoreandTime(int score, string time)
    {
        currentHighScore = PlayerPrefs.GetInt(scoreKey);
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(scoreKey, score);
            PlayerPrefs.SetString(timeKey, time);
            PlayerPrefs.Save();
        }
        
    }

    

}
