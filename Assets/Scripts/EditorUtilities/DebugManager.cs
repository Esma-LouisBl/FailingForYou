using GameKit.Dependencies.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private GameObject _debugUI;
    
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private TMP_Dropdown _placeDropdown, _genreDropdown, _typeDropdown;
    [SerializeField] private Toggle _cameraToggle, _connectingToggle, _crushCreationToggle;
    
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
    
    public void UpdateSceneFromDebugUI()
    {
        Enums.MiniGamePlace miniGamePlace = Enums.MiniGamePlace.tv;
        Enums.MiniGameGenre miniGameGenre = Enums.MiniGameGenre.none;
        Enums.MiniGameType miniGameType = Enums.MiniGameType.none;
        switch (_placeDropdown.options[_placeDropdown.value].text)
        {
            case "Tv" :
                miniGamePlace = Enums.MiniGamePlace.tv;
                break;
            case "Beach":
                miniGamePlace = Enums.MiniGamePlace.beach;
                break;
            case "Park":
                miniGamePlace = Enums.MiniGamePlace.park;
                break;
            case "Camping":
                miniGamePlace = Enums.MiniGamePlace.camping;
                break;
        }
        switch (_genreDropdown.options[_genreDropdown.value].text)
        {
            case "Romantic" :
                miniGameGenre = Enums.MiniGameGenre.romantic;
                break;
            case "Historic":
                miniGameGenre = Enums.MiniGameGenre.historic;
                break;
            case "Horrific":
                miniGameGenre = Enums.MiniGameGenre.horrific;
                break;
            case "Dramatic":
                miniGameGenre = Enums.MiniGameGenre.dramatic;
                break;
            case "Fantastic":
                miniGameGenre = Enums.MiniGameGenre.fantastic;
                break;
            case "None":
                miniGameGenre = Enums.MiniGameGenre.none;
                break;
        }
        switch (_typeDropdown.options[_typeDropdown.value].text)
        {
            case "Drawing" :
                miniGameType = Enums.MiniGameType.drawing;
                break;
            case "Textual":
                miniGameType = Enums.MiniGameType.textual;
                break;
            case "Sound":
                miniGameType = Enums.MiniGameType.sound;
                break;
            case "Photo":
                miniGameType = Enums.MiniGameType.photographic;
                break;
            case "None":
                miniGameType = Enums.MiniGameType.none;
                break;
        }
        _sceneManager.UpdateScene(miniGamePlace, miniGameGenre, miniGameType);
        _sceneManager.SwitchOnCamera(_cameraToggle.isOn.ToInt()+1);
        if (_connectingToggle.isOn)
        {
            _sceneManager.ShowConnectingAssets();
        }

        if (_crushCreationToggle.isOn)
        {
            _sceneManager.ShowCrushCreationAssets();
        }
    }
}
