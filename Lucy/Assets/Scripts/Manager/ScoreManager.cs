using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI[] playersScoreText;
    public int[] scoreByRank;


    public static ScoreManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        Score(0);
    }

    public void Score(int playerNum, int scoreToAdd)
    {
        PlayerData playerData = GameManager.Instance.players[playerNum];

        playerData.playerScore += scoreToAdd;
        playersScoreText[playerNum].text = "Player " + (playerData.playerId+1).ToString() + " - " + playerData.playerName + playerData.playerScore.ToString();
        playersScoreText[playerNum].color = playerData.playerColor;
    }

    public void Score(int scoreToAdd)
    {
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            PlayerData playerData = GameManager.Instance.players[i];

            playerData.playerScore += scoreToAdd;
            playersScoreText[i].text = "Player " + (playerData.playerId).ToString() + " - " + playerData.playerName + " : " + playerData.playerScore.ToString();
            playersScoreText[i].color = playerData.playerColor;
        }
    }
}
