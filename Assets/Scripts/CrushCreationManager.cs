using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using FishNet.Object;
using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class CrushCreationManager : NetworkBehaviour
{
    #region variables
    [SerializeField] private GameManagerNetwork _gameManagerNetwork;
	//[SerializeField] private UIManager _uiManager;
	
	public GameObject choseCrushPart;
	public List<GameObject> BullePlayers = new List<GameObject>();
	public List<Image> indexCrushCreation = new List<Image>();
	public TextMeshProUGUI partText;
	public Image hairImage;
	public Image accessoriesImage;
	public Image faceImage;
	public Image clothesImage;
	public Image crushImage;
	public List<Sprite> crushSprites = new List<Sprite>();
	public List<Sprite> hairSprites = new List<Sprite>();
	public List<Sprite> accessoriesSprites = new List<Sprite>();
	public List<Sprite> facesSprites = new List<Sprite>();
	public List<Sprite> clothesSprites = new List<Sprite>();
	public List<List<Sprite>> CrushImages = new List<List<Sprite>>();
	[SerializeField] private List<string> _crushParts = new List<string>();
	public List<string> indexParts = new List<string>();
	private string partEnvoie;
    public int index = 0;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
	{
		//crushCreationUI.SetActive(false);
		//HairSprite = GetComponent<Image>();
		//hairImage.sprite = hairSprites[index];
		indexCrushCreation[index].color = new Color32(91, 85, 175, 255);
	}
	// Update is called once per frame
	void Update()
	{

	}
	public void StartCrush()
	{
        _gameManagerNetwork.uiManager.OpenCrushCreationPC();
		Invoke("StartList", 5f);
    }

	public void StartList()
    {
		CrushList();
    }
    public void CrushList()
	{
		for (int i = 0; i < 4; i++) {

			int randomIndex = Random.Range(0, indexParts.Count);

			_crushParts[i] = indexParts[randomIndex];
			string part = indexParts[randomIndex];
			indexParts.RemoveAt(randomIndex);
		}
        initializationCrushCreation(_crushParts);
    }

	[ObserversRpc]
	public void initializationCrushCreation([NotNull] List<string> allPartsOnServer)
	{
		_gameManagerNetwork.uiManager.OpenCrushCreation();


        // GetPlayerNetwork();
        Debug.Log("try is, playernumber : " + _gameManagerNetwork.myLocalPlayer.playerData.playerNumber);
		partEnvoie = allPartsOnServer[_gameManagerNetwork.myLocalPlayer.playerData.playerNumber];
		Debug.Log("my part is : "+partEnvoie);

		switch (partEnvoie)
		{
			case "Faces":
                //afficher tout ce qui est relatif à Face
                faceImage.enabled = true;
                IndicationDressCrush(partEnvoie);
                crushImage = faceImage;
                crushSprites = facesSprites;
                Debug.Log("Part: " + partEnvoie);
                break;
			case "Hair":
                //afficher tout ce qui est relatif à Hair
                hairImage.enabled = true;
				IndicationDressCrush(partEnvoie);
                crushImage = hairImage;
                crushSprites = hairSprites;
                Debug.Log("Part: " + partEnvoie);
                break;
			case "Accessories":
                //afficher tout ce qui est relatif à Accessories
                accessoriesImage.enabled = true;
                IndicationDressCrush(partEnvoie);
                crushImage = accessoriesImage;
                crushSprites = accessoriesSprites;
                Debug.Log("Part: " + partEnvoie);
                break;
			case "Clothes":
                //afficher tout ce qui est relatif à Clothes
                clothesImage.enabled = true;
                IndicationDressCrush(partEnvoie);
                crushImage = clothesImage;
                crushSprites = clothesSprites;
                Debug.Log("Part: " + partEnvoie);
                break;
        }
	}

    public void IndicationDressCrush(string part)
	{
		if (part == "Hair")
		{
			partText.text = "Tu dois choisir les <b>cheveux</b>\n de la poupée";
		}
		else if (part == "Accessories")
		{
			partText.text = "Tu dois choisir <b>l'accessoire</b>\n de la poupée";
		}
		else if (part == "Faces")
		{
			partText.text = "Tu dois choisir la <b>tête</b>\n de la poupée";
		}
		else if (part == "Clothes")
		{
			partText.text = "Tu dois choisir la <b>tenue</b>\n de la poupée";
		}
	}
	public void RightListSprite()
	{
		indexCrushCreation[index].color = Color.white;
		if (index >= 2)
		{
			index = 0;
		}
		else
		{
			index++;
		}
		Debug.Log("Index: " + index);
		indexCrushCreation[index].color = new Color32(91, 85, 175, 255);
		crushImage.sprite = crushSprites[index];
	}
	public void LeftListSprite()
	{
		indexCrushCreation[index].color = Color.white;
		if (index <= 0)
		{
			index = 2;
		}
		else
		{
			index--;
		}
		Debug.Log("Index: " + index);
		indexCrushCreation[index].color = new Color32(91, 85, 175, 255);
		crushImage.sprite = crushSprites[index];
	}

	public void Confirm()
	{
		Debug.Log("Part: " + partEnvoie);
		//_uiManager.CrushPC();
        if (partEnvoie == "Hair")
		{
            EndReflectionPlayer(_gameManagerNetwork.myLocalPlayer.playerData.playerNumber - 1, partEnvoie, index);
			int valeurSprite = index + 1;
			//Debug.Log("Number: " + valeurSprite);
        }
		else if (partEnvoie == "Accessories")
		{

            EndReflectionPlayer(_gameManagerNetwork.myLocalPlayer.playerData.playerNumber - 1, partEnvoie, index);
			int valeurSprite = index + 7;
			//Debug.Log("Number: " + valeurSprite);
            
        }
		else if (partEnvoie == "Faces")
		{

            EndReflectionPlayer(_gameManagerNetwork.myLocalPlayer.playerData.playerNumber - 1, partEnvoie, index);
			int valeurSprite = index + 13;
			//Debug.Log("Number: " + valeurSprite);
            
        }
		else if (partEnvoie == "Clothes")
		{

            EndReflectionPlayer(_gameManagerNetwork.myLocalPlayer.playerData.playerNumber - 1, partEnvoie, index);
			int valeurSprite = index + 19;
			//Debug.Log("Number: " + valeurSprite);
			
        }

    }
    [ServerRpc(RequireOwnership = false)]
    public void EndReflectionPlayer(int playerNumber, string partPlayer, int index)
    {
        //Debug.Log("Player: " + playerNumber);
        BullePlayers[playerNumber].SetActive(false);
        //Debug.Log("NumPlayer V2: " + playerNumber);
        switch (partPlayer)
        {
            case "Hair":
                Debug.Log("Switch :" + partPlayer);
				Debug.Log("Index : " + index);

                _gameManagerNetwork.uiManager.SpriteCrush(hairSprites[index], partPlayer);
                break;
            case "Accessories":
                Debug.Log("Switch :" + partPlayer);
                Debug.Log("Index : " + index);
                _gameManagerNetwork.uiManager.SpriteCrush(accessoriesSprites[index], partPlayer);
                break;
            case "Faces":
                Debug.Log("Switch :" + partPlayer);
                Debug.Log("Index : " + index);
                _gameManagerNetwork.uiManager.SpriteCrush(facesSprites[index], partPlayer);
                break;
            case "Clothes":
                Debug.Log("Switch :" + partPlayer);
                Debug.Log("Index : " + index);
                _gameManagerNetwork.uiManager.SpriteCrush(clothesSprites[index], partPlayer);
                break;
        }

    }
}

