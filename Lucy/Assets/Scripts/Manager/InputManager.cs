using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputManagerColorButton[] inputs;
    public List<InputManagerColorButton> inputsPressed;
    public static InputManager Instance;
    public bool playWithUIButton;

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
            inputsPressed.Add(inputs[i]);
            LedManager.Instance.SwitchLight(i, true, true, 0);
            LedManager.Instance.SwitchLight(i, false, true, 0);
        }

    }

    public void IsPressed(Transform buttonName)
    {
        string name = buttonName.name;
        int numbersOnly = int.Parse(Regex.Replace(name, "[^0-9]", ""));

        bool isRed = name.Contains("Red");
        Press(numbersOnly, isRed, true);
    }

    void Update()
    {
        if (!playWithUIButton)
        {
            for (int i = 0; i < GameManager.Instance.players.Length; i++)
            {
                Press(i + 1, true, Input.GetButton("Player" + (i + 1) + "Blue"));
                Press(i + 1, false, Input.GetButton("Player" + (i + 1) + "Red"));
            }
        }
    }

    void Press(int playerID, bool red, bool imPressing)
    {
        playerID--;
        if (red)
        {
            inputs[playerID].red = imPressing;
        }
        else
        {
            inputs[playerID].blue = imPressing;
        }
        StartCoroutine(PressWithDelay(playerID, red, imPressing));
    }

    IEnumerator PressWithDelay(int playerID, bool red, bool imPressing)
    {
        yield return new WaitForEndOfFrame();
        if (red)
        {
            inputsPressed[playerID].red = imPressing;
        }
        else
        {
            inputsPressed[playerID].blue = imPressing;
        }
    }

    void LateUpdate()
    {
        if (playWithUIButton)
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
            if (inputs[playerID - 1].red)
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
                    Debug.Log("<color=#0000FF>Player " + (i + 1) + "</color>");
                    return true;
                }

            }
        }
        return false;
    }
    public bool IsPlayerHolding(int playerID, string color)
    {
        if (color.Contains("r") || color.Contains("R"))
        {
            if (inputs[playerID - 1].red)
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
    public bool IsPlayerHolding(string color)
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
                    Debug.Log("<color=#0000FF>Player " + (i + 1) + "</color>");
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
