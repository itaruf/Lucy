using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI[] playersScoreText;
    public GameObject[] scoreTxt;
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
        AddScore(0);
    }

    public void AddScore(int playerNum, int scoreToAdd)
    {
        PlayerData playerData = GameManager.Instance.players[playerNum];

        playerData.playerScore += scoreToAdd;
        playersScoreText[playerNum].text = playerData.playerScore.ToString();
        //playersScoreText[playerNum].color = playerData.playerColor;
    }

    public void AddScore(int scoreToAdd)
    {
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            PlayerData playerData = GameManager.Instance.players[i];

            playerData.playerScore += scoreToAdd;
            playersScoreText[i].text = playerData.playerScore.ToString();
            //playersScoreText[i].color = playerData.playerColor;
        }
    }

    public void EnableScore(bool active)
    {
        for (int i = 0; i < scoreTxt.Length; i++)
        {
            scoreTxt[i].SetActive(active);
        }
    }
}
