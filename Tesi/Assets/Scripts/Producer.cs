using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Producer : MonoBehaviour
{

    [SerializeField]
    private string name;
    [SerializeField]
    private int posX;
    [SerializeField]
    private int posY;
    [SerializeField]
    private int batteryAmount;
    [SerializeField]
    private int woodAmount;
    [SerializeField]
    private int stoneAmount;
    [SerializeField]
    private int chestAmount;
    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private Sprite sprite_1;
    [SerializeField]
    private Sprite sprite_2;
    [SerializeField]
    private Sprite sprite_3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return this.name;
    }

    public void SetAttribute(string attribute, string value)
    {
        switch (attribute)
        {
            case "battery-amount":
                this.batteryAmount = int.Parse(value);
                break;
            case "posX":
                this.posX = int.Parse(value);
                break;
            case "posY":
                this.posY = int.Parse(value);
                break;
            case "wood-amount":
                this.woodAmount = int.Parse(value);
                break;
            case "stone-amount":
                this.stoneAmount = int.Parse(value);
                break;
            case "chest-amount":
                this.chestAmount = int.Parse(value);
                break;
            default:
                break;
        }
    }

    public void MoveToDestination(float tileSize, Vector2 position)
    {
        gameObject.transform.position = new Vector2(position.x + (posX * tileSize) - (tileSize/2), position.y + (posY * tileSize) + (tileSize/2));
    }

    public IEnumerator MoveUp(float tileSize)
    {
        int actionTime = 120;
        for (int i = 0; i < actionTime; i++)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + tileSize / actionTime);
            yield return null;
        }

        this.posY++;

    }

    public IEnumerator MoveDown(float tileSize)
    {
        int actionTime = 120;
        for (int i = 0; i < actionTime; i++)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - tileSize / actionTime);
            yield return null;
        }

        this.posY--;

    }

    public IEnumerator MoveRight(float tileSize)
    {
        int actionTime = 120;
        for (int i = 0; i < actionTime; i++)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + tileSize / actionTime, gameObject.transform.position.y);
            yield return null;
        }

        this.posX++;

    }

    public IEnumerator MoveLeft(float tileSize)
    {
        int actionTime = 120;
        for (int i = 0; i < actionTime; i++)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - tileSize / actionTime, gameObject.transform.position.y);
            yield return null;
        }

        this.posX--;

    }

    public IEnumerator Recharge(float tileSize, int batteryCapacity, int actionTime)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Recharging...";

        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Recharging";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Recharging.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Recharging..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Recharging...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Recharging";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Recharging";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Recharging.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Recharging..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Recharging...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Recharging";
            }
            yield return null;
        }

        //Actual effect
        this.batteryAmount = batteryCapacity;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

    public IEnumerator DropWood(float tileSize)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Dropping Wood...";

        int actionTime = 120;

        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Wood";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Wood.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Dropping Wood..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Wood...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Wood";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Wood";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Wood.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Dropping Wood..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Wood...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Wood";
            }
            yield return null;
        }

        //Actual effect
        this.woodAmount--;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

    public IEnumerator DropStone(float tileSize)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Dropping Stone...";

        int actionTime = 120;

        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Stone";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Stone.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Dropping Stone..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Stone...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Stone";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Stone";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Stone.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Dropping Stone..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Stone...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Stone";
            }
            yield return null;
        }

        //Actual effect
        this.stoneAmount--;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

    public IEnumerator DropChest(float tileSize)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Dropping Chest...";

        int actionTime = 120;

        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Chest";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Chest.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Dropping Chest..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Chest...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Chest";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Chest";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Chest.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Dropping Chest..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Dropping Chest...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Dropping Chest";
            }
            yield return null;
        }

        //Actual effect
        this.chestAmount--;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

    public IEnumerator RetrieveWood(float tileSize)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Exchanging Wood...";

        int actionTime = 120;

        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Exchanging Wood";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Exchanging Wood.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Exchanging Wood..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Exchanging Wood...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Exchanging Wood";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Exchanging Wood";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Exchanging Wood.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Exchanging Wood..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Exchanging Wood...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Exchanging Wood";
            }
            yield return null;
        }

        //Actual effect
        this.woodAmount++;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

    public IEnumerator RetrieveStone(float tileSize)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Exchanging Stone...";

        int actionTime = 120;

        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Exchanging Stone";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Exchanging Stone.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Exchanging Stone..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Exchanging Stone...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Exchanging Stone";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Exchanging Stone";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Exchanging Stone.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Exchanging Stone..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Exchanging Stone...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Exchanging Stone";
            }
            yield return null;
        }

        //Actual effect
        this.stoneAmount++;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

    public IEnumerator ProduceChest(float tileSize)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Producing Chest...";

        int actionTime = 120;

        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Producing Chest";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Producing Chest.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Producing Chest..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Producing Chest...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Producing Chest";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Producing Chest";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Producing Chest.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Producing Chest..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Producing Chest...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Producing Chest";
            }
            yield return null;
        }

        //Actual effect
        this.woodAmount--;
        this.stoneAmount--;
        this.chestAmount++;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

}
