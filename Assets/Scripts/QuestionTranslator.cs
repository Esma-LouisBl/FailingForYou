using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class QuestionTranslator : MonoBehaviour
{
    public TextMeshProUGUI questionHolder;
    public LocalizeStringEvent localizeStringEvent;
    
    public List<MiniGameData> questionsList;
    
    public MiniGameData currentQuestion;

    public void PickQuestion()
    {
        currentQuestion = questionsList[Random.Range(0, questionsList.Count)];
        questionHolder.text = currentQuestion.key;

        localizeStringEvent.StringReference.TableReference = "MainTable";
        localizeStringEvent.StringReference.TableEntryReference = currentQuestion.key;
    }
}