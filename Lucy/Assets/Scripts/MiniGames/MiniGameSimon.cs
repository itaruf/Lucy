using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MiniGameSimon : MiniGame
{
    [Header("Simon Game")]
    public int[] seriesNum;
    int actualSerieIndex = 0;
    public float delayToShowSerie = 2f;
    public float delayToUnShow = 0.25f;
    [HideInInspector] public List<string> buttonToPress;

    [Space(20)]
    public GameObject[] redColor;
    public GameObject[] blueColor;


    bool showSerie;
    int actualRound;
    string roundColor;
    int playerShouldPress;
    string colorToPress;

    bool canPressButton;
    protected override void LaunchGame()
    {
        TimerManager.Instance.timerPlay = true;
        CreateSerie(seriesNum[actualSerieIndex]);
    }

    public override void TimerEnd()
    {

    }

    void Update()
    {
        if (canPressButton)
        {
            for (int i = 0; i < GameManager.Instance.players.Length; i++)
            {
                if (Input.GetButtonDown("Player" + (i + 1) + "Red"))
                {
                    Debug.Log((i + 1) + "Red");
                }
                if (Input.GetButtonDown("Player" + (i + 1) + "Blue"))
                {
                    Debug.Log((i + 1) + "Blue");
                }
            }
            //if (InputManager.Instance.IsPlayerPressing(playerShouldPress, colorToPress))
            //{
            //    NextRound();
            //}
            //else if(InputManager.Instance.IsPlayerPressing("Red") || InputManager.Instance.IsPlayerPressing("Blue"))
            //{
            //    Defeat();
            //}
            if (Input.GetButtonDown("Player" + (playerShouldPress) + colorToPress))
            {
                NextRound();
            }
            else
            {
                for (int i = 0; i < GameManager.Instance.players.Length; i++)
                {
                    if (Input.GetButtonDown("Player" + (i + 1) + "Red") || Input.GetButtonDown("Player" + (i + 1) + "Blue"))
                        Defeat(i + 1);
                }
            }
        }
    }

    void Defeat(int playerFailed)
    {
        int rand = Random.Range(1, 5);
        while (rand == playerFailed)
        {
            rand = Random.Range(1, 5);
        }
        ScoreManager.Instance.AddScore(rand, -3);
        GameEnd();
    }

    void Victory()
    {
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            ScoreManager.Instance.AddScore(i + 1, 3);
        }
        GameEnd();
    }

    void NextRound()
    {
        actualRound++;
        if (actualRound == buttonToPress.Count)
        {
            Debug.Log("Congrats ! You did a serie, wait for the next one");
            CreateSerie(seriesNum[actualSerieIndex]);
            return;
        }


        for (int i = 0; i < blueColor.Length; i++)
        {
            LedManager.Instance.SwitchLight(i + 1, true, false, 0);
            LedManager.Instance.SwitchLight(i + 1, false, false, 0);
            blueColor[i].SetActive(false);
            redColor[i].SetActive(false);
        }

        ChangeTarget();


        if (!canPressButton)
        {
            StartCoroutine(ShowSerie());
            return;
        }
    }

    void ChangeTarget()
    {

        roundColor = buttonToPress[actualRound];
        playerShouldPress = int.Parse(Regex.Replace(roundColor, "[^0-9]", ""));
        colorToPress = Regex.Replace(roundColor, "[0-9]", "");
    }

    IEnumerator ShowSerie()
    {
        canPressButton = false;
        ChangeTarget();

        if (colorToPress == "Red")
        {
            redColor[playerShouldPress - 1].SetActive(true);
            LedManager.Instance.SwitchLight(playerShouldPress, true, true, 0);
        }
        else
        {
            LedManager.Instance.SwitchLight(playerShouldPress, false, true, 0);
            blueColor[playerShouldPress - 1].SetActive(true);
        }

        yield return new WaitForSeconds(delayToShowSerie);

        if (colorToPress == "Red")
        {
            redColor[playerShouldPress - 1].SetActive(false);
            LedManager.Instance.SwitchLight(playerShouldPress, true, true, 0);
        }
        else
        {
            blueColor[playerShouldPress - 1].SetActive(false);
            LedManager.Instance.SwitchLight(playerShouldPress, false, true, 0);
        }

        yield return new WaitForSeconds(delayToUnShow);


        actualRound++;
        if (actualRound < buttonToPress.Count)
        {
            StartCoroutine(ShowSerie());
        }
        else
        {
            canPressButton = true;
            actualRound = -1;
            NextRound();
        }
    }
    public void CreateSerie(int roundNum)
    {
        if (actualSerieIndex == seriesNum.Length)
        {
            Victory();
            return;
        }

        actualSerieIndex++;
        actualRound = -1;
        buttonToPress.Clear();
        for (int i = 0; i < roundNum; i++)
        {
            string colorToAdd;
            colorToAdd = UnityEngine.Random.Range(1, 5).ToString();
            int randCol = UnityEngine.Random.Range(0, 2);
            if (randCol == 0)
                colorToAdd += "Red";
            else
                colorToAdd += "Blue";
            buttonToPress.Add(colorToAdd);
        }
        actualRound = 0;
        StartCoroutine(ShowSerie());
    }
}
