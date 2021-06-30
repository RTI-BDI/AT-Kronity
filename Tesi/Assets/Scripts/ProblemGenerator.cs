using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProblemGenerator : MonoBehaviour
{
	public GameObject container;

	public Sprite collectorSprite;
	public Sprite producerSprite;
	public Sprite rechargeStationSprite;

	public GameObject addingPanel;
	public GameObject specificsPanel;

	public GameObject generatingText;

	public GameObject[] fieldTexts = new GameObject[7]; 
	public GameObject[] fieldInputs = new GameObject[7]; 

	public Image objectSprite;

    // Start is called before the first frame update
    void Start()
    {

		addingPanel.SetActive(true);
		specificsPanel.SetActive(false);

		//TODO: to eliminate
		GameObject referenceAddedObject = (GameObject)Instantiate(Resources.Load("AddedObject"));

		for (int i = 0; i < 15; i++)
		{
			GameObject newObj = (GameObject)Instantiate(referenceAddedObject);
			newObj.transform.parent = container.transform;
			newObj.GetComponent<Image>().color = Random.ColorHSV();
		}

		Destroy(referenceAddedObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ChangeView(string obj)
	{
		addingPanel.SetActive(false);
		specificsPanel.SetActive(true);

		generatingText.GetComponent<TMP_Text>().text = obj;

		
		switch (obj)
		{
			case "Collector":
				fieldTexts[0].GetComponent<TMP_Text>().text = "Name: ";
				fieldTexts[1].GetComponent<TMP_Text>().text = "Battery-Amount: ";
				fieldTexts[2].GetComponent<TMP_Text>().text = "Position (X): ";
				fieldTexts[3].GetComponent<TMP_Text>().text = "Position (Y): ";
				fieldTexts[4].GetComponent<TMP_Text>().text = "Initial Wood Amount: ";
				fieldTexts[5].GetComponent<TMP_Text>().text = "Initial Stone Amount: ";
				fieldTexts[6].GetComponent<TMP_Text>().text = "";

				fieldInputs[6].SetActive(false);
				objectSprite.sprite = collectorSprite;
				break;
			case "Producer":
				fieldTexts[0].GetComponent<TMP_Text>().text = "Name: ";
				fieldTexts[1].GetComponent<TMP_Text>().text = "Battery-Amount: ";
				fieldTexts[2].GetComponent<TMP_Text>().text = "Position (X): ";
				fieldTexts[3].GetComponent<TMP_Text>().text = "Position (Y): ";
				fieldTexts[4].GetComponent<TMP_Text>().text = "Initial Wood Amount: ";
				fieldTexts[5].GetComponent<TMP_Text>().text = "Initial Stone Amount: ";
				fieldTexts[6].GetComponent<TMP_Text>().text = "Initial Chest Amount: ";

				objectSprite.sprite = producerSprite;
				break;
			case "Recharge-Station":
				fieldTexts[0].GetComponent<TMP_Text>().text = "Name: ";
				fieldTexts[2].GetComponent<TMP_Text>().text = "Position (X): ";
				fieldTexts[3].GetComponent<TMP_Text>().text = "Position (Y): ";

				fieldInputs[1].SetActive(false);
				fieldInputs[4].SetActive(false);
				fieldInputs[5].SetActive(false);
				fieldInputs[6].SetActive(false);
				objectSprite.sprite = rechargeStationSprite;
				break;
			default:
				break;
		}
	}

	private void ActivateAllInputFields()
	{
		for (int i = 0; i < fieldInputs.Length; i++)
		{
			fieldInputs[i].SetActive(true);
		}
	}
}
