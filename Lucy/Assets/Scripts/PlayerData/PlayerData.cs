using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "PlayerData")]

public class PlayerData : ScriptableObject
{
    [Header("Initialize")]
    public int playerId;
    [Header("Players stats")]
    public string playerName;
    public int playerScore;
    public Color playerColor;

}
