using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float minutes;
    public float seconds;
    public TextMeshProUGUI timerText;
    public string textBeforeNumb = "";
    [HideInInspector]public bool timerPlay = false;

    public static TimerManager Instance;

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
    void Update()
    {
        if (!timerPlay)
            return;

        seconds -= Time.deltaTime;

        //Add 0 before number under 10
        if (seconds >= 10)
            timerText.text = textBeforeNumb + " 0" + (int)minutes + " : " + (int)seconds;
        else
            timerText.text = textBeforeNumb + " 0" + (int)minutes + " : 0" + (int)seconds;


        if (seconds < 0)
        {
            seconds = 60;
            minutes--;
        }
        if (minutes < 0)
        {
            timerPlay = false;

            GameManager.Instance.gamesToLoad[GameManager.Instance.oldGameLoaded].TimerEnd();
            return;
        }
    }
}
