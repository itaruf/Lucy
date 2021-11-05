using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MiniGameVote : MiniGame
{
    public List<int> voteFor = new List<int>(4);
    public Dictionary<int, int> majority = new Dictionary<int, int>();
    bool canVote = false;
    int lastValue = -1;
    protected override void LaunchGame()
    {
        canVote = true;
        TimerManager.Instance.timerPlay = true;
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            voteFor.Add(0);
            LedManager.Instance.SwitchLight(i+1, true, true, 0);
            LedManager.Instance.SwitchLight(i+1, false, true, 0);
        }
    }

    void Update()
    {
        if (canVote)
        {
            CheckVote();
        }
    }

    void CheckVote()
    {
        InputManager input = InputManager.Instance;
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            if (Input.GetButtonDown("Player"+(i+1)+"Red"))
            {
                voteFor[i] = i + 2;
                if (voteFor[i] > 4)
                    voteFor[i] -= 4;
                else if (voteFor[i] < 1)
                    voteFor[i] += 4;
            }
            if (Input.GetButtonDown("Player" + (i + 1) + "Blue"))
            {
                voteFor[i] = i + 0;
                if (voteFor[i] > 4)
                    voteFor[i] -= 4;
                else if (voteFor[i] < 1)
                    voteFor[i] += 4;
            }

            if (Input.GetButton("Player" + (i + 1) + "Blue") && Input.GetButton("Player" + (i + 1) + "Red"))
            {
                voteFor[i] = i + 3;
                if (voteFor[i] > 4)
                    voteFor[i] -= 4;
                else if (voteFor[i] < 1)
                    voteFor[i] += 4;
            }
        }
    }

    public override void TimerEnd()
    {
        canVote = false;
        PlayerDidntVote();
        Debrief();
        //SortValues();
    }

    void Debrief()
    {
        for (int i = 0; i < voteFor.Count; i++)
        {
            ScoreManager.Instance.AddScore(voteFor[i]-1, -2);
        }

        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            voteFor.Add(0);
            LedManager.Instance.SwitchLight(i + 1, true, false, 0);
            LedManager.Instance.SwitchLight(i + 1, false, false, 0);
        }
        GameEnd();
    }

    void PlayerDidntVote()
    {
        for (int i = 0; i < voteFor.Count; i++)
        {
            if (voteFor[i] == 0)
                voteFor[i] = i+1;
        }
    }

    void SortValues()
    {
        majority.Clear();
        for (int i = 0; i < voteFor.Count; i++)
        {
            majority.Add(i, 0);
        }
        for (int i = 0; i < voteFor.Count; i++)
        {
            for (int j = 0; j < majority.Count; j++)
            {
                if (voteFor[i] == (j + 1))
                {
                    majority[j]++;
                }
            }
        }

        Dictionary<int, int> majorityOrdered = majority.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        foreach (KeyValuePair<int, int> item in majorityOrdered)
        {
            Debug.Log("Player " + (item.Key + 1) + " has " + item.Value + " vote");
        }
        majorityOrdered = majority.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        lastValue = -1;
        int index = 0;
        foreach (KeyValuePair<int, int> item in majorityOrdered)
        {
            if (index == 0)
            {
                lastValue = item.Value;
            }
            else if (index == 1)
            {
                if(lastValue == item.Value)
                {
                    Debug.Log("Draw");
                    //DRAW
                    return;
                }
            }
            else if(index == majorityOrdered.Count-1)
            {
                Debug.Log("Player " + (item.Key + 1) + " is eliminated because he has " + item.Value + " votes");
                //ELIMINATE PLAYER
                return;
            }
          index++;
        }
    }
}
