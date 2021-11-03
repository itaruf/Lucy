using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameDelay : MiniGame
{
    public float waitTime;
    protected override void LaunchGame()
    {
        StartCoroutine(WaitTimer());
    }

    public override void TimerEnd()
    {

    }

    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(waitTime);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
