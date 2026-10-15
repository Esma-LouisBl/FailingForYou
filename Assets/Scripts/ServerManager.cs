using FishNet.Managing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;

public class ServerManager : MonoBehaviour
{
    [SerializeField]
    private NetworkManager _networkManager;

    [SerializeField] private TextMeshProUGUI _IPV6Text;
    [SerializeField] private TMP_InputField _WrittenAdress;
    
    IPAddress ipv6 = null;
    
    public void CreateServer()
    {
        _networkManager.ServerManager.StartConnection();
        _IPV6Text.text = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
        //System.Net.IPAddress.TryParse(_IPV6Text.text, out ipv6);
        //_IPV6Text.text = ipv6.ToString();
    }

    public void CreateClient()
    {
        _networkManager.ClientManager.StartConnection(_WrittenAdress.text);
    }
}

// Transport Path : Assets/Plugins/FishNet/Runtime/Transporting/Transports/Tugboat/MyTransport.cs
