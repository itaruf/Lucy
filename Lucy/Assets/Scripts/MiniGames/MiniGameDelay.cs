using System.Collections;
using UnityEngine;

public class MiniGameDelay : MiniGame
{
    [Header("Wait")]
    public float waitTime;
    protected override void LaunchGame()
    {
        ScoreManager.Instance.EnableScore(true);
        LedManager.Instance.BlindLight(true);
        StartCoroutine(WaitTimer());
    }
    public override void TimerEnd()
    {

    }

    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(waitTime);
        ScoreManager.Instance.EnableScore(false);
        GameEnd();
    }
}
