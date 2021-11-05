using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGamePressSquare : MiniGame
{
    public List<bool> playerHasPressed;
    bool canPress;
    int allAreGood;
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
            allAreGood = 0;
            for (int i = 0; i < GameManager.Instance.players.Length; i++)
            {
                LedManager.Instance.SwitchLight(i + 1, true, !playerHasPressed[i], 0);

                if (playerHasPressed[i])
                    allAreGood ++;
                if(allAreGood == playerHasPressed.Count)
                {
                    GameEnd();
                }
                if(Input.GetButtonDown("Player" + (i+1) + "Red"))
                {
                    Debug.Log("Player" + (i + 1) + "Red");
                    playerHasPressed[i] = true;
                }
            }

        }
    }
}
