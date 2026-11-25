using UnityEngine;

public class ClientConnectionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _clientConnectionUICode, _clientConnectionUIQRCode, _clientConnectionUICharacterSelection, _clientConnectionUINameSelection, _clientConnectionUIWelcome, _clientConnectionUIConnecting;
    [SerializeField] UIManager _uiManager;
    
    public void HideClientConnectionUIQRCode()
    {
        _clientConnectionUIQRCode.SetActive(false);
    }
    
    public void HideClientConnectionUICode()
    {
        _clientConnectionUICode.SetActive(false);
    }
    
    public void HideClientConnectionUIWelcome()
    {
        _clientConnectionUIWelcome.SetActive(false);
    }
    
    public void HideClientConnectionUICharacterSelection()
    {
        _clientConnectionUICharacterSelection.SetActive(false);
    }
    
    public void HideClientConnectionUINameSelection()
    {
        _clientConnectionUINameSelection.SetActive(false);
    }
    
    public void ShowClientConnectionUICode()
    {
        _clientConnectionUICode.SetActive(true);
        _uiManager.selectedUI = Enums.SelectedUI.clientConnectionUICode;
    }
    
    public void ShowClientConnectionUICharacterSelection()
    {
        _clientConnectionUICharacterSelection.SetActive(true);
    }
    
    public void ShowClientConnectionUINameSelection()
    {
        _clientConnectionUINameSelection.SetActive(true);
    }
    
    public void ShowClientConnectionUIConnecting()
    {
        _clientConnectionUIConnecting.SetActive(true);
    }
}
