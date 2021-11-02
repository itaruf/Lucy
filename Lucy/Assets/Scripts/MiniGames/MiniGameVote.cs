using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MiniGameVote : MiniGame
{
    public List<int> voteFor = new List<int>(4);
    [SerializeField] public Dictionary<int, int> majority = new Dictionary<int, int>();
    bool canVote = false;
    protected override void LaunchGame()
    {
        canVote = true;
    }

    void Update()
    {
        if (canVote)
        {
            InputManager input = InputManager.Instance;

            for (int i = 0 ; i < GameManager.Instance.players.Length; i++)
            {
                if (input.IsPlayerPressing(i + 1, "Red"))
                {
                    voteFor[i] = i + 2;
                    if (voteFor[i] > 4)
                        voteFor[i] -= 4;
                    else if (voteFor[i] < 1)
                        voteFor[i] += 4;
                }
                if (input.IsPlayerPressing(i + 1, "Blue"))
                {
                    voteFor[i] = i + 0;
                    if (voteFor[i] > 4)
                        voteFor[i] -= 4;
                    else if (voteFor[i] < 1)
                        voteFor[i] += 4;
                }
            }
        }
    }

    public override void TimerEnd()
    {
        canVote = false;
        majority.Clear();
        for (int i = 0; i < voteFor.Count; i++)
        {
            majority.Add(i , 0);
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

        Dictionary<int,int> majorityOrdered = majority.OrderByDescending(x => x.Value).ToDictionary(x=>x.Key, x=>x.Value);

        foreach (KeyValuePair<int, int> item in majorityOrdered)
        {
            Debug.Log("Player " + (item.Key+1) + " has " + item.Value + " vote");
        }
    }
}
