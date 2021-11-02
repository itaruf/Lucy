using System.Text.RegularExpressions;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputManagerColorButton[] inputs;
    public static InputManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            inputs[i].playerName = GameManager.Instance.players[i].playerName + " | id :" + GameManager.Instance.players[i].playerId;
        }
    }

    public void IsPressed(Transform buttonName)
    {
        string name = buttonName.name;
        int numbersOnly = int.Parse(Regex.Replace(name, "[^0-9]", ""));

        bool isRed = name.Contains("Red");
        Press(numbersOnly, isRed);
    }

    void Press(int playerID, bool red)
    {
        playerID --;
        if (red)
        {
            inputs[playerID].red = true;
        }
        else
        {
            inputs[playerID].blue = true;
        }
    }

    void LateUpdate()
    {
        ResetBool();
    }

    void ResetBool()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].blue)
                inputs[i].blue = false;
            if (inputs[i].red)
                inputs[i].red = false;
        }
    }

    public bool IsPlayerPressing(int playerID, string color)
    {
        if (color.Contains("r") || color.Contains("R"))
        {
            if(inputs[playerID - 1].red)
                Debug.Log("<color=#FF0000>Player " + playerID + "</color>");

            return inputs[playerID - 1].red;
        }
        else
        {
            if (inputs[playerID - 1].blue)
                Debug.Log("<color=#0000FF>Player " + playerID + "</color>");

            return inputs[playerID - 1].blue;
        }
    }
    public bool IsPlayerPressing(string color)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (color.Contains("r") || color.Contains("R"))
            {
                if (inputs[i].red)
                {
                    Debug.Log("<color=#FF0000>Player " + (i + 1) + "</color>");
                    return true;
                }
            }
            else
            {
                if (inputs[i].blue)
                {
                    Debug.Log("<color=#0000FF>Player " + (i+1) + "</color>");
                    return true;
                }

            }
        }
        return false;
    }


}


[System.Serializable]
public class InputManagerColorButton
{
    [HideInInspector] public string playerName;
    public bool red;
    public bool blue;
}
