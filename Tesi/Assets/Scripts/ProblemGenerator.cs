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

	private struct Entity
	{
		public string name;
		public string type;
		public Dictionary<string, int> beliefs;
		public Sprite mySprite;

		public Entity(string name, string type, Dictionary<string, int> beliefs, Sprite mySprite)
		{
			this.name = name;
			this.type = type;
			this.beliefs = new Dictionary<string, int>(beliefs);
			this.mySprite = mySprite;
		}

	}

	private List<Entity> problemEntities = new List<Entity>();

    // Start is called before the first frame update
    void Start()
    {
		GoBack();
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
		ActivateAllInputFields();
		
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
				fieldTexts[1].GetComponent<TMP_Text>().text = "";
				fieldTexts[2].GetComponent<TMP_Text>().text = "Position (X): ";
				fieldTexts[3].GetComponent<TMP_Text>().text = "Position (Y): ";
				fieldTexts[4].GetComponent<TMP_Text>().text = "";
				fieldTexts[5].GetComponent<TMP_Text>().text = "";
				fieldTexts[6].GetComponent<TMP_Text>().text = "";

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

	public void GoBack()
	{
		addingPanel.SetActive(true);
		specificsPanel.SetActive(false);

		GameObject referenceAddedObject = (GameObject)Instantiate(Resources.Load("AddedObject"));

		foreach (Transform child in container.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		foreach (Entity e in problemEntities)
		{
			GameObject newObj = (GameObject)Instantiate(referenceAddedObject);
			newObj.transform.parent = container.transform;
			newObj.transform.GetChild(0).GetComponent<Image>().sprite = e.mySprite;
			newObj.GetComponentInChildren<TMP_Text>().text = e.name;
		}

		Destroy(referenceAddedObject);
	}

	public void AddEntity()
	{
		Dictionary<string, int> tempBeliefs = new Dictionary<string, int>();

		//"i" starts at 1 in order to avoid name
		for (int i = 1; i < fieldInputs.Length - 1; i++)
		{
			if (fieldInputs[i].active)
			{
				int value;
				bool success = int.TryParse(fieldInputs[i].GetComponent<TMP_InputField>().text, out value);

				if (success)
				{
					//TODO add constraints
					switch (fieldTexts[i].GetComponent<TMP_Text>().text)
					{
						case "Battery-Amount: ":
							//TODO - Fixed value 100
							if(value > 0 && value < 100)
							{
								tempBeliefs.Add("battery-amount", value);
							} else
							{
								BadInput("Battery Amount value out of bound");
							}	
							break;
						case "Position (X): ":
							tempBeliefs.Add("posX", value);
							break;
						case "Position (Y): ":
							tempBeliefs.Add("posY", value);
							break;
						case "Initial Wood Amount: ":
							tempBeliefs.Add("wood-amount", value);
							break;
						case "Initial Stone Amount: ":
							tempBeliefs.Add("stone-amount", value);
							break;
						case "Initial Chest Amount: ":
							tempBeliefs.Add("chest-amount", value);
							break;
						default:
							break;
					}
				} else
				{
					BadInput("Bad value in " + fieldTexts[i].GetComponent<TMP_Text>().text);
				}
			}
		}

		Entity toAdd = new Entity(fieldInputs[0].GetComponent<TMP_InputField>().text, generatingText.GetComponent<TMP_Text>().text.ToLower(), tempBeliefs, objectSprite.sprite);

		problemEntities.Add(toAdd);
		GoBack();
	}

	private void BadInput(string why)
	{
		Debug.Log("ERROR -- " + why);
		GoBack();
		return;
	}
}
