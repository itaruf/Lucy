using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    public string miniGameName;
    public HowToScore howToScore;

    [Header("SetTimer")]
    public bool timerNeeded = true;
    public int minutes = 1;
    public int seconds = 30;
    public enum HowToScore
    {
        scoreFromScore,
        scoreFromRank,
        noScore,
    }


    void OnEnable()
    {
        if (timerNeeded)
        {
            TimerManager.Instance.minutes = minutes;
            TimerManager.Instance.seconds = seconds;
            TimerManager.Instance.timerPlay = true;
        }
        LaunchGame();
    }

    void Update()
    {
       
    }

    protected abstract void LaunchGame();
    public abstract void TimerEnd();

    public virtual void GameEnd()
    {
        
    }
}
