using System.Collections;
using System.Collections.Generic;
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

}
