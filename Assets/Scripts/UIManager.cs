using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region UI
    
    [SerializeField] private GameObject _hostUI, _clientMainUI;
    [SerializeField] private GameObject _hostConnectionUI, _clientConnectionUI;
    [SerializeField] private GameObject _hostQuestionUI;
    [SerializeField] private GameObject _clientQuestionUI, _clientQuestionTextualUI;
    [SerializeField] private GameObject _CrushCreationUI;
        
    #endregion

    #region OtherSerializeFields

    [SerializeField] private TextMeshProUGUI _hostQuestionTMP;
    [SerializeField] private TextMeshProUGUI _clientQuestionTMP, _clientQuestionTypeTMP, _clientQuestionGenreTMP;

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
    public void ShowCrushCreationUI()
	{
		_CrushCreationUI.SetActive(true);
        
	}

	public void HideCrushCreationUI()
	{
		_CrushCreationUI.SetActive(false);
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
}
