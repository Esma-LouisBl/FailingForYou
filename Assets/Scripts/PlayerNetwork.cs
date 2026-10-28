using System;
using FishNet.Object;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    #region ScriptReferences

    public GameManagerNetwork gameManagerNetwork;
    public DataManager dataManager;

    #endregion

    private void Start()
    {
        gameManagerNetwork = FindFirstObjectByType<GameManagerNetwork>();
    }

    //This function is called by the "send" button
    [ServerRpc]
    public void SendPlayerTextAnswer(string answer)
    {
        gameManagerNetwork.ReceivePlayerTextAnswer(answer);
    }
}
