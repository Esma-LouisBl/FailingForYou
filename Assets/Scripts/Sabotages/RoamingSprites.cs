using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RoamingSprites : Sabotage
{
    [SerializeField] private GameObject _roamingSpritePrefab;
    [SerializeField] private RectTransform _parentCanvas;
    
    private List<GameObject> _prefabs = new List<GameObject>();
    private List<RectTransform> _prefabTransforms = new List<RectTransform>();
    private List<int> _prefabBehaviour = new List<int>(); //1 : top right | 2 : bot right | 3 : top left | 4 : bot left
    
    private bool _isInitialized;
    public override void ApplySabotage()
    {
        _prefabs.Clear();
        _prefabTransforms.Clear();
        
        for (int i = 0; i < 20; i++)
        {
            GameObject p = Instantiate(_roamingSpritePrefab, _parentCanvas);
            _prefabs.Add(p);
            _prefabTransforms.Add(p.GetComponent<RectTransform>());
            _prefabBehaviour.Add(Random.Range(1, 5));
        }

        foreach (var prefab in _prefabTransforms)   //spawn each prefab at a random position
        {
            prefab.localPosition = new Vector3(Random.Range(-495, 495), Random.Range(-905, 905), prefab.localPosition.z);
        }
        StartCoroutine(LifeTimer());
        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized)
        {
            for (int i = 0; i < _prefabs.Count; i++)
            {
                switch (_prefabBehaviour[i])    //movement depending on the behaviour
                {
                    case 1:
                        _prefabTransforms[i].localPosition = new Vector3(_prefabTransforms[i].localPosition.x + 1,
                            _prefabTransforms[i].localPosition.y + 1, _prefabTransforms[i].localPosition.z);
                        break;
                    case 2:
                        _prefabTransforms[i].localPosition = new Vector3(_prefabTransforms[i].localPosition.x + 1,
                            _prefabTransforms[i].localPosition.y - 1, _prefabTransforms[i].localPosition.z);
                        break;
                    case 3:
                        _prefabTransforms[i].localPosition = new Vector3(_prefabTransforms[i].localPosition.x - 1,
                            _prefabTransforms[i].localPosition.y + 1, _prefabTransforms[i].localPosition.z);
                        break;
                    case 4:
                        _prefabTransforms[i].localPosition = new Vector3(_prefabTransforms[i].localPosition.x - 1,
                            _prefabTransforms[i].localPosition.y - 1, _prefabTransforms[i].localPosition.z);
                        break;
                }
                
                //      20% chances to change behaviour
                // int changeBehaviour = Random.Range(0, 100);
                // if (changeBehaviour < 2)
                // {
                //     _prefabBehaviour[i] = Random.Range(1, 5);
                // }
                
                //the part below is here to put space limits -> when it reaches a border, it goes to the opposite direction
                if (_prefabTransforms[i].localPosition.x >= 495)    //in case it goes out (right)
                {
                    _prefabBehaviour[i] = Random.Range(3, 5);
                }
                if (_prefabTransforms[i].localPosition.y >= 905)    //in case it goes out (top)
                {
                    int r = Random.Range(0, 2);
                    if (r == 0)
                    {
                        _prefabBehaviour[i] = 2;
                    }
                    else
                    {
                        _prefabBehaviour[i] = 4;
                    }
                }
                if (_prefabTransforms[i].localPosition.x <= -495)    //in case it goes out (left)
                {
                    _prefabBehaviour[i] = Random.Range(1, 3);
                }
                if (_prefabTransforms[i].localPosition.y <= -905)    //in case it goes out (bottom)
                {
                    int r = Random.Range(0, 2);
                    if (r == 0)
                    {
                        _prefabBehaviour[i] = 1;
                    }
                    else
                    {
                        _prefabBehaviour[i] = 3;
                    }
                }
            }
        }
    }

    private void DestroyAllPrefabs()
    {
        _isInitialized = false;
        foreach (var prefab in _prefabs)
        {
            Destroy(prefab);
        }
    }

    private IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(15f);
        DestroyAllPrefabs();
    }
}
