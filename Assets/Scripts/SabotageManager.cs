using System.Collections.Generic;
using UnityEngine;

public class SabotageManager : MonoBehaviour
{
    [SerializeField] private GameManagerNetwork _gameManagerNetwork;
    [SerializeField] private List<Sabotage> _sabotages = new List<Sabotage>();

    public void SelectSabotageToApply(int targetNumber) //Apply sent Sabotage if this player is the target
    {
        if (_gameManagerNetwork.myLocalPlayer.playerData.playerNumber == targetNumber)
        {
            Sabotage _sabotageToApply = _sabotages[Random.Range(0, _sabotages.Count-1)];
            _sabotageToApply.ApplySabotage();
        }
    }
}
