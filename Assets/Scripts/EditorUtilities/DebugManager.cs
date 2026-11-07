using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private GameObject _debugUI;
    void Start()
    {
        _debugUI.SetActive(false);
    }
    
    public void ShowHideDebugUI()
    {
        if (_debugUI.activeInHierarchy)
        {
            _debugUI.SetActive(false);
        }
        else
        {
            _debugUI.SetActive(true);
        }
    }
}
