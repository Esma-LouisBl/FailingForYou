using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    #region UI
    
    [SerializeField] private GameObject _hostUI, _clientMainUI;
    [SerializeField] private GameObject _hostConnectionUI, _clientConnectionUI;
    [SerializeField] private GameObject _hostConnectionUIWelcome, _hostConnectionUIMenu, _hostConnectionUIOptions, _hostConnectionUICredits;
    [SerializeField] private GameObject _hostQuestionUI;
    [SerializeField] private GameObject _clientQuestionUI, _clientQuestionTextualUI, _clientConfirmationFooter, _clientWaitingScreen;
    [SerializeField] private GameObject _clientVotingArea;
        
    #endregion

    #region OtherSerializeFields

    [SerializeField] private List<GameObject> _players = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> _playerNames = new List<TextMeshProUGUI>();
    [SerializeField] private GameObject _playerCharacter1, _playerCharacter2, _playerCharacter3, _playerCharacter4;
    [SerializeField] private TextMeshProUGUI _player1Name, _player2Name, _player3Name, _player4Name;
    
    [SerializeField] private TextMeshProUGUI _hostQuestionTMP;
    [SerializeField] private TextMeshProUGUI _clientQuestionTMP, _clientQuestionTypeTMP, _clientQuestionGenreTMP;
    [SerializeField]  private GameManagerNetwork _gameManagerNetwork;

    [SerializeField] private InputActionReference _return;
    
    #endregion
    
    public Enums.SelectedUI selectedUI;

    
    private void Start()
    {
        // setting lists of players and their names
        _players.Add(_playerCharacter1);
        _players.Add(_playerCharacter2);
        _players.Add(_playerCharacter3);
        _players.Add(_playerCharacter4);
        
        _playerNames.Add(_player1Name);
        _playerNames.Add(_player2Name);
        _playerNames.Add(_player3Name);
        _playerNames.Add(_player4Name);
    }

    #region returnSystem
    
    private void OnEnable()
    {
        _return.action.started += Return;
    }

    private void Return(InputAction.CallbackContext obj)
    {
        Debug.Log("Return");
        switch (selectedUI)
        {
            case Enums.SelectedUI.connectionUIConnecting :
                //not inplemented
                break;
            case Enums.SelectedUI.connectionUICredits :
                HideConnectionUICredits();
                ShowConnectionUIMenu();
                break;
            case Enums.SelectedUI.connectionUIMenu :
                HideConnectionUIMenu();
                ShowConnectionUIWelcome();
                break;
            case Enums.SelectedUI.connectionUIOptions :
                HideConnectionUIOptions();
                ShowConnectionUIMenu();
                break;
            case Enums.SelectedUI.connectionUIWelcome :
                break;
        }
    }

    private void OnDisable()
    {
        _return.action.started -= Return;
    }

    private void HideConnectionUICredits()
    {
        _hostConnectionUICredits.SetActive(false);
    }

    private void HideConnectionUIMenu()
    {
        _hostConnectionUIMenu.SetActive(false);
    }

    private void HideConnectionUIOptions()
    {
        _hostConnectionUIOptions.SetActive(false);
    }
    
    private void ShowConnectionUIMenu()
    {
        selectedUI = Enums.SelectedUI.connectionUIMenu;
        _hostConnectionUIMenu.SetActive(true);
    }
    
    private void ShowConnectionUIWelcome()
    {
        selectedUI = Enums.SelectedUI.connectionUIWelcome;
        _hostConnectionUIWelcome.SetActive(true);
    }
    
    #endregion
    
    public void ShowConnectionUI()
    {
        _hostConnectionUI.SetActive(true);
        _clientConnectionUI.SetActive(true);
    }

    public void HideConnectionUI()
    {
        _hostConnectionUI.SetActive(false);
        _clientConnectionUI.SetActive(false);
    }
    
    public void UpdateServerConnectionUI()
    {
        for (int i = 0; i < _gameManagerNetwork.players.Count; i++)
        {
            if (_gameManagerNetwork.players[i] != null)
            {
                _players[i].SetActive(false);
                if (_gameManagerNetwork.players[i].playerData.playerName != "")
                {
                    _playerNames[i].text = _gameManagerNetwork.players[i].playerData.playerName;
                }
                if (_gameManagerNetwork.players[i].playerData.characterName != "")
                {
                    _gameManagerNetwork.characterManager.UpdatePlayerSprite(_gameManagerNetwork.players[i].playerData.characterName, i);
                }
            }
        }

        
    }
    
    public void ShowHostUI()
    {
        _hostUI.SetActive(true);
        _clientMainUI.SetActive(false);
    }

    public void ShowClientMainUI()
    {
        _clientMainUI.SetActive(true);
        _hostUI.SetActive(false);
        
        _clientConnectionUI.SetActive(false);
    }

    public void UpdateMiniGameUIHost(MiniGameData miniGame)
    {
        _hostQuestionUI.SetActive(true);
        _hostQuestionTMP.text = miniGame.question;
    }
    public void UpdateMiniGameUIClient(MiniGameData miniGame)
    {
        HideConnectionUI();
        ShowClientMainUI();
        //assign the common parts
        _clientQuestionTypeTMP.text = miniGame.type.ToString();
        _clientQuestionGenreTMP.text = miniGame.genre.ToString();
        _clientQuestionTMP.text = miniGame.question;
        
        //assign parts depending on the mini game type
        switch (miniGame.type)  //TEXTUAL
        {
            case Enums.MiniGameType.textual:
                _clientQuestionTextualUI.SetActive(true);
                break;
        }
        
        _clientQuestionUI.SetActive(true);
    }
    
    public void ShowClientVotingArea()
    {
        _clientWaitingScreen.SetActive(false);
        _clientVotingArea.SetActive(true);
        _clientQuestionTextualUI.SetActive(false);
        _clientQuestionUI.SetActive(true);
    }
}
