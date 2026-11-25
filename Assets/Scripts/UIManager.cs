using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region UI
    
    [SerializeField] private GameObject _hostUI, _clientMainUI, _hostMainUI;
    [SerializeField] private GameObject _hostConnectionUI, _clientConnectionUI;
    

    [SerializeField]
    private GameObject _clientWaitingScreen;

    [Space(20)]


    #endregion

    #region OtherSerializeFields

    
    [SerializeField]  private GameManagerNetwork _gameManagerNetwork;
    


    [SerializeField] private InputActionReference _return;
    [SerializeField] private InputActionReference _space;
    [SerializeField] private InputActionReference _touchScreen;
    [SerializeField] private InputActionReference _asterisk;

    [SerializeField] private Button _deselectionButton;
    [SerializeField] private TMP_InputField _nameEntered;
    
    #endregion

    #region Managers
    
    [SerializeField] private HostConnectionUIManager  _hostConnectionUIManager;
    [SerializeField] private ClientConnectionUIManager _clientConnectionUIManager;
    [SerializeField] private MiniGameUIManager _miniGameUIManager;
    
    #endregion
    
    public Enums.SelectedUI selectedUI;
    private string _selectedCharacterName;
    private bool _canSelectName = true; //false when player selected its name (local)



    #region inputSystem
    
    private void OnEnable()
    {
        _return.action.started += Return;
        _space.action.started += Space;
        _touchScreen.action.started += TouchScreen;
        _asterisk.action.started += Asterisk;
    }
    
    private void OnDisable()
    {
        _return.action.started -= Return;
        _space.action.started -= Space;
        _touchScreen.action.started -= TouchScreen;
        _asterisk.action.started -= Asterisk;
    }
    
    private void Space(InputAction.CallbackContext obj)
    {
        switch (selectedUI)
        {
            case Enums.SelectedUI.hostConnectionUIWelcome :
                _hostConnectionUIManager.HideHostConnectionUIWelcome();
                _hostConnectionUIManager.ShowHostConnectionUIMenu();
                break;
            case Enums.SelectedUI.clientConnectionUIWelcome :
                SwitchOnCodeUI();
                break;
        }
    }

    private void TouchScreen(InputAction.CallbackContext obj)
    {
        switch (selectedUI)
        {
            case Enums.SelectedUI.clientConnectionUIWelcome :
                SwitchOnCodeUI();
                break;
        }
    }

    private void Return(InputAction.CallbackContext obj)
    {
        switch (selectedUI)
        {
            case Enums.SelectedUI.hostConnectionUIConnecting :
                //not implemented
                break;
            case Enums.SelectedUI.hostConnectionUICredits :
                _hostConnectionUIManager.HideHostConnectionUICredits();
                _hostConnectionUIManager.ShowHostConnectionUIMenu();
                break;
            case Enums.SelectedUI.hostConnectionUIMenu :
                _hostConnectionUIManager.HideHostConnectionUIMenu();
                _hostConnectionUIManager.ShowHostConnectionUIWelcome();
                break;
            case Enums.SelectedUI.hostConnectionUIOptions :
                _hostConnectionUIManager.HideHostConnectionUIOptions();
                _hostConnectionUIManager.ShowHostConnectionUIMenu();
                break;
            case Enums.SelectedUI.hostConnectionUIWelcome :
                break;
        }
    }

    private void Asterisk(InputAction.CallbackContext obj)
    {
        FindFirstObjectByType<DebugManager>().ShowHideDebugUI();
    }
    
    #endregion

    public void SwitchOnCharacterSelectionUI()
    {
        _clientConnectionUIManager.HideClientConnectionUIQRCode();
        _clientConnectionUIManager.HideClientConnectionUICode();
        _clientConnectionUIManager.ShowClientConnectionUICharacterSelection();
    }
    
    public void SwitchOnHostConnectingUI()
    {
        _gameManagerNetwork.sceneManager.UpdateScene(Enums.MiniGamePlace.tv, Enums.MiniGameGenre.none, Enums.MiniGameType.none);
        _gameManagerNetwork.sceneManager.ShowConnectingAssets();
        _gameManagerNetwork.sceneManager.SwitchOnCamera(2);

        _hostConnectionUIManager.HideHostConnectionUIMenu();
        _hostConnectionUIManager.ShowHostConnectionUIConnecting();
    }
    
    public void SwitchOnHostOptionsUIFromMenu()
    {
        _hostConnectionUIManager.HideHostConnectionUIMenu();
        _hostConnectionUIManager.ShowHostConnectionUIOptions();
        selectedUI = Enums.SelectedUI.hostConnectionUIOptions;
    }

    public void SwitchOnCreditsUI()
    {
        _hostConnectionUIManager.HideHostConnectionUIMenu();
        _hostConnectionUIManager.ShowHostConnectionUICredits();
        selectedUI =  Enums.SelectedUI.hostConnectionUICredits;
    }

    private void SwitchOnCodeUI()
    {
        _clientConnectionUIManager.HideClientConnectionUIWelcome();
        _clientConnectionUIManager.ShowClientConnectionUICode();
    }
    
    public void SwitchOnNameSelectionUI()
    {
        _clientConnectionUIManager.HideClientConnectionUICharacterSelection();
        _clientConnectionUIManager.ShowClientConnectionUINameSelection();  
    }

    public void SwitchOnClientConnectingUI()
    {
        _clientConnectionUIManager.HideClientConnectionUINameSelection();
        _clientConnectionUIManager.ShowClientConnectionUIConnecting();
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

    public void LeaveGame()
    {
        Application.Quit();
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

    #region MiniGame

    public void UpdateTimer(int time)
    {
        _miniGameUIManager.UpdateTimer(time);
    }
    public void UpdateMiniGameUIHost(MiniGameData miniGame)
    {
        _miniGameUIManager.UpdateMiniGameUIHost(miniGame);
        _hostMainUI.SetActive(true);
    }
    public void UpdateMiniGameUIClient(MiniGameData miniGame)
    {
        HideConnectionUI();
        ShowClientMainUI();
        _miniGameUIManager.UpdateMiniGameUIClient(miniGame);
        _clientWaitingScreen.SetActive(false);
    }

    public void ShowHostVotingPhase(bool fourPlayers, string ans1, string ans2, string ans3, string ans4)
    {
        _hostMainUI.SetActive(false);
        _miniGameUIManager.ShowHostVotingPhase(fourPlayers, ans1, ans2, ans3, ans4);
    }

    public void ShowClientVotingArea(string answer1, string answer2, string answer3)
    {
        _miniGameUIManager.ShowClientVotingArea(answer1, answer2, answer3);
        _clientWaitingScreen.SetActive(false);
    }
    public void ShowHostMainUIWhilePlayersAreVoting()
    {
        _miniGameUIManager.ShowHostMainUIWhilePlayersAreVoting();
        _hostMainUI.SetActive(true);
        // _hostTextAnswersRevelationUI.SetActive(false);
    }

    public void ShowMiniGameWinner(int answer1Votes, int answer2Votes, int answer3Votes, int answer4Votes)
    {
        _miniGameUIManager.ShowMiniGameWinner(answer1Votes, answer2Votes, answer3Votes, answer4Votes);
        _hostMainUI.SetActive(false);
    }

    #region Tie

    public void ShowTieOnHost(List<int> playersInTie, string question)
    {
        _miniGameUIManager.ShowTieOnHost(playersInTie, question);
        _hostMainUI.SetActive(true);
    }
    public void ShowTieOnClient(string tieQuestion)
    {
        _miniGameUIManager.ShowTieOnClient(tieQuestion);
        _clientWaitingScreen.SetActive(false);
    }

    public void ShowTieWinner(int winnerNumber)
    {
        _miniGameUIManager.ShowTieWinner(winnerNumber, _gameManagerNetwork.miniGameManager.playersInTie,
            _gameManagerNetwork.miniGameManager.collectedTieAnswers, _gameManagerNetwork.miniGameManager.currentTieQuestion.answer);
    }
    #endregion
    

    #endregion

    public void ShowWaitingScreenAndShutDownEverythingElse()
    {
        _clientWaitingScreen.SetActive(true);
        _miniGameUIManager.ShowWaitingScreenAndShutDownEverythingElse();
    }

    #region CharacterSelection

    public void SetSelectedCharacter(string characterName) //Set a selected character when click on Button
    {
        _selectedCharacterName = characterName;
    }

    public void OnSelectCharacter() //When confirmed
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
            case "Kame" :
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

    public void OnSabotageClicked()
    {
        _gameManagerNetwork.myLocalPlayer.TryToSendASabotage(1);
    }
    
    #region LaunchGame

    public void OnLaunchGame()
    {
        _gameManagerNetwork.myLocalPlayer.SwitchOnServerRpc();
    }
    
    #endregion
}
