using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class MiniGameManager : NetworkBehaviour
{
    #region Variables

    public List<MiniGameData> miniGamesDrawn = new List<MiniGameData>();
    [SerializeField]
    public List<MiniGameData> allMiniGames = new List<MiniGameData>();
    
    public int amountOfMiniGames = 3;
    
    // public Enums.GamePhase serverGamePhase;
    
    public Dictionary<PlayerNetwork, string> textAnswers = new Dictionary<PlayerNetwork, string>();
    
    // public Dictionary<PlayerNetwork, int> numberOfVotesByPlayer = new Dictionary<PlayerNetwork, int>();

    #endregion
    
    #region ScriptReferences

    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private GameManagerNetwork _gameManagerNetwork;

    #endregion

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

    public void CollectTextAnswers(string answer, PlayerNetwork player)
    {
        textAnswers.Add(player, answer);
    }

    public void StartVotePhase()
    {
        SendGamePhaseToGameManagerNetwork(Enums.GamePhase.voting);
        
        StartVotePhaseToClient();
    }

    [ObserversRpc]
    public void StartVotePhaseToClient()
    {
        _uiManager.ShowClientVotingArea();
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
