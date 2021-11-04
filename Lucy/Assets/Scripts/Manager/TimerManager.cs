using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public float minutes;
    public float seconds;
    public TextMeshProUGUI[] timerText;
    public string textBeforeNumb = "";
    [HideInInspector] public bool timerPlay = false;

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

        SetText();

        if (seconds < 0)
        {
            seconds = 60;
            minutes--;
        }
        if (minutes < 0)
        {
            timerPlay = false;
            foreach (TextMeshProUGUI text in timerText)
            {
                text.text = "00 : 00";
            }
            GameManager.Instance.gamesToLoad[GameManager.Instance.oldGameLoaded].TimerEnd();
            return;
        }
    }

    public void SetText()
    {
        //Add 0 before number under 10
        if (seconds >= 10)
        {
            foreach (TextMeshProUGUI text in timerText)
            {
                text.text = textBeforeNumb + " 0" + (int)minutes + " : " + ((int)seconds + 1);
            }
        }
        else
        {
            foreach (TextMeshProUGUI text in timerText)
            {
                text.text = textBeforeNumb + " 0" + (int)minutes + " : 0" + ((int)seconds + 1);
            }
        }
    }
}
