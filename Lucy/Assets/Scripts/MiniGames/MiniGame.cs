using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    public enum HowToScore
    {
        scoreFromScore,
        scoreFromRank,
    }

    public HowToScore howToScore;

    [Header("SetTimer")]
    public bool timerNeeded = true;
    public int minutes = 1;
    public int seconds = 30;

    void OnEnable()
    {
        if (timerNeeded)
        {
            TimerManager.Instance.minutes = minutes;
            TimerManager.Instance.seconds = seconds;
        }
        LaunchGame();
    }

    void Update()
    {
        if (InputManager.Instance.IsPlayerPressing(1, "Red"))
        {
            //UNIQUEMENT LE JOUEUR 1 MAIS LA COULEUR ROUGE
        }
        if (InputManager.Instance.IsPlayerPressing("Blue"))
        {
            //N'IMPORTE QUEL JOUEUR QUI APPUIE SUR LA COULEUR BLEUE
        }
    }

    protected abstract void LaunchGame();

    public virtual void GameEnd()
    {
        if(howToScore == HowToScore.scoreFromRank)
        {
            //Score par classement
        }
    }
}
