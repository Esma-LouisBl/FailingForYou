using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameManagerNetwork _gameManagerNetwork;

    #region Dictionaries

    private Dictionary<string, int> _characterNames = new Dictionary<string, int>();
    private Dictionary<int, Sprite> _characterSprites = new Dictionary<int, Sprite>();
    private Dictionary<int, CharacterBehaviour> _characterBehaviours = new Dictionary<int, CharacterBehaviour>();

    #endregion

    public void UpdatePlayerSprite(string characterName, int playerIndex) //Update PlayerSprites on Desktop according to their character
    {
        _gameManagerNetwork.players[playerIndex].GetComponent<SpriteRenderer>().sprite = _characterSprites[_characterNames[characterName]];
    }

    public void PlayAllIdle()
    {
        for (int i = 0; i < _gameManagerNetwork.players.Count; i++)
        {
            _characterBehaviours[_characterNames[_gameManagerNetwork.players[i].characterName]].PlayIdle();
        }
    }
    
    public void PlayAllSurprised()
    {
        for (int i = 0; i < _gameManagerNetwork.players.Count; i++)
        {
            _characterBehaviours[_characterNames[_gameManagerNetwork.players[i].characterName]].PlaySurprised();
        }
    }
    
    public void PlayAllHappy()
    {
        for (int i = 0; i < _gameManagerNetwork.players.Count; i++)
        {
            _characterBehaviours[_characterNames[_gameManagerNetwork.players[i].characterName]].PlayHappy();
        }
    }
}
