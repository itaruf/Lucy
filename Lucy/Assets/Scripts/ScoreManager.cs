using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI[] playersScoreText;
    public int[] playersScore;

    void Start()
    {
        Score(0);
    }

    public void Score(int playerNum, int scoreToAdd)
    {
        playersScore[playerNum] += scoreToAdd;
        playersScoreText[playerNum].text = playersScore[playerNum].ToString();
    }

    public void Score(int scoreToAdd)
    {
        for (int i = 0; i < playersScore.Length; i++)
        {
            playersScore[i] += scoreToAdd;
            playersScoreText[i].text = playersScore[i].ToString();
        }
    }
}
