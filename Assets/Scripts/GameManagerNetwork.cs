using System.Collections.Generic;
using FishNet.Object;
using TMPro;
using UnityEngine;

public class GameManagerNetwork : NetworkBehaviour
{
    #region ScriptReferences

    public MiniGameManager miniGameManager;

    #endregion

    #region Variables

    public List<PlayerData> players;

    [SerializeField]    //Required for function ConfirmTextAnswer
    private TextMeshProUGUI _playerTextAnswer;

    #endregion

    public void ConfirmTextAnswer()
    {
        string answer = _playerTextAnswer.text;
        PlayerNetwork player = FindFirstObjectByType<PlayerNetwork>();
        player.SendPlayerTextAnswer(answer);
    }
    
    public void ReceivePlayerTextAnswer(string answer)
    {
        Debug.Log(answer);
    }
}
