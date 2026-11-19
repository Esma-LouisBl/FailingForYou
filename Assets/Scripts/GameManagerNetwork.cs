using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FishNet.Object;
using TMPro;
using UnityEngine;

public class GameManagerNetwork : NetworkBehaviour
{
    #region ScriptReferences

    public MiniGameManager miniGameManager;
    public CharacterManager characterManager;
    public UIManager uiManager;
    public SceneManager sceneManager;
    public SabotageManager sabotageManager;

    #endregion

    #region Variables

    public List<PlayerNetwork> players;
    public PlayerNetwork myLocalPlayer;
    
    public Enums.GamePhase gamePhase;
    
    [SerializeField]    //Required for function ConfirmTextAnswer & ConfirmTieAnswer
    private TextMeshProUGUI _playerTextAnswer, _playerTieAnswer;
    
    [SerializeField]    //Required for function ConfirmVote
    private TextMeshProUGUI _answer1, _answer2, _answer3;
    private int _voteAnswerNumber;

    public bool playerIsWaiting;
    
    private Dictionary<int, int> _instanceIdRepertory = new Dictionary<int, int>();
    private Dictionary<int, string> _playerNames = new Dictionary<int, string>();
    private List<Enums.CharacterName> _selectedCharacters = new List<Enums.CharacterName>();

    private bool _canSelectCharacter = true; //false when player selected its character (local)
    
    private List<int> _sabotagedPlayers = new List<int>();
    
    #region CharacterSelection
    
    [SerializeField]
    private TextMeshPro _player1NameText, _player2NameText, _player3NameText,  _player4NameText;
    [SerializeField]
    private List<Transform> _playerTransforms = new List<Transform>();
    [SerializeField]
    private GameObject _batPrefab, _bunnyPrefab, _chickenPrefab, _catPrefab, _chameleonPrefab, _axolotlPrefab;
    
    #endregion
    
    private int _rounds = 6;
    
    #endregion

    
    private int _incrementalValue = 0;

    public void PlayGame()
    {
        for (int i = 0; i <= _rounds; i++)

        {
            miniGameManager.StartMiniGame();
            if (_sabotagedPlayers.Count != 0)
            {
                foreach (int playerTargeted in _sabotagedPlayers) //Send sabotages to targeted players
                {
                    TransmitSabotage(playerTargeted);
                }
                _sabotagedPlayers.Clear(); //Clear the list after sabotages have been sent
            }

        }
    }
    
    public void AddPlayer(PlayerNetwork playerToAdd, int instanceIdReceived)
    {
        players.Add(playerToAdd);
        _incrementalValue++;
        //_instanceIdRepertory.Add(instanceIdReceived, _instanceIdRepertory.Count+1);
        
        //Debug
        // foreach (KeyValuePair<int, int> entry in _instanceIdRepertory)
        // {
        //     Debug.Log(entry.Key + " : " + entry.Value);
        // }
        
        playerToAdd.AssignMyPlayerNumber(_incrementalValue);//_instanceIdRepertory[instanceIdReceived]
    }
    
    public void UpdateGamePhaseOnServer(Enums.GamePhase phase)
    {
        gamePhase = phase;
        UpdateGamePhaseLocally(gamePhase);
    }

    [ObserversRpc]
    public void UpdateGamePhaseLocally(Enums.GamePhase phase)
    {
        gamePhase = phase;
    }
    

    #region SendButton

    public void ResetVariablesOnStartMiniGame()
    {
        _playerTextAnswer.text = "";
        _playerTieAnswer.text = "";
        _answer1.text = "";
        _answer2.text = "";
        _answer3.text = "";
        _voteAnswerNumber = 1;

        playerIsWaiting = false;
    }
    
    // Used by a LOCAL GameManagerNetwork
    // When player click on the Send Button
    // The function that is trigger changes according to the current game phase
    public void SendButton()
    {
        switch (gamePhase)
        {
            case Enums.GamePhase.answeringQuestion:
                ConfirmTextAnswer();
                break;
            case Enums.GamePhase.voting:
                ConfirmVote();
                break;
            case Enums.GamePhase.tie:
                ConfirmTieAnswer();
                break;
        }
        miniGameManager.WaitingScreenOnClient();
        playerIsWaiting = true;
    }
    
    public void ConfirmTextAnswer()
    {
        string answer = _playerTextAnswer.text;
        myLocalPlayer.SendPlayerTextAnswer(answer, myLocalPlayer.playerData.playerNumber);
    }

    public void ConfirmTieAnswer()
    {
        //need to do some shenanigans in order to convert into float without bugs
        string s = _playerTieAnswer.text;
        s = Regex.Replace(_playerTieAnswer.text, @"[^0-9.,-]", "");
        s.Replace(",", ".");
        float answer = float.Parse(s);
        
        myLocalPlayer.SendPlayerTieAnswer(answer, myLocalPlayer.playerData.playerNumber);
    }

    public void ConfirmVote()
    {
        string votedAnswer = "";
        switch (_voteAnswerNumber)
        {
            case 1:
                votedAnswer = _answer1.text;
                break;
            case 2:
                votedAnswer = _answer2.text;
                break;
            case 3:
                votedAnswer = _answer3.text;
                break;
        }
        myLocalPlayer.SendPlayerTextVote(votedAnswer);
    }

    public void SetVote(int answerNumber)   //called by the three answer buttons
    {
        _voteAnswerNumber = answerNumber;
    }
    #endregion
    
    
    public void ReceivePlayerTextAnswer(string answer, int playerNumber)
    {
        miniGameManager.CollectTextAnswers(answer, playerNumber);
        
        //If all answers are collected :
        if (miniGameManager.collectedAnswers == players.Count)
        {
            miniGameManager.StartVotePhase();
        }
    }

    public void ReceivePlayerTieAnswer(float answer, int playerNumber)
    {
        //check if the answer has already been entered by someone else
        if (miniGameManager.collectedTieAnswers.Contains(answer))
        {
            CannotReceiveAnswer(playerNumber);
        }
        else
        {
            miniGameManager.CollectTieAnswer(answer, playerNumber);
        }
    }

    [ObserversRpc]
    private void CannotReceiveAnswer(int playerNumber)
    {
        if (myLocalPlayer.playerData.playerNumber == playerNumber)
        {
            uiManager.ShowTieOnClientAfterBadAnswer();
        }
    }
    public void ReceivePlayerTextVote(string votedAnswer, int playerNumber)
    {
        miniGameManager.CollectTextVotes(votedAnswer, playerNumber);
        
        //If all votes are collected :
        if (miniGameManager.collectedVotes == players.Count)
        {
            miniGameManager.MiniGameWinnerElection();
        }
    }

    #region CharacterSelection
    
    public void CheckIfCharacterIsSelected(Enums.CharacterName characterName, int playerNumber) //Check if character called is already selected by a player
    {
        if (!_selectedCharacters.Contains(characterName))
        {
            _selectedCharacters.Add(characterName);
            //Show the player on Screen
            Instantiate(_batPrefab, _playerTransforms[playerNumber - 1].transform);
            _playerTransforms[0].gameObject.SetActive(true);
            ReturnSelectedCharacter(characterName, playerNumber);
        }
        else
        {
            ReturnSelectedCharacter(Enums.CharacterName.None, playerNumber);
        }
    }

    [ObserversRpc]
    private void ReturnSelectedCharacter(Enums.CharacterName selectedCharacter, int playerNumber) //return to player and update its data if character is already selected
    {
        if (myLocalPlayer.playerData.playerNumber == playerNumber && _canSelectCharacter)
        {
            if (selectedCharacter == Enums.CharacterName.None)
            {
                uiManager.ResetButtonCharacterSelection();
            }
            else
            {
                _canSelectCharacter = false;
                myLocalPlayer.playerData.character = selectedCharacter;
                uiManager.SwitchOnNameSelectionUI();
            }
        }
    }

    public void RegisterNameOnServer(string playerName, int sentPlayerNumber)
    {
        _playerNames.Add(sentPlayerNumber, playerName);
        switch (sentPlayerNumber)
        {
            case 1:
                _player1NameText.text = playerName;
                break;
            case 2:
                _player2NameText.text = playerName;
                break;
            case 3:
                _player3NameText.text = playerName;
                break;
            case 4:
                _player4NameText.text = playerName;
                break;
        }
    }

    #endregion

    #region Sabotages
    
    public void ReceiveSabotage(int targetNumber)
    {
        if (!_sabotagedPlayers.Contains(targetNumber))
        {
            ReturnIfSabotageWorked(true);
            _sabotagedPlayers.Add(targetNumber);
        }
        else
        {
            ReturnIfSabotageWorked(false);
        }
    }

    [ObserversRpc]
    public void TransmitSabotage(int targetNumber)
    {
        sabotageManager.SelectSabotageToApply(targetNumber);
    }
    
    [ObserversRpc]
    private void ReturnIfSabotageWorked(bool sabotageWorked) //Get the information if sabotage worked
    {
        switch (sabotageWorked)
        {
            case true:
                myLocalPlayer.playerData.playerSabotages -= 1;
                break;
            case false:
                break;
        }
    }

    #endregion
}
