using System.Collections.Generic;
using FishNet.Object;
using TMPro;
using UnityEngine;

public class GameManagerNetwork : NetworkBehaviour
{
    #region ScriptReferences

    public MiniGameManager miniGameManager;
    public CharacterManager characterManager;

    #endregion

    #region Variables

    public List<PlayerNetwork> players;
    public PlayerNetwork myLocalPlayer;
    
    public Enums.GamePhase gamePhase;
    
    [SerializeField]    //Required for function ConfirmTextAnswer
    private TextMeshProUGUI _playerTextAnswer;
    
    [SerializeField]    //Required for function ConfirmVote
    private TextMeshProUGUI _answer1, _answer2, _answer3;
    private int _voteAnswerNumber;
    
    private Dictionary<int, int> _instanceIdRepertory = new Dictionary<int, int>();

    #endregion

    public void AddPlayer(PlayerNetwork playerToAdd, int instanceIdReceived)
    {
        players.Add(playerToAdd);
        _instanceIdRepertory.Add(instanceIdReceived, _instanceIdRepertory.Count+1);
        
        //Debug
        // foreach (KeyValuePair<int, int> entry in _instanceIdRepertory)
        // {
        //     Debug.Log(entry.Key + " : " + entry.Value);
        // }
        
        playerToAdd.AssignMyPlayerNumber(_instanceIdRepertory[instanceIdReceived]);
    }
    
    public void UpdateGamePhaseOnServer(Enums.GamePhase phase)
    {
        gamePhase = phase;
        UpdateGamePhaseLocally(gamePhase);
    }

    [ObserversRpc]
    public void UpdateGamePhaseLocally(Enums.GamePhase phase)
    {
        gamePhase = phase;
    }
    

    #region SendButton

    // Used by a LOCAL GameManagerNetwork
    // When player click on the Send Button
    // The function that is trigger changes according to the current game phase
    public void SendButton()
    {
        switch (gamePhase)
        {
            case Enums.GamePhase.answeringQuestion:
                ConfirmTextAnswer();
                break;
            case Enums.GamePhase.voting:
                ConfirmVote();
                break;
        }
    }
    
    public void ConfirmTextAnswer()
    {
        string answer = _playerTextAnswer.text;
        myLocalPlayer.SendPlayerTextAnswer(answer, myLocalPlayer.playerData.playerNumber);
    }

    public void ConfirmVote()
    {
        string votedAnswer = "";
        switch (_voteAnswerNumber)
        {
            case 1:
                votedAnswer = _answer1.text;
                break;
            case 2:
                votedAnswer = _answer2.text;
                break;
            case 3:
                votedAnswer = _answer3.text;
                break;
        }
        myLocalPlayer.SendPlayerTextVote(votedAnswer);
    }

    public void SetVote(int answerNumber)   //called by the three answer buttons
    {
        _voteAnswerNumber = answerNumber;
    }
    #endregion
    
    
    public void ReceivePlayerTextAnswer(string answer, int playerNumber)
    {
        miniGameManager.CollectTextAnswers(answer, playerNumber);
        
        //If all answers are collected :
        if (miniGameManager.collectedAnswers == players.Count)
        {
            miniGameManager.StartVotePhase();
        }
    }

    public void ReceivePlayerTextVote(string votedAnswer, int playerNumber)
    {
        miniGameManager.CollectTextVotes(votedAnswer, playerNumber);
        
        //If all votes are collected :
        if (miniGameManager.collectedVotes == players.Count)
        {
            miniGameManager.MiniGameWinnerElection();
        }
    }
}
