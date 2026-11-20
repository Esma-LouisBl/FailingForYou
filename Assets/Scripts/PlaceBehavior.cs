using System.Collections.Generic;
using UnityEngine;

public class PlaceBehavior : MonoBehaviour
{
    [Header("Genres : ROM - 1 ; DRA - 2 ; FAN - 3 ; HIS - 4 ; HOR - 5")]
    [SerializeField] private List<GameObject> _genres = new List<GameObject>();

    [Header("Assets with IndexUV")]
    [SerializeField]
    private Material _KnifePaintingMaterial, _StickersMaterial;
    
    public void UpdateGenre(Enums.MiniGameGenre miniGameGenre)
    {
        switch (miniGameGenre)
        {
            case Enums.MiniGameGenre.romantic :
                _genres [0].gameObject.SetActive(true);
                break;
            case Enums.MiniGameGenre.dramatic :
                _genres [1].gameObject.SetActive(true);
                break;
            case Enums.MiniGameGenre.fantastic :
                _genres [2].gameObject.SetActive(true);
                break;
            case Enums.MiniGameGenre.historic :
                _genres [3].gameObject.SetActive(true);
                break;
            case Enums.MiniGameGenre.horrific :
                _genres [4].gameObject.SetActive(true);
                break;
        }
        
        switch (miniGameGenre)
        {
            case Enums.MiniGameGenre.none :
                _KnifePaintingMaterial.SetFloat("_IndexUV", 0);
                _StickersMaterial.SetFloat("_IndexUV", 0);
                break;
            case Enums.MiniGameGenre.romantic :
                _KnifePaintingMaterial.SetFloat("_IndexUV", 1);
                _StickersMaterial.SetFloat("_IndexUV", 1);
                break;
            case Enums.MiniGameGenre.horrific :
                _KnifePaintingMaterial.SetFloat("_IndexUV", 2);
                _StickersMaterial.SetFloat("_IndexUV", 2);
                break;
            case Enums.MiniGameGenre.fantastic :
                _KnifePaintingMaterial.SetFloat("_IndexUV", 3);
                _StickersMaterial.SetFloat("_IndexUV", 3);
                break;
            case Enums.MiniGameGenre.dramatic :
                _KnifePaintingMaterial.SetFloat("_IndexUV", 4);
                _StickersMaterial.SetFloat("_IndexUV", 4);
                break;
            case Enums.MiniGameGenre.historic :
                _KnifePaintingMaterial.SetFloat("_IndexUV", 5);
                _StickersMaterial.SetFloat("_IndexUV", 5);
                break;
        }
    }

    public void HideAllGenres()
    {
        foreach (GameObject g in _genres)
        {
            g.gameObject.SetActive(false);
        }
    }
}
