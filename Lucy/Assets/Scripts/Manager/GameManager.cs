using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerData[] players;
    public MiniGame[] gamesToLoad;
    [HideInInspector]public int oldGameLoaded = -1;
    public TextMeshProUGUI miniGameTxt;

    public static GameManager Instance;
    public bool maskUduinoInterface;

    void Awake()
    {
        if(Instance != null)
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
        LoadGame(0);
        if (maskUduinoInterface)
            GameObject.Find("UduinoInterface").SetActive(false);

        DialogManager.Instance.PlayDialog("KaleidoTest");

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    public void LoadGame(int indexLoadGame)
    {
        miniGameTxt.text = gamesToLoad[indexLoadGame].miniGameName;
        if (oldGameLoaded != -1)
            gamesToLoad[oldGameLoaded].gameObject.SetActive(false);
        gamesToLoad[indexLoadGame].gameObject.SetActive(true);
        oldGameLoaded = indexLoadGame;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LaunchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void DisplayDatas()
    {
        for (int i=0; i< players.Length; i++)
        {
            Debug.Log("playerId: " + (i+1) + " has a score of : "+players[i].playerScore);
        }
    }

    public void DisplayData(int playerId)
    {
        Debug.Log("playerId: "+(playerId+1)+ " has a score of : "+players[playerId].playerScore);
    }
}
