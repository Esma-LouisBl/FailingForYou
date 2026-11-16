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
	[SerializeField]
	private GameManagerNetwork _gameManagerNetwork;
	[SerializeField]
	private UIManager _uiManager;
    private PlayerNetwork _playerNetwork;
	public GameObject crushCreationUI;
	public GameObject crushCreationPC;
	public GameObject Intropresenter;
	public GameObject CodeBackgroundImage;
	public TextMeshProUGUI CodeTextBackground;
	public GameObject ChoseCrushPart;
	//public GameObject Player1;
	public List<GameObject> BullePlayers = new List<GameObject>();
	public List<Image> indexCrushCreation = new List<Image>();
	//public GameManagerNetwork gameManagerNetwork;
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
	[SerializeField]
	private List<string> crushParts = new List<string>();

	public List<string> indexParts = new List<string>();

	private string partEnvoie;

	public int Numplayer;

	public int index = 0;

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


	public void CrushList()
	{
		for (int i = 0; i < 4; i++) {

			int randomIndex = Random.Range(0, indexParts.Count);


			crushParts[i] = indexParts[randomIndex];
			string part = indexParts[randomIndex];
			indexParts.RemoveAt(randomIndex);

		}


		OpenCrushCreation(crushParts);
	}




	[ObserversRpc]
	public void OpenCrushCreation([NotNull] List<string> allPartsOnServer)
	{
		crushCreationUI.SetActive(true);
		hairImage.enabled = false;
		accessoriesImage.enabled = false;
		clothesImage.enabled = false;
		faceImage.enabled = false;

		// GetPlayerNetwork();
		Debug.Log("on teste ça, playernumber : " + _gameManagerNetwork.myLocalPlayer.playerData.playerNumber);
		partEnvoie = allPartsOnServer[_gameManagerNetwork.myLocalPlayer.playerData.playerNumber];
		Debug.Log("ma partie c'est : "+partEnvoie);

		switch (partEnvoie)
		{
			case "Face":
				//afficher tout ce qui est relatif à Face
				break;
		}
	}

	public void GetPlayerNetwork()
	{
		// Debug.Log("Coucou");
		_gameManagerNetwork.myLocalPlayer.ReceiveCrushManager();
	}
	[ObserversRpc]
	public void CloseCrushCreation()
	{
		crushCreationUI.SetActive(false);
	}
	[ServerRpc]
	public void OpenCrushCreationPC()
	{
		crushCreationPC.SetActive(true);
		CodeBackgroundImage.SetActive(true);


		CodeBackgroundImage.SetActive(true);
		CodeTextBackground.text = "Avant de commencer...";
		Invoke("ChangeCrushScreen", 5f);
	}
	[ServerRpc]
	public void CloseCrushCreationPC()
	{
		crushCreationPC.SetActive(false);
		CodeBackgroundImage.SetActive(false);
	}

	public void ChangeCrushScreen()
	{
		Intropresenter.SetActive(false);
		ChoseCrushPart.SetActive(true);
		CodeTextBackground.text = "Créer ensemble la poupée !";
	}

	//[ObserversRpc]
	public void ChosePartCrush(int playerNumber)
	{
		//Debug.Log("coucou V3");
		//Debug.Log("Player Number: " + playerNumber);
		Numplayer = playerNumber;
		//Debug.Log("NumPlayer: " + Numplayer);
		string part = crushParts[playerNumber - 1];
		//Debug.Log("Part: " + part);
		DressCrush(part, playerNumber, crushParts);
	}

	[ObserversRpc]
	public void DressCrush(string part, int number, List<string> crushPartsList)
	{
        crushParts = crushPartsList;
        //Debug.Log("Index of Hair: " + crushPartsList.IndexOf("Hair"));
		Debug.Log("Numplayer: " + number);

        if (crushParts.IndexOf("Hair") == number - 1)
		{
			int indexCrushPart = crushParts.IndexOf("Hair");
            Hair(part, number, indexCrushPart);
        }
        else if (crushParts.IndexOf("Accessories") == number - 1)
		{
			int indexCrushPart = crushParts.IndexOf("Accessories");
            Accesories(part, number, indexCrushPart);
        }
		else if (crushParts.IndexOf("Faces") == Numplayer - 1)
		{
			int indexCrushPart = crushParts.IndexOf("Faces");
            Faces(part, number, indexCrushPart);
        }
		else if (crushParts.IndexOf("Clothes") == number - 1)
		{
			int indexCrushPart = crushParts.IndexOf("Clothes");
			Clothes(part, number, indexCrushPart);
        }
        else
        {
            Debug.Log("Part not found for player number: " + number);
        }

        /*
		switch (part)
		{

			case "Hair":
				hairImage.enabled = true;
				IndicationDressCrush(part);
				//hairImage.sprite = hairSprites[index];
				crushImage = hairImage;
				crushSprites = hairSprites;
				partEnvoie = part;
				Debug.Log("Part: " + part);
				RemoveCrushPart(part);
				break;
			case "Accessories":
				accessoriesImage.enabled = true;
				IndicationDressCrush(part);
				//accessoriesSprite.sprite = accessoriesSprites[index];
				crushImage = accessoriesImage;
				crushSprites = accessoriesSprites;
				partEnvoie = part;
				Debug.Log("Part: " + part);
				RemoveCrushPart(part);
				break;
			case "Faces":
				faceImage.enabled = true;
				IndicationDressCrush(part);
				//faceImage.sprite = facesSprites[index];
				crushImage = faceImage;
				crushSprites = facesSprites;
				partEnvoie = part;
				Debug.Log("Part: " + part);
				RemoveCrushPart(part);
				break;
			case "Clothes":
				clothesImage.enabled = true;
				IndicationDressCrush(part);
				//clothesImage.sprite = clothesSprites[index];
				crushImage = clothesImage;
				crushSprites = clothesSprites;
				partEnvoie = part;
				Debug.Log("Part: " + part);
				RemoveCrushPart(part);
				break;

		}
		*/


    }
	
	public void Hair(string part, int Numplayer, int indexCrushPart){
		if(indexCrushPart == Numplayer - 1){
			hairImage.enabled = true;
			IndicationDressCrush(part);
			//hairImage.sprite = hairSprites[index];
			crushImage = hairImage;
			crushSprites = hairSprites;
			partEnvoie = part;
			Debug.Log("Part: " + part);
			RemoveCrushPart(part); 
		}
        else
        {
            Debug.Log("IndexCrushPart does not match :" + (Numplayer - 1));
        }



    }

    public void Accesories(string part, int Numplayer, int indexCrushPart)
	{
		if (indexCrushPart == Numplayer - 1)
		{
			accessoriesImage.enabled = true;
			IndicationDressCrush(part);
			//accessoriesSprite.sprite = accessoriesSprites[index];
			crushImage = accessoriesImage;
			crushSprites = accessoriesSprites;
			partEnvoie = part;
			Debug.Log("Part: " + part);
			RemoveCrushPart(part);
		}else
        {
            Debug.Log("IndexCrushPart does not match :" + (Numplayer - 1));
        }

    }
	
	public void Faces(string part, int Numplayer, int indexCrushPart)
	{
		if (indexCrushPart == Numplayer - 1)
		{
			faceImage.enabled = true;
			IndicationDressCrush(part);
			//faceImage.sprite = facesSprites[index];
			crushImage = faceImage;
			crushSprites = facesSprites;
			partEnvoie = part;
			Debug.Log("Part: " + part);
			RemoveCrushPart(part);
		}
        else
        {
            Debug.Log("IndexCrushPart does not match :" + (Numplayer - 1));
        }

    }

    public void Clothes(string part, int Numplayer, int indexCrushPart)
	{
		if (indexCrushPart == Numplayer - 1)
		{
			clothesImage.enabled = true;
			IndicationDressCrush(part);
			//clothesImage.sprite = clothesSprites[index];
			crushImage = clothesImage;
			crushSprites = clothesSprites;
			partEnvoie = part;
			Debug.Log("Part: " + part);
			RemoveCrushPart(part);
		}
        else
        {
            Debug.Log("IndexCrushPart does not match :" + (Numplayer - 1));
        }


    }


    public void RemoveCrushPart(string partToRemove)
	{
		crushParts.Remove(partToRemove);


	}
	
	[ServerRpc(RequireOwnership = false)]
    public void HideBullePlayer1()
	{
        Debug.Log("NumPlayer V2: " + Numplayer);
        BullePlayers[Numplayer - 1].SetActive(false);
		//BullePlayer1.SetActive(false);
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

		if (partEnvoie == "Hair")
		{
			HideBullePlayer1();
			int valeurSprite = index + 1;
			Debug.Log("Numero: " + valeurSprite);
		}
		else if (partEnvoie == "Accessories")
		{
			HideBullePlayer1();
			int valeurSprite = index + 7;
			Debug.Log("Numero: " + valeurSprite);
		}
		else if (partEnvoie == "Faces")
		{
			HideBullePlayer1();
			int valeurSprite = index + 13;
			Debug.Log("Numero: " + valeurSprite);
		}
		else if (partEnvoie == "Clothes")
		{
			HideBullePlayer1();
			int valeurSprite = index + 19;
			Debug.Log("Numero: " + valeurSprite);
		}


		//gameManagerNetwork.PlayerSendCrush();
	}
}

