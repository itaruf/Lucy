using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameCountdown : MiniGame
{
    protected override void LaunchGame()
    {
        SwitchOnAllRedButton();
    }

    void Update()
    {
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            if(InputManager.Instance.IsPlayerPressing(i+1, "Red"))
            {
                Debug.Log("Player " + (i + 1) + " is pressing red button");
                //Player 1 is pressing red button
                //Player 2 is pressing red button
                //Player 3 is pressing red button
                //Player 4 is pressing red button
            }
        }
    }

    public override void TimerEnd()
    {
        GameEnd();
    }

    void SwitchOnAllRedButton()
    {

    }
}
