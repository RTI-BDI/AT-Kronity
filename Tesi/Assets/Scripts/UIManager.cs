using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject setPanel;
	private static GameObject panel;

	[SerializeField]
	private GameObject setText_1;
	private static GameObject text_1;

	[SerializeField]
	private GameObject setText_2;
	private static GameObject text_2;

	[SerializeField]
	private GameObject setText_3;
	private static GameObject text_3;

	[SerializeField]
	private GameObject setText_4;
	private static GameObject text_4;

	[SerializeField]
	private GameObject setText_5;
	private static GameObject text_5;

	[SerializeField]
	private GameObject setText_6;
	private static GameObject text_6;

	[SerializeField]
	private GameObject setText_7;
	private static GameObject text_7;

	[SerializeField]
	private GameObject setText_8;
	private static GameObject text_8;

	[SerializeField]
	private Image setSprite;
	private static Image sprite;

	[SerializeField]
	private GameObject setSlider;
	private static GameObject slider;

	[SerializeField]
	private GameObject setTextSlider;
	private static GameObject textSlider;

	private static string inspectedObj = "";

	void Start()
	{
		panel = setPanel;
		text_1 = setText_1;
		text_2 = setText_2;
		text_3 = setText_3;
		text_4 = setText_4;
		text_5 = setText_5;
		text_6 = setText_6;
		text_7 = setText_7;
		text_8 = setText_8;
		sprite = setSprite;
		slider = setSlider;
		textSlider = setTextSlider;

		panel.SetActive(false);
	}

	public static void SetVisibleCollector(string objName, int objPosX, int objPosY, int objBatteryAmount, int objWoodAmount, int objStoneAmount, Sprite objSprite)
	{
		inspectedObj = objName;

		//Activate the panel
		if(panel != null)
		{
			panel.SetActive(true);
		}

		if(text_1 != null)
		{
			text_1.GetComponent<TMP_Text>().text = "Name: " + objName;
		}

		if(text_2 != null)
		{
			text_2.GetComponent<TMP_Text>().text = "X-Position: " + objPosX;
		}

		if(text_3 != null)
		{
			text_3.GetComponent<TMP_Text>().text = "Y-Position: " + objPosY;
		}

		if(text_4 != null)
		{
			text_4.GetComponent<TMP_Text>().text = "Battery Level: " + objBatteryAmount + "%";
		}

		if (text_5 != null)
		{
			text_5.GetComponent<TMP_Text>().text = "Wood Carried: " + objWoodAmount;
		}

		if (text_6 != null)
		{
			text_6.GetComponent<TMP_Text>().text = "Stone Carried: " + objStoneAmount;
		}

		if (text_7 != null)
		{
			text_7.GetComponent<TMP_Text>().text = "";
		}

		if (text_8 != null)
		{
			text_8.GetComponent<TMP_Text>().text = "";
		}

		if (sprite != null)
		{
			sprite.sprite = objSprite;
		}

		if(slider != null)
		{
			slider.SetActive(true);
			slider.GetComponent<Slider>().maxValue = GameManager.GetConstants()["battery-capacity"];
			slider.GetComponent<Slider>().value = objBatteryAmount;
		}

		if(textSlider != null)
		{
			textSlider.GetComponent<TMP_Text>().text = objBatteryAmount.ToString();
		}
	}

	public static void UpdateCollectorPanel(string objName, int objPosX, int objPosY, int objBatteryAmount, int objWoodAmount, int objStoneAmount, Sprite objSprite)
	{
		if(panel.active && inspectedObj == objName)
		{
			SetVisibleCollector(objName, objPosX, objPosY, objBatteryAmount, objWoodAmount, objStoneAmount, objSprite);
		}
	}

	public static void SetVisibleProducer(string objName, int objPosX, int objPosY, int objBatteryAmount, int objWoodAmount, int objStoneAmount, int objChestAmount, Sprite objSprite)
	{
		inspectedObj = objName;

		//Activate the panel
		if (panel != null)
		{
			panel.SetActive(true);
		}

		if (text_1 != null)
		{
			text_1.GetComponent<TMP_Text>().text = "Name: " + objName;
		}

		if (text_2 != null)
		{
			text_2.GetComponent<TMP_Text>().text = "X-Position: " + objPosX;
		}

		if (text_3 != null)
		{
			text_3.GetComponent<TMP_Text>().text = "Y-Position: " + objPosY;
		}

		if (text_4 != null)
		{
			text_5.GetComponent<TMP_Text>().text = "Wood Carried: " + objWoodAmount;
		}

		if (text_5 != null)
		{
			text_6.GetComponent<TMP_Text>().text = "Stone Carried: " + objStoneAmount;
		}

		if (text_6 != null)
		{
			text_7.GetComponent<TMP_Text>().text = "Chests Carried: " + objChestAmount;
		}

		if (text_7 != null)
		{
			text_8.GetComponent<TMP_Text>().text = "";
		}

		if (text_8 != null)
		{
			text_8.GetComponent<TMP_Text>().text = "";
		}

		if (sprite != null)
		{
			sprite.sprite = objSprite;
		}

		if (slider != null)
		{
			slider.SetActive(true);
			slider.GetComponent<Slider>().maxValue = GameManager.GetConstants()["battery-capacity"];
			slider.GetComponent<Slider>().value = objBatteryAmount;
		}

		if (textSlider != null)
		{
			textSlider.GetComponent<TMP_Text>().text = objBatteryAmount.ToString();
		}
	}

	public static void UpdateProducerPanel(string objName, int objPosX, int objPosY, int objBatteryAmount, int objWoodAmount, int objStoneAmount, int objChestAmount, Sprite objSprite)
	{
		if (panel.active && inspectedObj == objName)
		{
			SetVisibleProducer(objName, objPosX, objPosY, objBatteryAmount, objWoodAmount, objStoneAmount, objChestAmount, objSprite);
		}
	}

	public static void SetVisibleWood(string objName, int objPosX, int objPosY, Sprite objSprite)
	{
		inspectedObj = objName;

		//Activate the panel
		if (panel != null)
		{
			panel.SetActive(true);
		}

		if (text_1 != null)
		{
			text_1.GetComponent<TMP_Text>().text = "Name: " + objName;
		}

		if (text_2 != null)
		{
			text_2.GetComponent<TMP_Text>().text = "X-Position: " + objPosX;
		}

		if (text_3 != null)
		{
			text_3.GetComponent<TMP_Text>().text = "Y-Position: " + objPosY;
		}

		if (text_4 != null)
		{
			text_4.GetComponent<TMP_Text>().text = "Resource: Wood";
		}

		if (text_5 != null)
		{
			text_5.GetComponent<TMP_Text>().text = "";
		}

		if (text_6 != null)
		{
			text_6.GetComponent<TMP_Text>().text = "";
		}

		if (text_7 != null)
		{
			text_7.GetComponent<TMP_Text>().text = "";
		}

		if (text_8 != null)
		{
			text_8.GetComponent<TMP_Text>().text = "";
		}

		if (sprite != null)
		{
			sprite.sprite = objSprite;
		}

		if (slider != null)
		{
			slider.SetActive(false);
		}
	}

	public static void SetVisibleStone(string objName, int objPosX, int objPosY, Sprite objSprite)
	{
		inspectedObj = objName;

		//Activate the panel
		if (panel != null)
		{
			panel.SetActive(true);
		}

		if (text_1 != null)
		{
			text_1.GetComponent<TMP_Text>().text = "Name: " + objName;
		}

		if (text_2 != null)
		{
			text_2.GetComponent<TMP_Text>().text = "X-Position: " + objPosX;
		}

		if (text_3 != null)
		{
			text_3.GetComponent<TMP_Text>().text = "Y-Position: " + objPosY;
		}

		if (text_4 != null)
		{
			text_4.GetComponent<TMP_Text>().text = "Resource: Stone";
		}

		if (text_5 != null)
		{
			text_5.GetComponent<TMP_Text>().text = "";
		}

		if (text_6 != null)
		{
			text_6.GetComponent<TMP_Text>().text = "";
		}

		if (text_7 != null)
		{
			text_7.GetComponent<TMP_Text>().text = "";
		}

		if (text_8 != null)
		{
			text_8.GetComponent<TMP_Text>().text = "";
		}

		if (sprite != null)
		{
			sprite.sprite = objSprite;
		}

		if (slider != null)
		{
			slider.SetActive(false);
		}
	}

	public static void SetVisibleRechargeStation(string objName, int objPosX, int objPosY, Sprite objSprite)
	{
		inspectedObj = objName;

		//Activate the panel
		if (panel != null)
		{
			panel.SetActive(true);
		}

		if (text_1 != null)
		{
			text_1.GetComponent<TMP_Text>().text = "Name: " + objName;
		}

		if (text_2 != null)
		{
			text_2.GetComponent<TMP_Text>().text = "X-Position: " + objPosX;
		}

		if (text_3 != null)
		{
			text_3.GetComponent<TMP_Text>().text = "Y-Position: " + objPosY;
		}

		if (text_4 != null)
		{
			text_4.GetComponent<TMP_Text>().text = "Entity: Recharge station";
		}

		if (text_5 != null)
		{
			text_5.GetComponent<TMP_Text>().text = "";
		}

		if (text_6 != null)
		{
			text_6.GetComponent<TMP_Text>().text = "";
		}

		if (text_7 != null)
		{
			text_7.GetComponent<TMP_Text>().text = "";
		}

		if (text_8 != null)
		{
			text_8.GetComponent<TMP_Text>().text = "";
		}

		if (sprite != null)
		{
			sprite.sprite = objSprite;
		}

		if (slider != null)
		{
			slider.SetActive(false);
		}
	}

	public static void SetVisibleStorage(string objName, int objPosX, int objPosY, int objWoodStored, int objStoneStored, int objChestStored, Sprite objSprite)
	{
		inspectedObj = objName;

		//Activate the panel
		if (panel != null)
		{
			panel.SetActive(true);
		}

		if (text_1 != null)
		{
			text_1.GetComponent<TMP_Text>().text = "Name: " + objName;
		}

		if (text_2 != null)
		{
			text_2.GetComponent<TMP_Text>().text = "X-Position: " + objPosX;
		}

		if (text_3 != null)
		{
			text_3.GetComponent<TMP_Text>().text = "Y-Position: " + objPosY;
		}

		if (text_4 != null)
		{
			text_4.GetComponent<TMP_Text>().text = "Wood Stored: " + objWoodStored;
		}

		if (text_5 != null)
		{
			text_5.GetComponent<TMP_Text>().text = "Stone Stored: " + objStoneStored;
		}

		if (text_6 != null)
		{
			text_6.GetComponent<TMP_Text>().text = "Chests Stored: " + objStoneStored;
		}

		if (text_7 != null)
		{
			text_7.GetComponent<TMP_Text>().text = "Entity: Storage";
		}

		if (text_8 != null)
		{
			text_8.GetComponent<TMP_Text>().text = "";
		}

		if (sprite != null)
		{
			sprite.sprite = objSprite;
		}

		if(slider != null)
		{
			slider.SetActive(false);
		}
	}

	public static void ClosePanel()
	{
		if(panel != null)
		{
			panel.SetActive(false);
		}
	}

	public void OnSliderChange(float value)
	{
		textSlider.GetComponent<TMP_Text>().text = ((int)value).ToString();
	}

	public void UpdateBattery()
	{
		Dictionary<string, int> update = new Dictionary<string, int>();
		update.Add("battery-amount_" + inspectedObj, int.Parse(textSlider.GetComponent<TMP_Text>().text));

		Parser.UpdateSensors(update, "SET", GameManager.GetFrame());
	}

}
