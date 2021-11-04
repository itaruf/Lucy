using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGamePrisonersDilema : MiniGame
{
    public class BehaviorScores
    {
        public int coopScore { get; set; }
        public int betrayScore { get; set; }
    }

    [Header("Game")]
    [Range(2, 10)]
    public int numberOfRounds = 2;
    [Range(1, 10)]
    public int currentRound = 1;

    [Header("Chrono")]
    public float timerToThink = 0;
    public float timerBeforeStartingNewRound = 0;
    private float initialTimerToThink = 0;
    private float initialTimerBeforeStarting = 0;
    public float scorePopUpDuration = 0;

    [Header("Text")]
    public TextMeshProUGUI text;
    public string textBeforeNumb = "";
    public TextMeshProUGUI[] playersScoresPopup;

    private bool canVote;

    public Dictionary<int, bool> playersCurrentRoundActions = new Dictionary<int, bool>(4); // <ID du joueur, action jouée : trahison = true,, coopération = false>
    public Dictionary<int, bool> playersPreviousRoundActions = new Dictionary<int, bool>(4);
    public Dictionary<int, BehaviorScores> playerBehaviorScore = new Dictionary<int, BehaviorScores>(4); // <ID du joueur, (coopScore, betrayScore)>

    // On conserve la dynamique du groupe
    public BehaviorScores behaviorScoresGlobal = new BehaviorScores { betrayScore = 0, coopScore = 0 };
    [Header("Behavior Score")]
    public int betrayScore = 0;
    public int coopScore = 0;

    // Points pour chaque cas
    [Header("Outcome Score Points")]
    public int coop1of4Points = 0;
    public int coop2of4Points = 0;
    public int coop3of4Points = 0;
    public int coop4of4Points = 0;

    public int betray1of4Points = 0;
    public int betray2of4Points = 0;
    public int betray3of4Points = 0;
    public int betray4of4Points = 0;

    // Situations possibles
    // [Header("Outcome Triggered")]
    private bool isInCoop1of4 = false;
    private bool isInCoop2of4 = false;
    private bool isInCoop3of4 = false;
    private bool isInCoop4of4 = false;

    private bool isInBetray1of4 = false;
    private bool isInBetray2of4 = false;
    private bool isInBetray3of4 = false;
    private bool isInBetray4of4 = false;

    private List<bool> _coopOutcomes = new List<bool>(4);
    private List<bool> _betrayOutcomes = new List<bool>(4);
    private List<int> _coopOutcomesPoints = new List<int>(4);
    private List<int> _betrayOutcomesPoints = new List<int>(4);

    // Permet de déterminer les cas dans lesquels nous sommes au tour actuel
    public int _numberOfBetrayers = 0;
    public int _numberOfCoops = 0;

    // Permet de déterminer les cas dans lesquels nous étions au tour précédent
    private int _previousNumberOfBetrayers = 0;
    private int _previousNumberOfCoops = 0;

    // Scores comportementaux
    [Header("Behavior Score Points")]
    public int betrayStarterPoints = 0;
    public int betrayFollowPoints = 0;
    public int betrayContinuePoints = 0;
    public int betrayContinueHardPoints = 0;

    public int coopStarterPoints = 0;
    public int coopFollowPoints = 0;
    public int coopContinuePoints = 0;
    public int coopFighterPoints = 0;
    public int coopFighterHardPoints = 0;

    private bool waitBeforeStart = true;

    void Start()
    {
        /*// Initialisation de l'état de base des joueurs
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            GameManager.Instance.players[i].playerScore = 0;
            playersPreviousRoundActions.Add(i + 1, false);

            if (Random.Range(1, 3) == 1) // initialisation aléatoire pour forcer le joueur a joué une action
                playersCurrentRoundActions.Add(i + 1, true);
            else
                playersCurrentRoundActions.Add(i + 1, false);
            //playersCurrentRoundActions.Add(i + 1, false);
            playerBehaviorScore.Add(i + 1, new BehaviorScores { coopScore = 0, betrayScore = 0 });
        }

        int[] coopOutcomesPointsArray = {
            coop1of4Points,
            coop2of4Points,
            coop3of4Points,
            coop4of4Points,
        };

        int[] betrayOutcomesPointsArray = {
            betray1of4Points,
            betray2of4Points,
            betray3of4Points,
            betray4of4Points,
        };

        bool[] coopArray = {
            isInCoop1of4,
            isInCoop2of4,
            isInCoop3of4,
            isInCoop4of4,
        };

        bool[] betrayArray = {
            isInBetray1of4,
            isInBetray2of4,
            isInBetray3of4,
            isInBetray4of4
        };

        _coopOutcomesPoints.AddRange(coopOutcomesPointsArray);
        _betrayOutcomesPoints.AddRange(betrayOutcomesPointsArray);

        _coopOutcomes.AddRange(coopArray);
        _betrayOutcomes.AddRange(betrayArray);

        initialTimerToThink = timerToThink;
        initialTimerBeforeStarting = timerBeforeStartingNewRound;

        StartCoroutine(GameIntroduction());*/
    }

    IEnumerator GameIntroduction()
    {
        Debug.Log("The Game is about to begin...");
        yield return new WaitForSeconds(3f);
        Debug.Log("... Now !");
        canVote = true;
        waitBeforeStart = false;
    }

    void Update()
    {
        if (waitBeforeStart)
            return;

        betrayScore = behaviorScoresGlobal.betrayScore;
        coopScore = behaviorScoresGlobal.coopScore;

        if (currentRound <= numberOfRounds)
        {
            //Debug.Log("Is vote enabled : " + canVote);
            if (canVote)
            {
                ChooseAction(); // Les joueurs font leur choix d'action pour un tour donné sur une fenêtre de temps
                //StartCoroutine(WaitingForPlayersInput());

                if (timerToThink > 0)
                {
                    timerToThink -= Time.deltaTime;
                    if (timerToThink <= 0)
                    {
                        timerToThink = Mathf.Clamp(timerToThink, 0, Mathf.Infinity);
                    }

                    SetText(timerToThink);
                }
                else
                {
                    if (canVote)
                    {
                        canVote = false;

                        DetermineBehaviorScoresToEnable();
                        DeterminePlayerScore();

                        Debug.Log("_previousNumberOfBetrayers: " + _previousNumberOfBetrayers);
                        Debug.Log("_previousNumberOfCoops " + _previousNumberOfCoops);

                        StartCoroutine(DelayNextRound());
                    }
                }
            }
        }
        GameEnd();
    }

    protected override void LaunchGame()
    {
        // Initialisation de l'état de base des joueurs
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            GameManager.Instance.players[i].playerScore = 0;
            playersPreviousRoundActions.Add(i + 1, false);

            if (Random.Range(1, 3) == 1) // initialisation aléatoire pour forcer le joueur a joué une action
                playersCurrentRoundActions.Add(i + 1, true);
            else
                playersCurrentRoundActions.Add(i + 1, false);
            //playersCurrentRoundActions.Add(i + 1, false);
            playerBehaviorScore.Add(i + 1, new BehaviorScores { coopScore = 0, betrayScore = 0 });
        }

        int[] coopOutcomesPointsArray = {
            coop1of4Points,
            coop2of4Points,
            coop3of4Points,
            coop4of4Points,
        };

        int[] betrayOutcomesPointsArray = {
            betray1of4Points,
            betray2of4Points,
            betray3of4Points,
            betray4of4Points,
        };

        bool[] coopArray = {
            isInCoop1of4,
            isInCoop2of4,
            isInCoop3of4,
            isInCoop4of4,
        };

        bool[] betrayArray = {
            isInBetray1of4,
            isInBetray2of4,
            isInBetray3of4,
            isInBetray4of4
        };

        _coopOutcomesPoints.AddRange(coopOutcomesPointsArray);
        _betrayOutcomesPoints.AddRange(betrayOutcomesPointsArray);

        _coopOutcomes.AddRange(coopArray);
        _betrayOutcomes.AddRange(betrayArray);

        initialTimerToThink = timerToThink;
        initialTimerBeforeStarting = timerBeforeStartingNewRound;

        StartCoroutine(GameIntroduction());
    }

    void ChooseAction() // Choisir de trahir ou non
    {
        if (!canVote) return;
        //Debug.Log("Choose Action");
        InputManager input = InputManager.Instance;

        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            if (input.IsPlayerPressing(i + 1, "Red"))
            {
                Debug.Log($"playerid {i + 1} chooses to betray");
                playersCurrentRoundActions[i + 1] = true; // Le joueur trahit
            }

            if (input.IsPlayerPressing(i + 1, "Blue"))
            {
                Debug.Log($"playerid {i + 1} chooses to coop");
                playersCurrentRoundActions[i + 1] = false; // Le joueur coopère
            }
        }
    }

    void CheckNumberOfBetrayersCoops()
    {
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            if (playersCurrentRoundActions[i + 1])
            {
                _numberOfBetrayers++;
            }
            else
            {
                _numberOfCoops++;
            }
        }
        Debug.Log("_numberOfBetrayers: " + _numberOfBetrayers);
        Debug.Log("_numberOfCoops: " + _numberOfCoops);
    }

    void DetermineBehaviorScoresToEnable()
    {
        CheckNumberOfBetrayersCoops();
        // Reset
        for (int i = 0; i < _coopOutcomes.Count; i++)
        {
            _coopOutcomes[i] = false;
            _betrayOutcomes[i] = false;
        }

        switch (_numberOfBetrayers)
        {
            case 0: // Aucun traître = 4 coops
                _coopOutcomes[3] = true;
                break;
            case 1: // 1 traître = 3 coops
                _coopOutcomes[2] = true;
                _betrayOutcomes[0] = true;
                break;
            case 2: // 2 traître = 2 coops
                _coopOutcomes[1] = true;
                _betrayOutcomes[1] = true;
                break;
            case 3: // 3 traître = 1 coop
                _coopOutcomes[0] = true;
                _betrayOutcomes[2] = true;
                break;
            case 4: // Full traître
                _betrayOutcomes[3] = true;
                break;
            default:
                break;
        }
    }

    void DeterminePlayerScore()
    {
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            if (_betrayOutcomes[i]) // On cherche le cas correspondant au nombre de personnes qui ont trahi
            {
                for (int j = 0; j < GameManager.Instance.players.Length; j++) // On cherche toutes les personnes concernées par ce cas (true)
                {
                    if (playersCurrentRoundActions[j + 1])
                    {
                        ScoreManager.Instance.AddScore(j, _betrayOutcomesPoints[i]); // On leur attribue le score correspondant au cas de trahison actuel
                        if (currentRound != 1) IncreaseBetrayScore(j + 1); // On augmente le score de traitrise
                        if (_betrayOutcomesPoints[i] < 0)
                        {
                            playersScoresPopup[j].text = _betrayOutcomesPoints[i].ToString();
                            playersScoresPopup[j].color = Color.red;
                        }
                        else
                        {
                            playersScoresPopup[j].text = "+" + _betrayOutcomesPoints[i].ToString();

                            if (_betrayOutcomesPoints[i] == 0) playersScoresPopup[j].color = Color.blue;
                            else playersScoresPopup[j].color = Color.green;
                        }
                    }
                }
            }

            if (_coopOutcomes[i]) // On cherche le cas correspondant au nombre de personnes qui ont coopéré
            {
                for (int j = 0; j < GameManager.Instance.players.Length; j++) // On cherche toutes les personnes concernées par ce cas (false)
                {
                    if (!playersCurrentRoundActions[j + 1])
                    {
                        ScoreManager.Instance.AddScore(j, _coopOutcomesPoints[i]); // On leur attribue le score correspondant au cas de trahison actuel
                        if (currentRound != 1) IncreaseCoopScore(j + 1); // On augmente le score de coop
                        if (_coopOutcomesPoints[i] < 0)
                        {
                            playersScoresPopup[j].text = _coopOutcomesPoints[i].ToString();
                            playersScoresPopup[j].color = Color.red;
                        }
                        else
                        {
                            playersScoresPopup[j].text = "+" + _coopOutcomesPoints[i].ToString();

                            if (_coopOutcomesPoints[i] == 0) playersScoresPopup[j].color = Color.blue;
                            else playersScoresPopup[j].color = Color.green;
                        }

                    }
                }
            }
        }
        //GameManager.Instance.DisplayDatas();
        StartCoroutine(DisplayScorePopup());
    }

    void IncreaseBetrayScore(int playeriD) // Un joueur trahit sur ce tour
    {
        switch (_previousNumberOfBetrayers)
        {
            case 0: // 0 joueur a trahi précédemment // was in coop4of4
                playerBehaviorScore[playeriD].betrayScore += betrayStarterPoints;
                behaviorScoresGlobal.betrayScore += betrayStarterPoints;
                break;
            case 1: // 1 joueur a trahi précédemment
                if (playersPreviousRoundActions[playeriD]) // was in betray1of4  
                {
                    playerBehaviorScore[playeriD].betrayScore += betrayContinueHardPoints;
                    behaviorScoresGlobal.betrayScore += betrayContinueHardPoints;
                }
                else // was in coop3of4  
                {
                    playerBehaviorScore[playeriD].betrayScore += betrayFollowPoints;
                    behaviorScoresGlobal.betrayScore += betrayFollowPoints;
                }
                break;
            case 2: // 2 joueurs ont trahi précédemment
                if (playersPreviousRoundActions[playeriD]) // was in betray2of4  
                {
                    playerBehaviorScore[playeriD].betrayScore += betrayContinuePoints;
                    behaviorScoresGlobal.betrayScore += betrayContinuePoints;
                }
                else // was in coop2of4  
                {
                    playerBehaviorScore[playeriD].betrayScore += betrayFollowPoints;
                    behaviorScoresGlobal.betrayScore += betrayFollowPoints;
                }
                break;
            case 3: // 3 joueurs ont trahi précédemment
                if (playersPreviousRoundActions[playeriD]) // was in betray3of4  
                {
                    playerBehaviorScore[playeriD].betrayScore += betrayContinuePoints;
                    behaviorScoresGlobal.betrayScore += betrayContinuePoints;
                }
                else // was in coop1of4 
                {
                    playerBehaviorScore[playeriD].betrayScore += betrayFollowPoints;
                    behaviorScoresGlobal.betrayScore += betrayFollowPoints;
                }
                break;
            case 4: // 4 joueurs ont trahi précédemment // was in betray4of4
                playerBehaviorScore[playeriD].betrayScore += betrayContinuePoints;
                behaviorScoresGlobal.betrayScore += betrayContinuePoints;
                break;
            default:
                break;
        }
    }

    void IncreaseCoopScore(int playeriD) // Un joueur coopère sur ce tour
    {
        switch (_previousNumberOfCoops)
        {
            case 0: // 0 joueur a coopéré précédemment // was in betray4of4  
                behaviorScoresGlobal.coopScore += coopStarterPoints;
                break;
            case 1: // 1 joueur a coopéré précédemment
                if (playersPreviousRoundActions[playeriD]) // was in betray3of4  
                {
                    playerBehaviorScore[playeriD].coopScore += coopFollowPoints;
                    behaviorScoresGlobal.coopScore += coopFollowPoints;
                }
                else // was in coop1of4 
                {
                    playerBehaviorScore[playeriD].coopScore += coopFighterHardPoints;
                    behaviorScoresGlobal.coopScore += coopFighterHardPoints;
                }
                break;
            case 2: // 2 joueurs ont coopéré précédemment
                if (playersPreviousRoundActions[playeriD]) // was in betray2of4  
                {
                    playerBehaviorScore[playeriD].coopScore += coopFollowPoints;
                    behaviorScoresGlobal.coopScore += coopFollowPoints;
                }
                else // was in coop2of4 
                {
                    playerBehaviorScore[playeriD].coopScore += coopFighterPoints;
                    behaviorScoresGlobal.coopScore += coopFighterPoints;
                }
                break;
            case 3: // 3 joueurs ont coopéré précédemment
                if (playersPreviousRoundActions[playeriD]) // was in betray1of4  
                {
                    playerBehaviorScore[playeriD].coopScore += coopFollowPoints;
                    behaviorScoresGlobal.coopScore += coopFollowPoints;
                }
                else // was in coop3of4  
                {
                    playerBehaviorScore[playeriD].coopScore += coopFighterPoints;
                    behaviorScoresGlobal.coopScore += coopFighterPoints;
                }
                break;
            case 4: // 4 joueur ont coopéré précédemment
                playerBehaviorScore[playeriD].coopScore += coopContinuePoints;
                behaviorScoresGlobal.coopScore += coopContinuePoints;
                break;
            default:
                break;
        }
    }

    IEnumerator DelayNextRound()
    {
        yield return new WaitForSeconds(1f);

        timerBeforeStartingNewRound--;
        SetText(timerBeforeStartingNewRound);

        if (timerBeforeStartingNewRound > 0)
        {
            StartCoroutine(DelayNextRound());
            yield break;
        }

        if (!canVote)
        {
            currentRound++;
            yield return new WaitForSeconds(1f);
            canVote = true;
            SetupVariables();
        }
    }
    void SetupVariables()
    {
        // On conserve le nombre de traitres/coopérateurs pour le tour suivant
        _previousNumberOfBetrayers = _numberOfBetrayers;
        _previousNumberOfCoops = _numberOfCoops;
        _numberOfBetrayers = 0;
        _numberOfCoops = 0;

        // On sauvegarde les actions des joueurs du tour actuel pour le tour suivant
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            playersPreviousRoundActions[i + 1] = playersCurrentRoundActions[i + 1];

            if (Random.Range(1, 3) == 1) // à chaque tour, le joueur doit jouer, sinon il se voit attribuer une action aléatoire
                playersCurrentRoundActions[i + 1] = true;
            else
                playersCurrentRoundActions[i + 1] = false;
        }

        timerBeforeStartingNewRound = initialTimerBeforeStarting; // reset du timer avant prochain tour
        timerToThink = initialTimerToThink; // reset du timer pour voter
    }

    IEnumerator DisplayScorePopup()
    {
        for (int i = 0; i < playersScoresPopup.Length; i++)
        {
            playersScoresPopup[i].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(scorePopUpDuration);

        for (int i = 0; i < playersScoresPopup.Length; i++)
        {
            playersScoresPopup[i].gameObject.SetActive(false);
        }
    }
    void SetText(float timer)
    {
        this.text.text = textBeforeNumb + ((int)timer).ToString();
    }

    public override void TimerEnd()
    {
    }
}