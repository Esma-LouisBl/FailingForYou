using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class CrushCreationManager : MonoBehaviour
{

	public GameObject crushCreationUI;
	public List<Image> indexCrushCreation = new List<Image>();
	public GameManagerNetwork gameManagerNetwork;
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
		crushCreationUI.SetActive(false);
		//HairSprite = GetComponent<Image>();
		//hairImage.sprite = hairSprites[index];
		indexCrushCreation[index].color = new Color32(91,85,175, 255);
		//crushImage = hairImage;
		//crushSprites = hairSprites;
		//hairImage.enabled = true;
		//crushImage.enabled = true;
		crushParts.Add("Hair");
		crushParts.Add("Accessories");
		crushParts.Add("Faces");
		crushParts.Add("Clothes");



	}

	// Update is called once per frame
	void Update()
	{
		
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
				//hairImage.sprite = hairSprites[index];
				crushImage = hairImage;
				crushSprites = hairSprites;
				partEnvoie = part;
				break;
			case "Accessories":
				accessoriesImage.enabled = true;
				//accessoriesSprite.sprite = accessoriesSprites[index];
				crushImage = accessoriesImage;
				crushSprites = accessoriesSprites;
				partEnvoie = part;
				break;
			case "Faces":
				faceImage.enabled = true;
				//faceImage.sprite = facesSprites[index];
				crushImage = faceImage;
				crushSprites = facesSprites;
				partEnvoie = part;
				break;
			case "Clothes":
				clothesImage.enabled = true;
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
			int valeurSprite = index + 1;
			Debug.Log("Numero: " + valeurSprite);
		}
		else if (partEnvoie == "Accessories")
		{
			int valeurSprite = index + 7;
			Debug.Log("Numero: " + valeurSprite);
		}
		else if (partEnvoie == "Faces")
		{
			int valeurSprite = index + 13;
			Debug.Log("Numero: " + valeurSprite);
		}
		else if (partEnvoie == "Clothes")
		{
			int valeurSprite = index + 19;
			Debug.Log("Numero: " + valeurSprite);
		}

		//gameManagerNetwork.PlayerSendCrush();
	}
}

