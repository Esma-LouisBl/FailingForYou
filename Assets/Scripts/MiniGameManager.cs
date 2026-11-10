using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniGameManager : NetworkBehaviour
{
    #region Variables

    public List<MiniGameData> miniGamesDrawn = new List<MiniGameData>();
    [SerializeField]
    public List<MiniGameData> allMiniGames = new List<MiniGameData>();
    [SerializeField]
    public List<MiniGameData> tieQuestions = new List<MiniGameData>();
    public MiniGameData currentTieQuestion;
    
    public int amountOfMiniGames = 3;
    
    // public Enums.GamePhase serverGamePhase;
    
    // This list stocks text answers ordered like the players (textAnswers[2] -> player2's answer)
    [AllowMutableSyncType]
    public SyncList<string> textAnswers = new();
    // private List<int> _votes = new List<int>(); 
    public Dictionary<string, int> votes = new Dictionary<string, int>();

    public int collectedAnswers, collectedVotes;
    
    List<int> playersInTie = new List<int>();
    
    // public Dictionary<PlayerNetwork, int> numberOfVotesByPlayer = new Dictionary<PlayerNetwork, int>();

    #endregion
    
    #region ScriptReferences

    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private GameManagerNetwork _gameManagerNetwork;

    #endregion

    private void Start()
    {
        textAnswers.GetCollection(true).Capacity = 5;
        for (int i = 0; i < 5; i++)
        {
            textAnswers.Add("");
        }

        // _votes.Capacity = 5;
        // for (int i = 0; i < 5; i++)
        // {
        //     _votes.Add(0);
        // }
    }

    public void SendGamePhaseToGameManagerNetwork(Enums.GamePhase phase)
    {
        _gameManagerNetwork.UpdateGamePhaseOnServer(phase);
    }
    
    //Add to miniGamesDrawn [numberOfMiniGames -> depends if short or long game] from _allMiniGames
    public void ChooseMiniGames(int numberOfMiniGames)
    {
        for (int i = 0; i < numberOfMiniGames; i++)
        {
            int r = Random.Range(0, allMiniGames.Count);
            miniGamesDrawn.Add(allMiniGames[r]);
            allMiniGames.RemoveAt(r); //we don't want two times the same question in the game
        }
    }

    public void StartMiniGame()
    {
        SetMiniGame(miniGamesDrawn[0]);
        miniGamesDrawn.RemoveAt(0);
        
        // Reset some variables here
        for (int i = 0; i < 5; i++)
        {
            textAnswers[i] = "";
        }

        collectedAnswers = 0;
    }
    
    public void SetMiniGame(MiniGameData miniGame)
    {
        _uiManager.ShowHostUI();
        _uiManager.HideConnectionUI();
        _uiManager.UpdateMiniGameUIHost(miniGame);

        SendGamePhaseToGameManagerNetwork(Enums.GamePhase.answeringQuestion);
        
        LaunchMiniGameToClient(miniGame);
    }
    
    [ObserversRpc]
    public void LaunchMiniGameToClient(MiniGameData miniGame)
    {
        _uiManager.UpdateMiniGameUIClient(miniGame);
    }

    public void CollectTextAnswers(string answer, int playerNumber)
    {
        collectedAnswers++;
        textAnswers[playerNumber] = answer;
    }


    #region Vote

    public void StartVotePhase()
    {
        collectedVotes = 0; //variable reset
        votes.Clear();
        for (int i = 1; i < textAnswers.Count; i++)
        {
            votes.Add(textAnswers[i], 0);
        }
        
        SendGamePhaseToGameManagerNetwork(Enums.GamePhase.voting);
        StartVotePhaseHost();
        StartCoroutine(WaitBeforeVoteOnClient()); 
    }

    private IEnumerator WaitBeforeVoteOnClient()
    {
        if (_gameManagerNetwork.players.Count < 4)
        {
            yield return new WaitForSeconds(8f);
            StartVotePhaseToClient(textAnswers[1], textAnswers[2], textAnswers[3], textAnswers[4]);
        }
        else
        {
            yield return new WaitForSeconds(10f);
            StartVotePhaseToClient(textAnswers[1], textAnswers[2], textAnswers[3], textAnswers[4]);
        }

        _uiManager.ShowHostMainUIWhilePlayersAreVoting();
    }
    
    public void StartVotePhaseHost()
    {
        if (_gameManagerNetwork.players.Count < 4)
        {
            _uiManager.ShowHostVotingPhase(false, textAnswers[1], textAnswers[2], textAnswers[3], textAnswers[4]);
        }
        else
        {
            _uiManager.ShowHostVotingPhase(true, textAnswers[1], textAnswers[2], textAnswers[3], textAnswers[4]);
        }
    }

    [ObserversRpc]
    public void StartVotePhaseToClient(string answer1, string answer2, string answer3, string answer4)
    {
        switch (_gameManagerNetwork.myLocalPlayer.playerData.playerNumber)
        {
            case 1:
                _uiManager.ShowClientVotingArea(answer2, answer3, answer4);
                break;
            case 2:
                _uiManager.ShowClientVotingArea(answer1, answer3, answer4);
                break;
            case 3:
                _uiManager.ShowClientVotingArea(answer1, answer2, answer4);
                break;
            case 4:
                _uiManager.ShowClientVotingArea(answer1, answer2, answer3);
                break;
        }
    }

    public void CollectTextVotes(string answerVoted, int playerNumber)
    {
        collectedVotes++;
        votes[answerVoted]++;
    }

    public void MiniGameWinnerElection()
    {
        Dictionary<int, int> ranking = new Dictionary<int, int>();  //acts like a podium : the first int is the PLAYER, the second int is the RANK associated
        ranking.Clear();
        int votesToCheck = 3; //maximum of votes to check while ranking the players
        int occupiedPlaces = 0; //just to know where are we when ranking the players
        bool occupiedPlacesHasChanged = false;
        
        List<int> votesOrdered = new List<int>();
        votesOrdered.Clear();
        for (int i = 0; i < 5; i++) //lists initialization
        {
            votesOrdered.Add(0);
        }

        //this for loop orders all the votes like textAnswers (so 1 is for player1 etc.)
        for (int i = 0; i < textAnswers.Count; i++)
        {
            if (votes.ContainsKey(textAnswers[i]))
            {
                votesOrdered[i] = votes[textAnswers[i]];
            }
        }

        while (ranking.Count < _gameManagerNetwork.players.Count)
        {
            for (int i = 1; i < votesOrdered.Count; i++)
            {
                if (votesOrdered[i] == votesToCheck)
                {
                    ranking.Add(i, occupiedPlaces+1);
                    occupiedPlacesHasChanged = true;
                }
            }

            if (occupiedPlacesHasChanged)
            {
                occupiedPlaces++;
            }
            votesToCheck--;
        }

        //check if there are ties
        for (int i = 1; i < 4; i++)     //we check for all the 4 possible ranks
        {
            playersInTie.Clear();
            foreach (var values in ranking)
            {
                if (values.Value == i)  //if someone is in that rank, we add em to playersInTie
                {
                    playersInTie.Add(values.Key);
                }
            }

            if (playersInTie.Count >= 1) //if there are multiple people on the same rank, there is a tie OR there is a person
            {                            //who is alone but superior to the others, so the inferior ties aren't important
                break;
            }
        }

        _uiManager.ShowMiniGameWinner(votesOrdered[1], votesOrdered[2], votesOrdered[3], votesOrdered[4]);
        
        //if tie : start tie
        if (playersInTie.Count > 1)
        {
            StartCoroutine(WaitBeforeTie());
        }
        // FinishVotePhase();
    }

    private IEnumerator WaitBeforeTie()
    {
        yield return new WaitForSeconds(5f);
        Tie();
    }
    public void Tie()
    {
        currentTieQuestion = tieQuestions[Random.Range(0, tieQuestions.Count)];
        _uiManager.ShowTieOnHost(playersInTie, currentTieQuestion.question);

        StartTieOnClient(playersInTie);
    }

    [ObserversRpc]
    public void StartTieOnClient(List<int> playersConcerned)
    {
        Debug.Log("le moment de vérité");
        if (playersConcerned.Contains(_gameManagerNetwork.myLocalPlayer.playerData.playerNumber))
        {
            Debug.Log("je suis dans l'égalité mais dans l'illégalité");
            _uiManager.ShowTieOnClient();
        }
    }

    public void FinishVotePhase()
    {
        //var reset
        for (int i = 0; i < 5; i++)
        {
            textAnswers[i] = "";
        }

        // for (int i = 0; i < 5; i++)
        // {
        //     _votes[i] = 0;
        // }
        votes.Clear();
    }

    #endregion
    
}
