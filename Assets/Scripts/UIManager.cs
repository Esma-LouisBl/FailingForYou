using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    #region UI
    
    [SerializeField] private GameObject _hostUI, _clientMainUI, _hostMainUI;
    [SerializeField] private GameObject _hostConnectionUI, _clientConnectionUI;
    [SerializeField] private GameObject _hostConnectionUIWelcome, _hostConnectionUIMenu, _hostConnectionUIOptions, _hostConnectionUICredits;
    [SerializeField] private GameObject _hostQuestionUI;
    [SerializeField] private GameObject _clientQuestionUI, _clientQuestionTextualUI, _clientConfirmationFooter, _clientWaitingScreen;

    [SerializeField] private GameObject _hostTextAnswersRevelationUI, _answerOnHost1, _answerOnHost2, _answerOnHost3, _answerOnHost4;
    [SerializeField] private GameObject _clientVotingArea, _clientAnswerInCaseThereAreFourPlayers;
        
         
    #endregion

    #region OtherSerializeFields

    [SerializeField] private List<GameObject> _players = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> _playerNames = new List<TextMeshProUGUI>();
    [SerializeField] private GameObject _playerCharacter1, _playerCharacter2, _playerCharacter3, _playerCharacter4;
    [SerializeField] private TextMeshProUGUI _player1Name, _player2Name, _player3Name, _player4Name;
    
    [SerializeField] private TextMeshProUGUI _hostQuestionTMP, _hostQuestionTypeTMP, _hostQuestionGenreTMP;
    [SerializeField] private TextMeshProUGUI _clientQuestionTMP, _clientQuestionTypeTMP, _clientQuestionGenreTMP, _hookSentence;
    [SerializeField] private GameManagerNetwork _gameManagerNetwork;
    
    [SerializeField] private TextMeshProUGUI _answer1, _answer2, _answer3; //for clients
    [SerializeField] private TextMeshProUGUI _answer1Host, _answer2Host, _answer3Host, _answer4Host;
    
    //All the tokens that represent a vote for an answer on host screen
    [SerializeField] private List<GameObject> _answer1VoteTokens = new List<GameObject>();
    [SerializeField] private List<GameObject> _answer2VoteTokens = new List<GameObject>();
    [SerializeField] private List<GameObject> _answer3VoteTokens = new List<GameObject>();
    [SerializeField] private List<GameObject> _answer4VoteTokens = new List<GameObject>();


    [SerializeField] private InputActionReference _return;
    
    #endregion
    
    public Enums.SelectedUI selectedUI;

    private List<int> _randomOrderOfAnswersOnScreen;
    
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
                //not implemented
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

        Debug.Log(_randomOrderOfAnswersOnScreen[1] + " a la réponse en haut à gauche");
        Debug.Log(_randomOrderOfAnswersOnScreen[2] + " a la réponse en haut à droite");
        Debug.Log(_randomOrderOfAnswersOnScreen[3] + " a la réponse en bas à gauche");
        
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
            Debug.Log($"le joueur en haut à gauche a eu {votes[_randomOrderOfAnswersOnScreen[1]]} votes");
            for (int i = 0; i < votes[_randomOrderOfAnswersOnScreen[1]]; i++)
            {
                _answer1VoteTokens[i].SetActive(true);
                yield return new WaitForSeconds(1f);
            }
        }
        if (votes[_randomOrderOfAnswersOnScreen[2]] > 0)    //check if the answer on top right on screen has votes
        {
            Debug.Log($"le joueur en haut à droite a eu {votes[_randomOrderOfAnswersOnScreen[2]]} votes");
            for (int i = 0; i < votes[_randomOrderOfAnswersOnScreen[2]]; i++)
            {
                _answer2VoteTokens[i].SetActive(true);
                yield return new WaitForSeconds(1f);
            }
        }
        if (votes[_randomOrderOfAnswersOnScreen[3]] > 0)    //check if the answer on bottom left on screen has votes
        {
            Debug.Log($"le joueur en bas à gauche a eu {votes[_randomOrderOfAnswersOnScreen[3]]} votes");
            for (int i = 0; i < votes[_randomOrderOfAnswersOnScreen[3]]; i++)
            {
                _answer3VoteTokens[i].SetActive(true);
                yield return new WaitForSeconds(1f);
            }
        }
        if (_randomOrderOfAnswersOnScreen.Count>4)
        {
            if (votes[_randomOrderOfAnswersOnScreen[4]] > 0)    //check if the answer on bottom right on screen has votes
            {
                Debug.Log($"le joueur en bas à droite a eu {votes[_randomOrderOfAnswersOnScreen[4]]} votes");
                for (int i = 0; i < votes[_randomOrderOfAnswersOnScreen[4]]; i++)
                {
                    _answer4VoteTokens[i].SetActive(true);
                    yield return new WaitForSeconds(1f);
                }
            }   
        }
    }
}
