using System;
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
            if (InputManager.Instance.IsPlayerPressing(playerShouldPress, colorToPress))
            {
                NextRound();
            }
            else if(InputManager.Instance.IsPlayerPressing("Red") || InputManager.Instance.IsPlayerPressing("Blue"))
            {
                Defeat();
            }
        }
    }

    void Defeat()
    {
        Debug.Log("Defeat");
    }

    void Victory()
    {
        Debug.Log("Victory");
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
            redColor[playerShouldPress - 1].SetActive(true);
        else
            blueColor[playerShouldPress - 1].SetActive(true);

        yield return new WaitForSeconds(delayToShowSerie);

        if (colorToPress == "Red")
            redColor[playerShouldPress - 1].SetActive(false);
        else
            blueColor[playerShouldPress - 1].SetActive(false);

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
        if(actualSerieIndex == seriesNum.Length)
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
