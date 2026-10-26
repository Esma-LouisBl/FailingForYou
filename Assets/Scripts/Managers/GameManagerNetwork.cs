using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class GameManagerNetwork : NetworkBehaviour
{
    #region Variables

    public List<PlayerData> players;

    #endregion

    #region ScriptReferences

    public MiniGameManager miniGameManager;

    #endregion

    public void ReceivePlayerTextAnswer(string answer)
    {
        
    }
}
