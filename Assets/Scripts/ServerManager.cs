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

    [SerializeField] private TextMeshProUGUI _IPText;
    [SerializeField] private TMP_InputField _WrittenAdress;
    
    IPAddress ipv4 = null;
    
    public void CreateServer()
    {
        _networkManager.ServerManager.StartConnection();
        if (Dns.GetHostAddresses(Dns.GetHostName())[1].ToString().Length >= 14)
        {
            _IPText.text = Dns.GetHostAddresses(Dns.GetHostName())[3].ToString(); //0 for Ipv6
        }
        else
        {
            _IPText.text = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(); //0 for Ipv6
        }
    }

    public void CreateClient()
    {
        if (_WrittenAdress.text != "")
        {
            _networkManager.ClientManager.StartConnection(_WrittenAdress.text);
        }
    }
}

// Transport Path : Assets/Plugins/FishNet/Runtime/Transporting/Transports/Tugboat/MyTransport.cs
