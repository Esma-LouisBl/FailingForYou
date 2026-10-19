using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public List<MiniGameData> miniGamesDrawn = new List<MiniGameData>();
    [SerializeField]
    private List<MiniGameData> _allMiniGames = new List<MiniGameData>();
    //could be interesting to complete this list automatically

    public int amountOfMiniGames;
    //Add to miniGamesDrawn [numberOfMiniGames -> depends if short or long game] from _allMiniGames
    public void ChooseMiniGames(int numberOfMiniGames)
    {
        for (int i = 0; i < numberOfMiniGames; i++)
        {
            int r = Random.Range(0, _allMiniGames.Count);
            miniGamesDrawn.Add(_allMiniGames[r]);
            _allMiniGames.RemoveAt(r);
        }
    }
}
