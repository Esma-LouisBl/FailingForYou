using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region UI
    
    [SerializeField] private GameObject _hostUI, _clientUI;
    [SerializeField] private GameObject _hostQuestionUI;
    [SerializeField] private GameObject _clientQuestionUI, _clientQuestionTextualUI;
        
    #endregion

    #region OtherSerializeFields

    [SerializeField] private TextMeshProUGUI _clientQuestionTMP, _clientQuestionTypeTMP, _clientQuestionGenreTMP;

    #endregion

    public void ShowHostUI()
    {
        _hostUI.SetActive(true);
        _clientUI.SetActive(false);
    }

    public void ShowClientUI()
    {
        _clientUI.SetActive(true);
        _hostUI.SetActive(false);
    }
    
    public void UpdateMiniGameUI(MiniGameData miniGame)
    {
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
}
