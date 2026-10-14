using FishNet.Managing;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    [SerializeField]
    private NetworkManager _networkManager;
    
    public void CreateServer()
    {
        _networkManager.ServerManager.StartConnection();
    }

    public void CreateClient()
    {
        _networkManager.ClientManager.StartConnection();
    }
}
