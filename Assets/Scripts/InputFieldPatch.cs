using TMPro;
using UnityEngine;

public class InputFieldPatch : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _textEntered;
    public void ResetText()
    {
        _textEntered.text = "";
    }
}
