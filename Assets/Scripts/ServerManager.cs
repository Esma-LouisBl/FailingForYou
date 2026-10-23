using FishNet.Managing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework.Constraints;
using ZXing;
using ZXing.QrCode;

public class ServerManager : MonoBehaviour
{
    //NetworkManager reference
    [SerializeField]
    private NetworkManager _networkManager;

    //UI
    [SerializeField]
    private TextMeshProUGUI _codeTextUI;
    [SerializeField]
    private TMP_InputField _writtenAdress;
    [SerializeField]
    private RawImage _cameraZone, _qrCodeImage;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter;
    [SerializeField]
    private RectTransform _scanZone;
    [SerializeField]
    private GameObject _mobileCanvas, _desktopCanvas, _serverCanvas;
    
    //Private
    private bool _isCamAvailable, _canTryScanning = true;
    private WebCamTexture _cameraTexture;
    private Texture2D _storeEncodedTexture;
    
    void Start()
    {
        //Ensures the right canvas is enable, and others are not
        _mobileCanvas.SetActive(false);
        _desktopCanvas.SetActive(false);
        _serverCanvas.SetActive(true);
        
        _storeEncodedTexture = new Texture2D(256, 256);
        
        //Check the device and reacts accordingly
        if (SystemInfo.deviceType == DeviceType.Handheld) //Handheld or Desktop in our case
        {
            //_serverCanvas.SetActive(false);
            //_mobileCanvas.SetActive(true);
            SetUpCamera();
            CreateServer();
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //_serverCanvas.SetActive(false);
            //_desktopCanvas.SetActive(true);
            //CreateServer();
            Debug.Log("Server should start");
        }
        else
        {
            Debug.LogError("Unsupported Device Type");
        }
    }
    
    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld) //Handheld or Desktop in our case
        {
            UpdateCameraRender();
            Scan();
        }
    }
    
    //Start the server connection and show up its IP address at the screen with a string and a QR code
    public void CreateServer()
    {
        ///////temporary
        _serverCanvas.SetActive(false);
        _desktopCanvas.SetActive(true);
        
        _networkManager.ServerManager.StartConnection();
        if (Dns.GetHostAddresses(Dns.GetHostName())[1].ToString().Length >= 16) //Ipv4 addresses contain 15 characters including "." and numbers
        {
            _codeTextUI.text = Dns.GetHostAddresses(Dns.GetHostName())[3].ToString(); //0 for Ipv6
            EncodeTextToQRCode(Dns.GetHostAddresses(Dns.GetHostName())[3].ToString());
        }
        else
        {
            _codeTextUI.text = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(); //0 for Ipv6
            EncodeTextToQRCode(Dns.GetHostAddresses(Dns.GetHostName())[1].ToString());
        }
    }

    //Try to connect as a client
    public void CreateClient()
    {
        ///////temporary
        _serverCanvas.SetActive(false);
        _mobileCanvas.SetActive(true);
        
        if (_writtenAdress.text != "")
        {
            _networkManager.ClientManager.StartConnection(_writtenAdress.text);
        }
        //////////On connection, call GameManagerNetwork to update its PlayerData and get a new Id
    }
    
    //SetUpCamera for the QR code Scan
    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            _isCamAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                _cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
            }
            /*
            else
            {
                return;
            }
            */
        }
        
        _cameraTexture.Play();
        _cameraZone.texture = _cameraTexture;
        _isCamAvailable = true;
    }
    
    //Encode a text to create a QR code
    private Color32[] Encode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width,
            }
        };
        return writer.Write(textForEncoding);
    }
    
    //Change the image texture into a QR code
    public void EncodeTextToQRCode(string textWritten)
    {
        Color32[] _convertPixelToTexture = Encode(textWritten, _storeEncodedTexture.width, _storeEncodedTexture.height);
        _storeEncodedTexture.SetPixels32(_convertPixelToTexture);
        _storeEncodedTexture.Apply();
        
        _qrCodeImage.texture = _storeEncodedTexture;
    }
    
    //Update the camera view
    private void UpdateCameraRender()
    {
        if (!_isCamAvailable)
        {
            return;
        }
        float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;
        
        int orientation = _cameraTexture.videoRotationAngle;
        _cameraZone.rectTransform.localEulerAngles = new Vector3(0, 0, -1*_cameraTexture.videoRotationAngle); //z was orientation
    }
    
    //Use the camera of the phone to try to catch a QR code and connect if an IP is found
    private void Scan()
    {
        if (!_canTryScanning)
        {
            return;
        }
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
            if (result != null)
            {
                if (result.Text != "")
                {
                    _canTryScanning = false;
                    ConnectionDelay();
                    _writtenAdress.text = result.Text;
                    CreateClient();
                }
            }
        }
        catch
        {
            
        }
    }

    private new IEnumerator ConnectionDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _canTryScanning = true;
    }
}

// Transport Path : Assets/Plugins/FishNet/Runtime/Transporting/Transports/Tugboat/MyTransport.cs
