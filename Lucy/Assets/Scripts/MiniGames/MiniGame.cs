using System;
using System.Collections;
using TMPro;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    public string miniGameName;
    public HowToScore howToScore;

    [Header("SetTimer")]
    public bool timerNeeded = true;
    public int minutes = 1;
    public int seconds = 30;

    [Header("WaitForDialogEnd")]
    public bool waitDialogEnd;
    public string dialogToFind;

    public enum HowToScore
    {
        scoreFromScore,
        scoreFromRank,
        noScore,
    }


    void OnEnable()
    {
        TimerManager.Instance.timerPlay = false;
        foreach (TextMeshProUGUI text in TimerManager.Instance.timerText)
        {
            text.text = "";
        }
        TimerManager.Instance.gameObject.SetActive(timerNeeded);
        if (timerNeeded)
        {
            TimerManager.Instance.minutes = minutes;
            TimerManager.Instance.seconds = seconds;
            TimerManager.Instance.SetText();
        }
        if (waitDialogEnd)
        {
            DialogManager.Instance.PlayDialog("WaitToPressIntroVoice");
            StartCoroutine(WaitDialogEnd());
        }
        else
        {
            LaunchGame();
        }
    }

    IEnumerator WaitDialogEnd()
    {
        Dialog d = Array.Find(DialogManager.Instance.dialogs, dialog => dialog.name == dialogToFind);
        yield return new WaitForSeconds(d.clip.length);
        LaunchGame();
    }

    void Update()
    {

    }

    protected abstract void LaunchGame();
    public abstract void TimerEnd();

    public virtual void GameEnd()
    {
        GameManager.Instance.LoadNextGame();
    }
}
