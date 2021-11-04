using System.Collections;
using System.Collections.Generic;
using Uduino;
using UnityEngine;

public class LedManager : MonoBehaviour
{
    public static LedManager Instance;
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

        SetupButton();

    }

    void Update()
    {
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            //if (InputManager.Instance.IsPlayerPressing(i + 1, "Red"))
            //{
            //    LedManager.Instance.SwitchLight(i+2, "Red", true, 1);
            //}
        }
    }

    void SetupButton()
    {
        UduinoManager.Instance.pinMode(3, PinMode.Output);  // setup du pin 3 pour écriture
    }

    public void SwitchLight(int playerLight, bool isRed, bool switchOn, float timeBeforeSwitchOff)
    {
        if (isRed)
        {
            int index = playerLight * 2;
            UduinoManager.Instance.digitalWrite(index, State.HIGH);
        }
        else
        {
            int index = playerLight * 2 +1;
            UduinoManager.Instance.digitalWrite(index, State.HIGH);
        }

        if (timeBeforeSwitchOff > 0 && switchOn)
           StartCoroutine(WaitForSwitchOff(playerLight, isRed, timeBeforeSwitchOff));
    }

    IEnumerator WaitForSwitchOff(int playerLight, bool isRed, float time)
    {
        playerLight--;
        yield return new WaitForSeconds(time);
        if (isRed)
        {
            int index = playerLight * 2;
            UduinoManager.Instance.digitalWrite(index, State.LOW);
        }
        else
        {
            int index = playerLight * 2 + 1;
            UduinoManager.Instance.digitalWrite(index, State.LOW);
        }

    }
}
