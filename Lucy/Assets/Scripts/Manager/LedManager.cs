using System.Collections;
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

        Invoke("SetupButton", 0.5f);

        for (int i = 0; i < 4; i++)
        {
            SwitchLight(i + 1, true, false, 0);
            SwitchLight(i + 1, false, false, 0);
        }

    }

    void Update()
    {
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            if (Input.GetButtonDown("Player" + (i+1) + "Red"))
            {
                Debug.Log(i+1);
                Debug.Log("Player" + (i+1));
                LedManager.Instance.SwitchLight((i+1), true, true, 0);
            }
            if (Input.GetButtonDown("Player" + (i + 1) + "Blue"))
            {
                Debug.Log(i + 1);
                Debug.Log("Player" + (i + 1));
                LedManager.Instance.SwitchLight((i + 1), false, true, 0);
            }
        }
    }

    void SetupButton()
    {
        for (int i = 2; i < 10; i++)
        {
            UduinoManager.Instance.pinMode(i, PinMode.Output);
        }
    }

    public void SwitchLight(int playerLight, bool isRed, bool switchOn, float timeBeforeSwitchOff)
    {
        int index = playerLight * 2;
        if (isRed)
        {
            if (switchOn)
            {
                UduinoManager.Instance.digitalWrite(index, State.HIGH);
            }
            else
                UduinoManager.Instance.digitalWrite(index, State.LOW);
        }
        else
        {
            index++;
            if (switchOn)
                UduinoManager.Instance.digitalWrite(index, State.HIGH);
            else
                UduinoManager.Instance.digitalWrite(index, State.LOW);
        }
        Debug.Log("I switch light " + index);

        if (timeBeforeSwitchOff > 0 && switchOn)
            StartCoroutine(WaitForSwitchOff(index, isRed, timeBeforeSwitchOff));
    }

    IEnumerator WaitForSwitchOff(int index, bool isRed, float time)
    {
        yield return new WaitForSeconds(time);
        if (isRed)
        {
            UduinoManager.Instance.digitalWrite(index, State.LOW);
        }
        else
        {
            UduinoManager.Instance.digitalWrite(index, State.LOW);
        }

    }

    public void BlindLight(bool blind)
    {
        if (blind)
        {
            //StartCoroutine(Blind());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    IEnumerator Blind()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i > 3)
                LedManager.Instance.SwitchLight(i + 1, false, true, 0.4f);
            else
                LedManager.Instance.SwitchLight(i + 1, true, true, 0.4f);
            yield return new WaitForSeconds(0.5f);
            Debug.Log(i + 1);
        }
        StartCoroutine(Blind());
    }
}
