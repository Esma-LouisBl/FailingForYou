using System;
using System.Collections;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    #region ScriptReferences

    public GameManagerNetwork gameManagerNetwork;
    public PlayerData playerData;

    #endregion

    private int _instanceId;

    #region Initilization
    
    private void Awake()
    {
        gameManagerNetwork = FindFirstObjectByType<GameManagerNetwork>();
    }

    private void Start()
    {

        StartCoroutine(StartWithDelay());

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
    private new IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        if (IsOwner)
        {
            _instanceId = FindFirstObjectByType<NetworkManager>().ClientManager.GetInstanceID();
            Debug.Log("My instance id : " + _instanceId);
            RegisterPlayerOnServer(_instanceId);
        }
    }

    [ServerRpc]
    public void RegisterPlayerOnServer(int instanceToSend)
    {
        gameManagerNetwork.AddPlayer(this, instanceToSend);
    }

    [ObserversRpc]
    public void AssignMyPlayerNumber(int myPlayerNumber)
    {
        playerData.playerNumber = myPlayerNumber;
    }

    [ServerRpc]
    public void SendPlayerTextVote(string votedAnswer)
    {
        gameManagerNetwork.ReceivePlayerTextVote(votedAnswer, playerData.playerNumber);
    }
    
    #region CharacterSelection

    [ServerRpc]
    public void TrySelectCharacter(Enums.CharacterName characterName, int sentPlayerNumber)
    {
        gameManagerNetwork.CheckIfCharacterIsSelected(characterName, sentPlayerNumber);
        Debug.Log("Selected character: " + characterName + sentPlayerNumber);
    }
    
    #endregion
}
