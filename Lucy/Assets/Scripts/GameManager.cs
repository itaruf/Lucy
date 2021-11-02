using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class GameManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameObject[] gamesToLoad;
    int oldGameLoaded = -1;

    void Start()
    {
        //UduinoManager.Instance.pinMode(3, PinMode.Output);  // setup du pin 3 pour écriture
        //UduinoManager.Instance.digitalWrite(3, State.HIGH); // allume "une led"
        //UduinoManager.Instance.digitalWrite(3, State.LOW); // eteint
    }

    public void LoadGame(int indexLoadGame)
    {
        if (oldGameLoaded != -1)
            gamesToLoad[oldGameLoaded].SetActive(false);
        gamesToLoad[indexLoadGame].SetActive(true);
        oldGameLoaded = indexLoadGame;
    }
}
