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

    public void SwitchLight(int playerLight, string ledColor, bool switchOn, float timeBeforeSwitchOff)
    {
        UduinoManager.Instance.digitalWrite(playerLight, State.HIGH);
        if (timeBeforeSwitchOff > 0 && switchOn)
           StartCoroutine(WaitForSwitchOff(playerLight, ledColor, timeBeforeSwitchOff));
    }

    IEnumerator WaitForSwitchOff(int playerLight, string color, float time)
    {
        yield return new WaitForSeconds(time);
        UduinoManager.Instance.digitalWrite(playerLight, State.LOW);
    }
}
