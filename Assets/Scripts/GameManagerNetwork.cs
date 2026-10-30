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

    private int _voteAnswerNumber;

    #endregion

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
                break;
        }
    }
    
    public void ConfirmTextAnswer()
    {
        string answer = _playerTextAnswer.text;
        myLocalPlayer.SendPlayerTextAnswer(answer);
    }

    public void ConfirmVote()
    {
        
    }

    public void SetVote(int answerNumber)
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
}
