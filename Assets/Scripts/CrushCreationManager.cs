using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FishNet.Object;
using System.Threading;

public class CrushCreationManager : NetworkBehaviour
{
	//private NetworkBehaviour _networkBehaviour;
	public GameObject crushCreationUI;
	public GameObject crushCreationPC;
	public GameObject Intropresenter;
	public GameObject CodeBackgroundImage;
	public TextMeshProUGUI CodeTextBackground;
	public GameObject ChoseCrushPart;
	public GameObject Player1;
	public GameObject BullePlayer1;
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
	private List<string> crushParts = new List<string>();

	private string partEnvoie;


	public int index = 0;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		//crushCreationUI.SetActive(false);
		//HairSprite = GetComponent<Image>();
		//hairImage.sprite = hairSprites[index];
		indexCrushCreation[index].color = new Color32(91, 85, 175, 255);
		//crushImage = hairImage;
		//crushSprites = hairSprites;
		//hairImage.enabled = true;
		//crushImage.enabled = true;
		crushParts.Add("Hair");
		crushParts.Add("Accessories");
		crushParts.Add("Faces");
		crushParts.Add("Clothes");
		//Player1.SetActive(true);
		//Player1.GetComponentInChildren<GameObject>().SetActive(true);



	}

	// Update is called once per frame
	void Update()
	{


	}

	public void OpenCrushCreation()
	{
		crushCreationUI.SetActive(true);
		Debug.Log("Open Crush Creation");
	}

	public void CloseCrushCreation()
	{
		crushCreationUI.SetActive(false);
	}
	public void OpenCrushCreationPC()
	{
		crushCreationPC.SetActive(true);
		CodeBackgroundImage.SetActive(true);


		CodeBackgroundImage.SetActive(true);
		CodeTextBackground.text = "Avant de commencer...";
		Invoke("ChangeCrushScreen", 5f);
	}

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
	public void chosePartCrush()
	{
		hairImage.enabled = false;
		accessoriesImage.enabled = false;
		clothesImage.enabled = false;
		faceImage.enabled = false;

		int randomIndex = Random.Range(0, crushParts.Count);
		string part = crushParts[randomIndex];
		switch (part)
		{
			case "Hair":
				hairImage.enabled = true;
				IndicationDressCrush(part);
				//hairImage.sprite = hairSprites[index];
				crushImage = hairImage;
				crushSprites = hairSprites;
				partEnvoie = part;
				break;
			case "Accessories":
				accessoriesImage.enabled = true;
				IndicationDressCrush(part);
				//accessoriesSprite.sprite = accessoriesSprites[index];
				crushImage = accessoriesImage;
				crushSprites = accessoriesSprites;
				partEnvoie = part;
				break;
			case "Faces":
				faceImage.enabled = true;
				IndicationDressCrush(part);
				//faceImage.sprite = facesSprites[index];
				crushImage = faceImage;
				crushSprites = facesSprites;
				partEnvoie = part;
				break;
			case "Clothes":
				clothesImage.enabled = true;
				IndicationDressCrush(part);
				//clothesImage.sprite = clothesSprites[index];
				crushImage = clothesImage;
				crushSprites = clothesSprites;
				partEnvoie = part;
				break;
		}
		RemoveCrushPart(part);
	}

	public void RemoveCrushPart(string partToRemove)
	{
		crushParts.Remove(partToRemove);


	}
	//[ServerRpc(RequireOwnership = false)]
	[ServerRpc]
	public void HideBullePlayer1()
	{
		BullePlayer1.SetActive(false);
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

