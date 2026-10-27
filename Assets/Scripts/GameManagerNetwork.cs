using FishNet.Object;
using UnityEngine;

public class GameManagerNetwork : NetworkBehaviour
{
    #region ScriptReferences

    public MiniGameManager miniGameManager;

    #endregion

    public void ReceivePlayerTextAnswer(string answer)
    {
        Debug.Log(answer);
    }
}
