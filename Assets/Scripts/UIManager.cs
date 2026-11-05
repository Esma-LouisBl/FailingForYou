using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region UI
    
    [SerializeField] private GameObject _hostUI, _clientMainUI, _hostMainUI;
    [SerializeField] private GameObject _hostConnectionUI, _clientConnectionUI;
    [SerializeField] private GameObject _hostConnectionUIWelcome, _hostConnectionUIMenu, _hostConnectionUIOptions, _hostConnectionUICredits;
    [SerializeField] private GameObject _hostQuestionUI;
    [SerializeField] private GameObject _clientQuestionUI, _clientQuestionTextualUI, _clientConfirmationFooter, _clientWaitingScreen, _clientConnectionUICode, _clientConnectionUICharacterSelection, _clientConnectionUINameSelection;
    [SerializeField] private GameObject _clientVotingArea;
        
         
    #endregion

    #region OtherSerializeFields

    [SerializeField] private List<GameObject> _players = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> _playerNames = new List<TextMeshProUGUI>();
    [SerializeField] private GameObject _playerCharacter1, _playerCharacter2, _playerCharacter3, _playerCharacter4;
    [SerializeField] private TextMeshProUGUI _player1Name, _player2Name, _player3Name, _player4Name;
    
    [SerializeField] private TextMeshProUGUI _hostQuestionTMP, _hostQuestionTypeTMP, _hostQuestionGenreTMP;
    [SerializeField] private TextMeshProUGUI _clientQuestionTMP, _clientQuestionTypeTMP, _clientQuestionGenreTMP;
    [SerializeField] private GameManagerNetwork _gameManagerNetwork;
    
    [SerializeField] private TextMeshProUGUI _answer1, _answer2, _answer3;

    [SerializeField] private InputActionReference _return;

    [SerializeField] private Button _deselectionButton;
    [SerializeField] private TMP_InputField _nameEntered;
    
    #endregion
    
    public Enums.SelectedUI selectedUI;
    
    private string _selectedCharacterName;
    private bool _canSelectName = true; //false when player selected its name (local)
    
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

    public void SwitchOnCharacterSelectionUI()
    {
        _clientConnectionUICode.SetActive(false);
        _clientConnectionUICharacterSelection.SetActive(true);
    }
    
    public void SwitchOnNameSelectionUI()
    {
        _clientConnectionUICharacterSelection.SetActive(false);
        _clientConnectionUINameSelection.SetActive(true);
    }
    
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
                if (_gameManagerNetwork.players[i].playerData.character != Enums.CharacterName.None)
                {
                    _gameManagerNetwork.characterManager.UpdatePlayerSprite(_gameManagerNetwork.players[i].playerData.character, i);
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
        _hostQuestionTMP.text = miniGame.question;
        _hostQuestionTypeTMP.text = miniGame.type.ToString();
        _hostQuestionGenreTMP.text = miniGame.genre.ToString();
        _hostMainUI.SetActive(true);
        _hostQuestionUI.SetActive(true);
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
        
        _clientVotingArea.SetActive(false);
        _clientQuestionUI.SetActive(true);
    }
    
    public void ShowClientVotingArea(string answer1, string answer2, string answer3)
    {
        //Part to randomize the order of answers
        List<string> answers = new List<string>();
        answers.Add(answer1);
        answers.Add(answer2);
        answers.Add(answer3);
        
        int r = Random.Range(0, answers.Count);
        _answer1.text = answers[r];
        answers.Remove(answers[r]);
        r = Random.Range(0, answers.Count);
        _answer2.text = answers[r];
        answers.Remove(answers[r]);
        _answer3.text = answers[0];
        
        //What to show on screen
        _clientWaitingScreen.SetActive(false);
        _clientVotingArea.SetActive(true);
        _clientQuestionTextualUI.SetActive(false);
        _clientQuestionUI.SetActive(true);
    }

    #region CharacterSelection

    public void SetSelectedCharacter(string characterName)
    {
        _selectedCharacterName =  characterName;
    }

    public void OnSelectCharacter()
    {
        switch (_selectedCharacterName)
        {
            case "Boubou" :
                _gameManagerNetwork.myLocalPlayer.TrySelectCharacter(Enums.CharacterName.Boubou, _gameManagerNetwork.myLocalPlayer.playerData.playerNumber);
                break;
            case "Bog" :
                _gameManagerNetwork.myLocalPlayer.TrySelectCharacter(Enums.CharacterName.Bog, _gameManagerNetwork.myLocalPlayer.playerData.playerNumber);
                break;
            case "Kouc" :
                _gameManagerNetwork.myLocalPlayer.TrySelectCharacter(Enums.CharacterName.Kouc, _gameManagerNetwork.myLocalPlayer.playerData.playerNumber);
                break;
            case "Pacc" :
                _gameManagerNetwork.myLocalPlayer.TrySelectCharacter(Enums.CharacterName.Pacc, _gameManagerNetwork.myLocalPlayer.playerData.playerNumber);
                break;
            case "Pito" :
                _gameManagerNetwork.myLocalPlayer.TrySelectCharacter(Enums.CharacterName.Pito, _gameManagerNetwork.myLocalPlayer.playerData.playerNumber);
                break;
            case "Kamé" :
                _gameManagerNetwork.myLocalPlayer.TrySelectCharacter(Enums.CharacterName.Kame, _gameManagerNetwork.myLocalPlayer.playerData.playerNumber);
                break;
        }
    }

    public void ResetButtonCharacterSelection() //Deselect every buttons by selecting another
    {
        _deselectionButton.Select();
    }

    public void SelectName()
    {
        if (_nameEntered.text != "" && _canSelectName)
        {
            _canSelectName = false;
            _gameManagerNetwork.myLocalPlayer.RegisterName(_nameEntered.text, _gameManagerNetwork.myLocalPlayer.playerData.playerNumber);
            _gameManagerNetwork.myLocalPlayer.playerData.playerName = _nameEntered.text;
        }
    }
    
    #endregion
}
