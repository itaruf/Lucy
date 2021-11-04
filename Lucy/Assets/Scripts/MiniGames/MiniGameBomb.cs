using System.Collections;
using UnityEngine;

public class MiniGameBomb : MiniGame
{
    [Header("Bomb")]
    public float delayBeforeNextPlayer = 1f;
    int actualPlayer;
    bool isBombOnPlayer;

    public GameObject[] bombToShow;
    protected override void LaunchGame()
    {
        TimerManager.Instance.timerPlay = true;
        actualPlayer = Random.Range(1, 5);
        bombToShow[actualPlayer - 1].SetActive(true);
    }
    void Update()
    {
        if (Input.GetButtonDown("Player" + (actualPlayer) + "Red"))
        {
            if (!TimerManager.Instance.timerPlay)
                TimerManager.Instance.timerPlay = true;

            foreach (GameObject item in bombToShow)
            {
                item.SetActive(false);
            }
            StartCoroutine(ChangePlayer());
        }
    }

    IEnumerator ChangePlayer()
    {
        isBombOnPlayer = false;
        yield return new WaitForSeconds(delayBeforeNextPlayer);
        isBombOnPlayer = true;
        actualPlayer++;
        if (actualPlayer > 4)
            actualPlayer -= 4;
            bombToShow[actualPlayer - 1].SetActive(true);
    }
    public override void TimerEnd()
    {
        if (isBombOnPlayer)
        {
            Debug.Log("Player " + actualPlayer + " died");
            ScoreManager.Instance.AddScore(actualPlayer, -10);
            GameEnd();
        }
        else
        {
            Debug.Log("Bomb is not on player so no one die");
            GameEnd();
            //NoOne die
        }


    }
}
