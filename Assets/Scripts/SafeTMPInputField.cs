// Source - https://stackoverflow.com/a/79670677
// Posted by Prakyath
// Retrieved 2026-10-26, License - CC BY-SA 4.0

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SafeTMPInputField : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private TMP_InputField inputField;
    private string cachedText = "";

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        cachedText = inputField.text;
    }

    void Update()
    {
        if (inputField.isFocused)
        {
            cachedText = inputField.text;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("test");
        cachedText = inputField.text;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("on deselect");
        // If the text got wiped, bring it back
        if (string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("dans le if");
            inputField.text = cachedText;
        }
    }

    public void OnEndEdit()
    {
        // If the text got wiped, bring it back
        if (string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("dans le if le 2");
            inputField.text = cachedText;
        }
    }
}