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
                if (playerHasPressed[i])
                    allAreGood ++;
                if(allAreGood == playerHasPressed.Count)
                {
                    GameEnd();
                }
                if(Input.GetButtonDown("Player" + (i+1) + "Red"))
                {
                    playerHasPressed[i] = true;
                }
            }

        }
    }
}
