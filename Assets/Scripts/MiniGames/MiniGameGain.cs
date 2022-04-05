using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameGain : MiniGame
{
    public int[] numbersToReach;
    public float delayBeforeChangeNumber;
    public List<int> playersScore = new List<int>(4);
    int actualIndex = -1;
    bool canPress;
    int total;
    public float delay = 0.5f;

    public TextMeshProUGUI rulesText;

    int lastPlayerWhoPressed;
    bool imChecking = false;

    protected override void LaunchGame()
    {
        canPress = true;
        ChangeIndex();
    }

    public override void TimerEnd()
    {

    }

    void Update()
    {
        if (canPress)
        {
            for (int i = 0; i < playersScore.Count; i++)
            {
                if (InputManager.Instance.IsPlayerPressing(i+1, "Red") || InputManager.Instance.IsPlayerPressing(i+1, "Blue"))
                {
                    playersScore[i]++;
                    lastPlayerWhoPressed = i+1;
                    CheckScore();
                }
            }
        }
    }

    void CheckScore()
    {
        total = 0;
        for (int i = 0; i < playersScore.Count; i++)
        {
            total += playersScore[i];
        }

        if (!imChecking)
        {
            if (total == numbersToReach[actualIndex])
            {
                StartCoroutine(WaitForTotal());
            }
        }
    }

    IEnumerator WaitForTotal()
    {
        imChecking = true;
        yield return new WaitForSeconds(delayBeforeChangeNumber);
        CompareEnd();
    }

    void CompareEnd()
    {
        if (total == playersScore[actualIndex])
        {
            Debug.Log("Tout le monde gagne");
        }
        else
        {
            Debug.Log("Le joueur " + (lastPlayerWhoPressed + 1) + " a perdu");
        }
        ChangeIndex();
    }

    void ChangeIndex()
    {
        actualIndex++;
        ChangeNumText(rulesText, "You have to reach " + numbersToReach[actualIndex].ToString());

        for (int i = 0; i < playersScore.Count; i++)
        {
            ScoreManager.Instance.AddScore(i, playersScore[i]);
        }

        if (actualIndex == playersScore.Count)
        {
            canPress = false;
            GameEnd();
        }
    }

    void ChangeNumText(TextMeshProUGUI textToChange, string text)
    {
        textToChange.text = text;
    }
}
