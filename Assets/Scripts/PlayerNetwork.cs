using FishNet.Object;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    #region ScriptReferences

    public GameManagerNetwork gameManagerNetwork;
    public PlayerData playerData;

    #endregion

    //Called when the player click on Send button
    [ServerRpc]
    public void SendPlayerTextAnswer(string answer)
    {
        gameManagerNetwork.ReceivePlayerTextAnswer(answer);
    }
}
