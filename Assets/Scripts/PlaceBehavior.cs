using System.Collections.Generic;
using UnityEngine;

public class PlaceBehavior : MonoBehaviour
{
    [Header("Genres : ROM - 1 ; DRA - 2 ; FAN - 3 ; HIS - 4 ; HOR - 5")]
    [SerializeField] private List<GameObject> _genres = new List<GameObject>();

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
    }

    public void HideAllGenres()
    {
        foreach (GameObject g in _genres)
        {
            g.gameObject.SetActive(false);
        }
    }
}
