using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    [SerializeField] private GameObject _hostTextAnswersRevelationUI, _answerOnHost1, _answerOnHost2, _answerOnHost3, _answerOnHost4;
    [SerializeField] private GameObject _clientQuestionUI, _clientQuestionTextualUI, _clientConfirmationFooter, _clientWaitingScreen, _clientConnectionUICode, _clientConnectionUICharacterSelection, _clientConnectionUINameSelection, _clientConnectionWelcome;
    [SerializeField] private GameObject _clientVotingArea, _clientAnswerInCaseThereAreFourPlayers;


    #endregion

    #region OtherSerializeFields

    [SerializeField] private List<GameObject> _players = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> _playerNames = new List<TextMeshProUGUI>();
    [SerializeField] private GameObject _playerCharacter1, _playerCharacter2, _playerCharacter3, _playerCharacter4;
    [SerializeField] private TextMeshProUGUI _player1Name, _player2Name, _player3Name, _player4Name;
    
    [SerializeField] private TextMeshProUGUI _hostQuestionTMP, _hostQuestionTypeTMP, _hostQuestionGenreTMP;
    [SerializeField] private TextMeshProUGUI _clientQuestionTMP, _clientQuestionTypeTMP, _clientQuestionGenreTMP, _hookSentence;
    [SerializeField]  private GameManagerNetwork _gameManagerNetwork;
    
    [SerializeField] private TextMeshProUGUI _answer1, _answer2, _answer3; //for clients
    [SerializeField] private TextMeshProUGUI _answer1Host, _answer2Host, _answer3Host, _answer4Host;
    
    //All the tokens that represent a vote for an answer on host screen
    [SerializeField] private List<GameObject> _answer1VoteTokens = new List<GameObject>();
    [SerializeField] private List<GameObject> _answer2VoteTokens = new List<GameObject>();
    [SerializeField] private List<GameObject> _answer3VoteTokens = new List<GameObject>();
    [SerializeField] private List<GameObject> _answer4VoteTokens = new List<GameObject>();


    [SerializeField] private InputActionReference _return;
    [SerializeField] private InputActionReference _space;
    [SerializeField] private InputActionReference _touchScreen;
    [SerializeField] private InputActionReference _asterisk;

    [SerializeField] private Button _deselectionButton;
    [SerializeField] private TMP_InputField _nameEntered;
    
    #endregion
    
    public Enums.SelectedUI selectedUI;
    private string _selectedCharacterName;
    private bool _canSelectName = true; //false when player selected its name (local)

    private List<int> _randomOrderOfAnswersOnScreen = new List<int>();

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
        
        // Hide some UI we don't want to see
        _hostTextAnswersRevelationUI.SetActive(false);
    }

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
                HideHostConnectionUIWelcome();
                ShowHostConnectionUIMenu();
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
                HideHostConnectionUICredits();
                ShowHostConnectionUIMenu();
                break;
            case Enums.SelectedUI.hostConnectionUIMenu :
                HideHostConnectionUIMenu();
                ShowHostConnectionUIWelcome();
                break;
            case Enums.SelectedUI.hostConnectionUIOptions :
                HideHostConnectionUIOptions();
                ShowHostConnectionUIMenu();
                break;
            case Enums.SelectedUI.hostConnectionUIWelcome :
                break;
        }
    }

    private void Asterisk(InputAction.CallbackContext obj)
    {
        FindFirstObjectByType<DebugManager>().ShowHideDebugUI();
    }

    #region returnSystem

    private void HideHostConnectionUICredits()
    {
        _hostConnectionUICredits.SetActive(false);
    }
    
    private void HideHostConnectionUIWelcome()
    {
        _hostConnectionUIWelcome.SetActive(false);
    }

    private void HideHostConnectionUIMenu()
    {
        _hostConnectionUIMenu.SetActive(false);
    }

    private void HideHostConnectionUIOptions()
    {
        _hostConnectionUIOptions.SetActive(false);
    }
    
    private void ShowHostConnectionUIMenu()
    {
        selectedUI = Enums.SelectedUI.hostConnectionUIMenu;
        _hostConnectionUIMenu.SetActive(true);
    }
    
    private void ShowHostConnectionUIWelcome()
    {
        selectedUI = Enums.SelectedUI.hostConnectionUIWelcome;
        _hostConnectionUIWelcome.SetActive(true);
    }

    #endregion
    
    #endregion

    public void SwitchOnCharacterSelectionUI()
    {
        _clientConnectionUICode.SetActive(false);
        _clientConnectionUICharacterSelection.SetActive(true);
    }

    private void SwitchOnCodeUI()
    {
        _clientConnectionWelcome.SetActive(false);
        _clientConnectionUICode.SetActive(true);
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
        _hookSentence.text = "À vos plumes !";
        
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

    public void ShowHostVotingPhase(bool fourPlayers, string ans1, string ans2, string ans3, string ans4)
    {
        _hostMainUI.SetActive(false);
        _answerOnHost1.SetActive(false);
        _answerOnHost2.SetActive(false);
        _answerOnHost3.SetActive(false);
        _answerOnHost4.SetActive(false);
        
        //Part to randomize the order of answers
        List<string> answers = new List<string>(); //List of the answers ordered by player
        answers.Add("");
        answers.Add(ans1);
        answers.Add(ans2);
        answers.Add(ans3);
        if (fourPlayers)
        {
            answers.Add(ans4);
        }
        else
        {
            answers.Add("");
        }

        for (int i = 0; i < 4; i++)
        {
            _randomOrderOfAnswersOnScreen.Add(0);   //List to see at which player belongs the answers in the order they are displayed on host screen
        }

        List<int> answerToAssign = new List<int>(); //Temporary list that repertories the answers which hasn't yet been assigned randomly
        answerToAssign.Add(1);
        answerToAssign.Add(2);
        answerToAssign.Add(3);
        if (fourPlayers)
        {
            answerToAssign.Add(4);
        }
        for (int i = 1; i < _randomOrderOfAnswersOnScreen.Count; i++)
        {
            int r = Random.Range(0, answerToAssign.Count);
            _randomOrderOfAnswersOnScreen[i] = answerToAssign[r];
            answerToAssign.RemoveAt(r);
        }

        _answer1Host.text = answers[_randomOrderOfAnswersOnScreen[1]];
        _answer2Host.text = answers[_randomOrderOfAnswersOnScreen[2]];
        _answer3Host.text = answers[_randomOrderOfAnswersOnScreen[3]];
        if (fourPlayers)
        {
            _answer4Host.text = answers[_randomOrderOfAnswersOnScreen[4]];
        }
        
        _hostTextAnswersRevelationUI.SetActive(true);
        StartCoroutine(LetAnswersAppear(fourPlayers));
    }

    private IEnumerator LetAnswersAppear(bool fourPlayers)
    {
        yield return new WaitForSeconds(1f);
        _answerOnHost1.SetActive(true);
        yield return new WaitForSeconds(2f);
        _answerOnHost2.SetActive(true);
        yield return new WaitForSeconds(2f);
        _answerOnHost3.SetActive(true);
        if (!fourPlayers) yield break;
        yield return new WaitForSeconds(2f);
        _answerOnHost4.SetActive(true);
    }
    public void ShowClientVotingArea(string answer1, string answer2, string answer3)
    {
        //Part to randomize the order of answers
        List<string> answers = new List<string>();
        answers.Add(answer1);
        answers.Add(answer2);
        if (answer3 != "")  //check if there is a third answer => 4 players
        {answers.Add(answer3);}
        
        int r = Random.Range(0, answers.Count);
        _answer1.text = answers[r];
        answers.Remove(answers[r]);
        r = Random.Range(0, answers.Count);
        _answer2.text = answers[r];
        answers.Remove(answers[r]);
        if (answer3 != "")
        {_answer3.text = answers[0];}
        else
        {
            _clientAnswerInCaseThereAreFourPlayers.SetActive(false);
        }
        
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
    
    public void ShowHostMainUIWhilePlayersAreVoting()
    {
        _hookSentence.text = "Votez pour la meilleure réponse !";
        _hostMainUI.SetActive(true);
        _hostTextAnswersRevelationUI.SetActive(false);
    }

    public void ShowMiniGameWinner(int answer1Votes, int answer2Votes, int answer3Votes, int answer4Votes)
    {
        _hostTextAnswersRevelationUI.SetActive(true);
        _hostMainUI.SetActive(false);
    
        StartCoroutine(ShowTokens(answer1Votes, answer2Votes, answer3Votes, answer4Votes));
    }

    private IEnumerator ShowTokens(int answer1Votes, int answer2Votes, int answer3Votes, int answer4Votes)
    {
        List<int> votes = new List<int>();
        votes.Add(0);
        votes.Add(answer1Votes);
        votes.Add(answer2Votes);
        votes.Add(answer3Votes);
        votes.Add(answer4Votes);
        
        if (votes[_randomOrderOfAnswersOnScreen[1]] > 0)    //check if the answer on top left on screen has votes
        {
            for (int i = 0; i < votes[_randomOrderOfAnswersOnScreen[1]]; i++)
            {
                _answer1VoteTokens[i].SetActive(true);
                yield return new WaitForSeconds(1f);
            }
        }
        if (votes[_randomOrderOfAnswersOnScreen[2]] > 0)    //check if the answer on top right on screen has votes
        {
            for (int i = 0; i < votes[_randomOrderOfAnswersOnScreen[2]]; i++)
            {
                _answer2VoteTokens[i].SetActive(true);
                yield return new WaitForSeconds(1f);
            }
        }
        if (votes[_randomOrderOfAnswersOnScreen[3]] > 0)    //check if the answer on bottom left on screen has votes
        {
            for (int i = 0; i < votes[_randomOrderOfAnswersOnScreen[3]]; i++)
            {
                _answer3VoteTokens[i].SetActive(true);
                yield return new WaitForSeconds(1f);
            }
        }
        if (_randomOrderOfAnswersOnScreen.Count > 4)
        {
            if (votes[_randomOrderOfAnswersOnScreen[4]] > 0)    //check if the answer on bottom right on screen has votes
            {
                for (int i = 0; i < votes[_randomOrderOfAnswersOnScreen[4]]; i++)
                {
                    _answer4VoteTokens[i].SetActive(true);
                    yield return new WaitForSeconds(1f);
                }
            }
        }
    } 
}
