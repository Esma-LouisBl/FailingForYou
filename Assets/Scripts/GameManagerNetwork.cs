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

    [SerializeField]    //Required for function ConfirmTextAnswer
    private TextMeshProUGUI _playerTextAnswer;

    #endregion

    // Used by a LOCAL GameManagerNetwork
    public void ConfirmTextAnswer()
    {
        string answer = _playerTextAnswer.text;
        myLocalPlayer.SendPlayerTextAnswer(answer);
    }
    
    public void ReceivePlayerTextAnswer(string answer, PlayerNetwork player)
    {
        miniGameManager.CollectTextAnswers(answer, player);
        
        //If all answers are collected :
        if (miniGameManager.textAnswers.Count == players.Count)
        {
            miniGameManager.StartVotePhase();
        }
    }
}
