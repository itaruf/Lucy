using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGamePrisonersDilema : MonoBehaviour
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
    public float timerToThink = 0f;
    public float timerBeforeStartingNewRound = 0f;
    private bool hasRoundStarted = false;

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
    private int _numberOfBetrayers = 0;
    private int _numberOfCoops = 0;

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

    void Start()
    {
        // Initialisation de l'état de base des joueurs
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            playersPreviousRoundActions.Add(i + 1, false);
            playersCurrentRoundActions.Add(i + 1, false);
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

        // On commence la manche
        hasRoundStarted = true;

        ChooseAction();
        DetermineBehaviorScoresToEnable();
        DeterminePlayerScore();

        SetupVariables();
    }

    void Update()
    {
        betrayScore = behaviorScoresGlobal.betrayScore;
        coopScore = behaviorScoresGlobal.coopScore;

        if (!hasRoundStarted)
        {
            hasRoundStarted = !hasRoundStarted;
            ChooseAction();
            //StartCoroutine(RoundTimer());
        }

        if (!TimerManager.Instance.timerPlay)
            TimerManager.Instance.timerPlay = true;
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
            playersCurrentRoundActions[i + 1] = false;
        }
    }

    void ChooseAction() // Choisir de trahir ou non
    {
        InputManager input = InputManager.Instance;

        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            if (input.IsPlayerPressing(i + 1, "Red"))
            {
                playersCurrentRoundActions[i + 1] = true; // Le joueur trahit
            }

            if (input.IsPlayerPressing(i + 1, "Blue"))
            {
                playersCurrentRoundActions[i + 1] = false; // Le joueur ne trahit pas
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
        Debug.Log("_numberOfCoops: "+_numberOfCoops);
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
                    }
                }
            }
        }
        GameManager.Instance.DisplayDatas();
        Debug.Log("betrayScore: "+betrayScore);
        Debug.Log("coopScore: "+coopScore);
    }

    void IncreaseBetrayScore(int playeriD) // Un joueur trahit sur ce tour
    {
        Debug.Log("_previousNumberOfBetrayers: " + _previousNumberOfBetrayers);
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
        Debug.Log("_previousNumberOfCoops " + _previousNumberOfCoops);
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

    /*protected override void LaunchGame()
    {
    }

    public override void TimerEnd()
    {
    }*/
}
