using System;
using System.Collections.Generic;
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
    
    public int amountOfMiniGames = 3;
    
    // public Enums.GamePhase serverGamePhase;
    
    // This list stocks text answers ordered like the players (textAnswers[2] -> player2's answer)
    [AllowMutableSyncType]
    public SyncList<string> textAnswers = new();

    public int collectedAnswers;
    
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

    public void StartVotePhase()
    {
        SendGamePhaseToGameManagerNetwork(Enums.GamePhase.voting);
        // CAUTION : When Julien will initialize playerNumber, change from 1 to 4 instead of 0 to 3 !!!!!!!!!!!
        StartVotePhaseToClient(textAnswers[0], textAnswers[1], textAnswers[2], textAnswers[3]);
    }

    [ObserversRpc]
    public void StartVotePhaseToClient(string answer1, string answer2, string answer3, string answer4)
    {
        switch (_gameManagerNetwork.myLocalPlayer.playerData.playerNumber)
        {
            case 0:     //PUT 1 HERE WHEN JULIEN WILL HAVE FINALLY DONE HIS JOB
                _uiManager.ShowClientVotingArea(answer4, answer2, answer3);
                break;
            case 1:
                _uiManager.ShowClientVotingArea(answer4, answer1, answer3);
                break;
            case 2:
                _uiManager.ShowClientVotingArea(answer4, answer1, answer2);
                break;
            case 3:
                _uiManager.ShowClientVotingArea(answer2, answer1, answer3);
                break;
        }
    }

    public void FinishVotePhase()
    {
        // PlayerNetwork winner = numberOfVotesByPlayer[0];
        // foreach (var i in numberOfVotesByPlayer)
        // {
        //     
        // }
    }
}
