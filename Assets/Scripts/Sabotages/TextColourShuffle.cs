using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextColourShuffle : Sabotage
{
    [SerializeField] private List<TextMeshProUGUI> _texts;
    private List<Color> _originalColors = new List<Color>();
    public override void ApplySabotage()
    {
        foreach (TextMeshProUGUI text in _texts)
        {
            _originalColors.Add(text.color);
        }
        StartCoroutine(ShuffleColours());
    }

    private void ResetColours()
    {
        for (int i = 0; i < _texts.Count; i++)
        {
            _texts[i].color = _originalColors[i];
        }
    }

    private IEnumerator ShuffleColours()
    {
        for (int i = 0; i < 100; i++)
        {
            foreach (TextMeshProUGUI text in _texts)
            {
                text.color = Random.ColorHSV();
            }
            yield return new WaitForSeconds(0.2f);
        }
        ResetColours();
    }
}
