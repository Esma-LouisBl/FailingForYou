using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [Header("Places")] [SerializeField] private GameObject _tvScene;
    [SerializeField] private GameObject _dressingScene, _beachScene, _campingScene, _parkScene;

    [SerializeField] private GameObject _connectingAssets, _crushCreationAssets;
    
    [Header("MiniGames")] [SerializeField] private GameObject _drawingAssets;
    [SerializeField] private GameObject _textualAssets, _soundAssets, _photographicAssets;

    [SerializeField] private GameObject _camera;
    [SerializeField] private Transform _cameraSlot1, _cameraSlot2;
    
    private List<GameObject> _scenes = new List<GameObject>();

    private void Start()
    {
        _scenes.Add(_beachScene);
        _scenes.Add(_campingScene);
        _scenes.Add(_parkScene);
        SwitchOnCamera(1);
        HideAll();
    }

    private void HideAll()
    {
        foreach (GameObject g in _scenes)
        {
            g.GetComponent<PlaceBehavior>().HideAllGenres();
            g.SetActive(false);
        }
        _tvScene.SetActive(false);
        _dressingScene.SetActive(false);
        _crushCreationAssets.SetActive(false);
        _connectingAssets.SetActive(false);
        _textualAssets.gameObject.SetActive(false);
        _soundAssets.gameObject.SetActive(false);
        _photographicAssets.gameObject.SetActive(false);
        _drawingAssets.gameObject.SetActive(false);
    }
    
    public void UpdateScene(Enums.MiniGamePlace miniGamePlace, Enums.MiniGameGenre miniGameGenre, Enums.MiniGameType miniGameType)
    {
        HideAll();
        switch (miniGamePlace)
        {
            case Enums.MiniGamePlace.tv :
                _tvScene.gameObject.SetActive(true);
                break;
            
            case Enums.MiniGamePlace.beach :
                _beachScene.gameObject.SetActive(true);
                _beachScene.GetComponent<PlaceBehavior>().UpdateGenre(miniGameGenre);
                SetAssetsForMiniGame(miniGameType);
                break;
            
            case Enums.MiniGamePlace.camping :
                _campingScene.gameObject.SetActive(true);
                _campingScene.GetComponent<PlaceBehavior>().UpdateGenre(miniGameGenre);
                SetAssetsForMiniGame(miniGameType);
                break;
            
            case Enums.MiniGamePlace.park :
                _parkScene.gameObject.SetActive(true);
                _parkScene.GetComponent<PlaceBehavior>().UpdateGenre(miniGameGenre);
                SetAssetsForMiniGame(miniGameType);
                break;
        }
    }

    private void SetAssetsForMiniGame(Enums.MiniGameType miniGameType)
    {
        switch (miniGameType)
        {
            case Enums.MiniGameType.drawing :
                _drawingAssets.gameObject.SetActive(true);
                break;
            case Enums.MiniGameType.textual :
                _textualAssets.gameObject.SetActive(true);
                break;
            case Enums.MiniGameType.sound :
                _soundAssets.gameObject.SetActive(true);
                break;
            case Enums.MiniGameType.photographic :
                _photographicAssets.gameObject.SetActive(true);
                break;
            case Enums.MiniGameType.none :
                break;
            case Enums.MiniGameType.accessories :
                break;
            case Enums.MiniGameType.clothing :
                break;
        }
    }

    public void ShowConnectingAssets()
    {
        _dressingScene.gameObject.SetActive(true);
        _connectingAssets.gameObject.SetActive(true);
    }
    
    public void ShowCrushCreationAssets()
    {
        _dressingScene.gameObject.SetActive(true);
        _crushCreationAssets.gameObject.SetActive(true);
    }

    public void SwitchOnCamera(int camera)
    {
        switch (camera)
        {
            case 1:
                _camera.GetComponent<Transform>().position = _cameraSlot1.position;
                _camera.GetComponent<Transform>().rotation = _cameraSlot1.rotation;
                break;
            case 2:
                _camera.GetComponent<Transform>().position = _cameraSlot2.position;
                _camera.GetComponent<Transform>().rotation = _cameraSlot2.rotation;
                break;
        }
    }
}
