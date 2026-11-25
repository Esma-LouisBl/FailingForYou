using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _hostQuestionTMP, _hostQuestionTypeTMP, _hostQuestionGenreTMP;
    [SerializeField] private GameObject _hostQuestionUI;
    [SerializeField] private GameObject _hostTextAnswersRevelationUI;
    [SerializeField] private TextMeshProUGUI _answer1, _answer2, _answer3; //for clients
    [SerializeField] private TextMeshProUGUI _answer1Host, _answer2Host, _answer3Host, _answer4Host;
    [SerializeField] private InputFieldPatch _textAnswerInputField, _tieInputField; //mandatory to reset InputFields
    [SerializeField] private TextMeshProUGUI _clientQuestionTMP, _clientQuestionTypeTMP, _clientQuestionGenreTMP, _hookSentence;
    [SerializeField] private GameObject _clientVotingArea, _clientAnswerInCaseThereAreFourPlayers;
    [SerializeField] private GameObject _clientQuestionUI, _clientQuestionTextualUI, _clientConfirmationFooter;
    [SerializeField] private GameObject _answerOnHost1, _answerOnHost2, _answerOnHost3, _answerOnHost4;
    
    private List<int> _randomOrderOfAnswersOnScreen = new List<int>();
    
    //All the tokens that represent a vote for an answer on host screen
    [SerializeField] private List<GameObject> _answer1VoteTokens = new List<GameObject>();
    [SerializeField] private List<GameObject> _answer2VoteTokens = new List<GameObject>();
    [SerializeField] private List<GameObject> _answer3VoteTokens = new List<GameObject>();
    [SerializeField] private List<GameObject> _answer4VoteTokens = new List<GameObject>();
    
    [Header("Tie (host)")]
    [SerializeField] private GameObject _tieHostUI;
    [Tooltip("Use this to deform the band")]
    [SerializeField] private RectTransform _bandTransform;
    [SerializeField] private TextMeshProUGUI _tieAppearText;
    [SerializeField] private TextMeshProUGUI _tieQuestionText;
    [SerializeField] private List<GameObject> _playersInTieGO;
    [SerializeField] private List<TextMeshProUGUI> _playersNames;
    [SerializeField] private List<Image> _playersImages;
    [SerializeField] private List<TextMeshProUGUI> _playersAnswers;
    [SerializeField] private GameObject _answerContainer;
    [SerializeField] private TextMeshProUGUI _tieAnswer, _winnerName;

    [SerializeField] [Tooltip("This text appears when some players have entered the same answer")]
    private List<TextMeshProUGUI> _fastestPlayer;
    
    [Header("Tie (client)")]
    [SerializeField] private GameObject _tieClientUI;
    [SerializeField] private TextMeshProUGUI _tieQuestionClient;
    private void Start()
    {
        // Hide some UI we don't want to see
        _hostTextAnswersRevelationUI.SetActive(false);
    }
    public void UpdateTimer(int time)
    {
        _timerText.text = time.ToString();
    }
    
    public void UpdateMiniGameUIHost(MiniGameData miniGame)
    {
        _hostQuestionTMP.text = miniGame.question;
        _hostQuestionTypeTMP.text = miniGame.type.ToString();
        _hostQuestionGenreTMP.text = miniGame.genre.ToString();
        _hostQuestionUI.SetActive(true);
        _hostTextAnswersRevelationUI.SetActive(false);
        _tieHostUI.SetActive(false);
    }
    
    public void UpdateMiniGameUIClient(MiniGameData miniGame)
    {
        _textAnswerInputField.ResetText();
        _tieInputField.ResetText();
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
        _randomOrderOfAnswersOnScreen.Clear();
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

        if (fourPlayers)
        {
            _randomOrderOfAnswersOnScreen.Add(0);
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
        _clientVotingArea.SetActive(true);
        _clientQuestionTextualUI.SetActive(false);
        _clientQuestionUI.SetActive(true);
    }
    
    public void ShowHostMainUIWhilePlayersAreVoting()
    {
        _hookSentence.text = "Votez pour la meilleure réponse !";
        // _hostTextAnswersRevelationUI.SetActive(false);
    }
    
    public void ShowMiniGameWinner(int answer1Votes, int answer2Votes, int answer3Votes, int answer4Votes)
    {
        _hostTextAnswersRevelationUI.SetActive(true);
    
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
    
    #region Tie

    public void ShowTieOnHost(List<int> playersInTie, string question)
    {
        _answerContainer.SetActive(false);
        _winnerName.gameObject.SetActive(false);
        _tieHostUI.SetActive(true);
        
        foreach (var text in _fastestPlayer)
        {
            text.gameObject.SetActive(false);
        }
        StartCoroutine(TieStartAnimation(playersInTie, question));
    }

    private IEnumerator TieStartAnimation(List<int> playersInTie, string question)
    {
        _tieQuestionText.text = question;
        _tieAppearText.gameObject.SetActive(true);  //little animation when tie is appearing
        for (int i = 0; i < 260; i++)
        {
            _bandTransform.sizeDelta = new Vector2(_bandTransform.sizeDelta.x, _bandTransform.sizeDelta.y+1);
            yield return new WaitForSeconds(0.01f);
        }
        _tieAppearText.gameObject.SetActive(false);

        for (int i = 0; i < playersInTie.Count; i++)    //how many players should appear
        {
            _playersInTieGO[i].gameObject.SetActive(true);
            // _playersNames[i].text = playersInTie[i].playerName;
        }

    }

    public void ShowTieOnClient(string tieQuestion)
    {
        _tieClientUI.SetActive(true);
        _tieQuestionClient.text = tieQuestion;
    }
    
    public void ShowTieWinner(int winnerNumber, List<int> playersInTie, List<float> collectedTieAnswers, float answer)
    {
        for (int i = 0; i < playersInTie.Count; i++)
        {
            if (collectedTieAnswers[playersInTie[i]] == -67.67f)
            {
                _playersAnswers[i].text = collectedTieAnswers[winnerNumber].ToString();
                _fastestPlayer[i-1].gameObject.SetActive(true);
            }
            else
            {
                _playersAnswers[i].text = collectedTieAnswers[playersInTie[i]].ToString();
            }
        }

        _winnerName.gameObject.SetActive(true);
        _winnerName.text = $"Le gagnant est le joueur {winnerNumber} !";
        _tieAnswer.text = $"La réponse attendue était : {answer}";
        _answerContainer.SetActive(true);
    }
    
    #endregion
    
    public void ShowWaitingScreenAndShutDownEverythingElse()
    {
        _clientQuestionUI.SetActive(false);
        _clientVotingArea.SetActive(false);
        _tieClientUI.SetActive(false);
    }
}
