using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameGain : MiniGame
{
    public int[] numbersToReach;
    public int delayBeforeChangeNumber;
    public List<int> playersScore = new List<int>(4);
    int actualIndex = -1;
    bool canPress;
    int total;
    public float delay = 0.5f;

    int lastPlayerWhoPressed;

    protected override void LaunchGame()
    {
        canPress = true;
    }

    public override void TimerEnd()
    {

    }

    void Update()
    {
        if (canPress)
        {
            if (InputManager.Instance.IsPlayerPressing(1, "Red") || InputManager.Instance.IsPlayerPressing(1, "Blue"))
            {
                playersScore[0]++;
                lastPlayerWhoPressed = 1;
            }
            if (InputManager.Instance.IsPlayerPressing(2, "Red") || InputManager.Instance.IsPlayerPressing(2, "Blue"))
            {
                playersScore[1]++;
                lastPlayerWhoPressed = 2;

            }
            if (InputManager.Instance.IsPlayerPressing(3, "Red") || InputManager.Instance.IsPlayerPressing(3, "Blue"))
            {
                playersScore[2]++;
                lastPlayerWhoPressed = 3;

            }
            if (InputManager.Instance.IsPlayerPressing(4, "Red") || InputManager.Instance.IsPlayerPressing(4, "Blue"))
            {
                playersScore[3]++;
                lastPlayerWhoPressed = 4;

            }
        }
        total = 0;
        for (int i = 0; i < playersScore.Count; i++)
        {
            total += playersScore[i];
        }

        if(total == playersScore[actualIndex])
        {
            WaitForTotal();
        }
    }

    IEnumerator WaitForTotal()
    {
        yield return new WaitForSeconds(delayBeforeChangeNumber);
        CompareEnd();
    }

    void CompareEnd()
    {
        if(total == playersScore[actualIndex])
        {
            Debug.Log("Tout le monde gagne");
        }
        else
        {
            Debug.Log("Le joueur " + (lastPlayerWhoPressed+1) + " a perdu");
        }
        ChangeIndex();
    }

    void ChangeIndex()
    {
        actualIndex++;
    }
}
