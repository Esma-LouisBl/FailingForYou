using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFontShuffle : Sabotage
{
    [SerializeField] private List<TMP_FontAsset> _fonts;
    [SerializeField] private List<TextMeshProUGUI> _texts;
    
    private List<TMP_FontAsset> _originalFonts = new List<TMP_FontAsset>();
    public override void ApplySabotage()
    {
        foreach (var text in _texts)    //Save original fonts
        {
            _originalFonts.Add(text.font);
        }

        StartCoroutine(ShuffleFonts());
    }
    private void ResetFonts()
    {
        for (int i = 0; i < _texts.Count; i++)
        {
            _texts[i].font = _originalFonts[i];
        }
    }

    private IEnumerator ShuffleFonts()
    {
        for (int i = 0; i < 100; i++)
        {
            foreach (TextMeshProUGUI text in _texts)
            {
                text.font = _fonts[Random.Range(0, _fonts.Count)];
            }
            yield return new WaitForSeconds(0.2f);
        }
        ResetFonts();
    }
}
