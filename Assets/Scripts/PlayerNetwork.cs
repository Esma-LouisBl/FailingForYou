using System;
using FishNet.Object;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    #region ScriptReferences

    public GameManagerNetwork gameManagerNetwork;
    public PlayerData playerData;

    #endregion

    #region Initilization
    
    private void Awake()
    {
        gameManagerNetwork = FindFirstObjectByType<GameManagerNetwork>();
    }

    private void Start()
    {
        GetGameManagerNetworkLocally();
    }

    public override void OnStartServer()
    {
        gameManagerNetwork.players.Add(this);
    }

    #endregion

    // The function below is useful to let the mobiles use the GameManagerNetwork
    // For example when we want to use OnClick with a button, we need to assign a GameObject from the hierarchy
    // (PlayerNetwork is not available in the hierarchy cause it is spawned later in game)
    public void GetGameManagerNetworkLocally()
    {
        if (IsOwner)
        {
            gameManagerNetwork = FindFirstObjectByType<GameManagerNetwork>();
            gameManagerNetwork.myLocalPlayer = this;
        }
    }

    [ServerRpc]
    public void SendPlayerTextAnswer(string answer)
    {
        gameManagerNetwork.ReceivePlayerTextAnswer(answer, playerData.playerNumber);
    }
}
