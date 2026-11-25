using UnityEngine;

public class HostConnectionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _hostConnectionUIWelcome, _hostConnectionUIMenu, _hostConnectionUIOptions, _hostConnectionUICredits, _hostConnectionUIConnecting;
    [SerializeField] private GameObject _background;
    [SerializeField] private UIManager _uiManager;
    
    
    public void HideHostConnectionUICredits()
    {
        _hostConnectionUICredits.SetActive(false);
    }
    
    public void HideHostConnectionUIWelcome()
    {
        _hostConnectionUIWelcome.SetActive(false);
    }

    public void HideHostConnectionUIMenu()
    {
        _hostConnectionUIMenu.SetActive(false);
    }

    public void HideHostConnectionUIOptions()
    {
        _hostConnectionUIOptions.SetActive(false);
    }
    
    public void ShowHostConnectionUIMenu()
    {
        _uiManager.selectedUI = Enums.SelectedUI.hostConnectionUIMenu;
        _hostConnectionUIMenu.SetActive(true);
        _background.SetActive(false);
    }
    
    public void ShowHostConnectionUIWelcome()
    {
        _uiManager.selectedUI = Enums.SelectedUI.hostConnectionUIWelcome;
        _hostConnectionUIWelcome.SetActive(true);
        _background.SetActive(true);
    }

    public void ShowHostConnectionUIConnecting()
    {
        _uiManager.selectedUI = Enums.SelectedUI.hostConnectionUIConnecting;
        _hostConnectionUIConnecting.SetActive(true);
    }

    public void ShowHostConnectionUIOptions()
    {
        _uiManager.selectedUI = Enums.SelectedUI.hostConnectionUIOptions;
        _hostConnectionUIOptions.SetActive(true);
    }

    public void ShowHostConnectionUICredits()
    {
        _uiManager.selectedUI = Enums.SelectedUI.hostConnectionUICredits;
        _hostConnectionUICredits.SetActive(true);
    }
}