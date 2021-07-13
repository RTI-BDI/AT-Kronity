using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GridManager grid;
	private static float tileSize;
	private static Vector2 gridPosition;
	[SerializeField]
	private Parser setParse;
    private static Parser parser;

    private static Dictionary<string, int> constants = new Dictionary<string, int>();

    private List<GameObject> collectors = new List<GameObject>();
    private List<GameObject> producers = new List<GameObject>();
    private List<GameObject> woods = new List<GameObject>();
    private List<GameObject> stones = new List<GameObject>();
    private List<GameObject> storages = new List<GameObject>();
    private List<GameObject> rechargeStations = new List<GameObject>();
    
    private Client client;

	private enum State
	{
		Playing,
		Pause,
		Waiting,
		Planning
	}

	private static int frame;
	private State state;

	// Start is called before the first frame update
	void Start()
    {
        Application.targetFrameRate = 60;

		//Initialize the game
		parser = setParse;
		parser.Parse();
		constants = new Dictionary<string, int>();

		InstantiateGame();

		//Reset static variables
		frame = 0;
		GoPause();

		//Visual Instantiation of grid and objects
		grid.SetGridSize(constants["grid-size"]);
		grid.GenerateGrid();
		tileSize = grid.GetTileSize();
		gridPosition = grid.GetPostion();

		PositionEntities();

		//Client client = new Client();
		//Debug.Log("OK");
		//client.SendMessage("EXIT");
	}

	// Update is called once per frame
	void Update()
    {

		switch (state)
		{
			case State.Playing:
				frame++;
				UIManager.UpdateFrameText(frame);
				break;
			case State.Pause:
				UIManager.UpdateFrameText(frame);
				break;
			default:
				break;
		}

        if(Input.GetKeyDown("g"))
            producers[0].GetComponent<Producer>().MoveUp();
    }
    
    private void InstantiateGame()
    {
        //References
        GameObject referenceCollector = (GameObject)Instantiate(Resources.Load("Collector"));
        GameObject referenceProducer = (GameObject)Instantiate(Resources.Load("Producer"));
        GameObject referenceWood = (GameObject)Instantiate(Resources.Load("Wood"));
        GameObject referenceStone = (GameObject)Instantiate(Resources.Load("Stone"));
        GameObject referenceStorage = (GameObject)Instantiate(Resources.Load("Storage"));
        GameObject referenceRechargeStation = (GameObject)Instantiate(Resources.Load("RechargeStation"));

        //Instantiate GameObjects
        foreach (Parameter obj in parser.GetProblem().objects)
        {
            switch (obj.type)
            {
                case "collector":        
                    GameObject collector = (GameObject)Instantiate(referenceCollector);
                    collector.GetComponent<Collector>().SetName(obj.name);
                    collectors.Add(collector);
                    break;
                case "producer":
                    GameObject producer = (GameObject)Instantiate(referenceProducer);
                    producer.GetComponent<Producer>().SetName(obj.name);
                    producers.Add(producer);
                    break;
                case "wood":
                    GameObject wood = (GameObject)Instantiate(referenceWood);
                    wood.GetComponent<Wood>().SetName(obj.name);
                    woods.Add(wood);
                    break;
                case "stone":
                    GameObject stone = (GameObject)Instantiate(referenceStone);
                    stone.GetComponent<Stone>().SetName(obj.name);
                    stones.Add(stone);
                    break;
                case "storage":
                    GameObject storage = (GameObject)Instantiate(referenceStorage);
                    storage.GetComponent<Storage>().SetName(obj.name);
                    storages.Add(storage);
                    break;
                case "r_station":
                    GameObject rechargeStation = (GameObject)Instantiate(referenceRechargeStation);
                    rechargeStation.GetComponent<RechargeStation>().SetName(obj.name);
                    rechargeStations.Add(rechargeStation);
                    break;
                default:
                    break;
            }
        }

        //Fetch attributes initializations from problem
        foreach (Expression init in parser.GetProblem().initializations)
        {
            //Not  considering constant beliefs
            if(init.exp_1.node.belief.type == Belief.BeliefType.Predicate)
            {
                //This is a predicate !!! Not yet an usage
            } else if (init.exp_1.node.belief.type == Belief.BeliefType.Function)
            {
                string type;
                type = parser.GetProblem().objects.Find(o => o.name == init.exp_1.node.belief.param[0].name).type;

                switch (type)
                {
                    case "collector":
                        GameObject collector = null;
                        foreach (GameObject g in collectors)
                        {
                            if(g.GetComponent<Collector>().GetName() == init.exp_1.node.belief.param[0].name)
                            {
                                collector = g;
                            }
                        }
                        if (collector != null)
                            collector.GetComponent<Collector>().SetAttribute(init.exp_1.node.belief.name, init.exp_2.node.value);
                        break;
                    case "producer":
                        GameObject producer = null;
                        foreach (GameObject g in producers)
                        {
                            if(g.GetComponent<Producer>().GetName() == init.exp_1.node.belief.param[0].name)
                            {
                                producer = g;
                            }
                        }
                        if (producer != null)
                            producer.GetComponent<Producer>().SetAttribute(init.exp_1.node.belief.name, init.exp_2.node.value);
                        break;
                    case "wood":
                        GameObject wood = null;
                        foreach (GameObject g in woods)
                        {
                            if (g.GetComponent<Wood>().GetName() == init.exp_1.node.belief.param[0].name)
                            {
                                wood = g;
                            }
                        }
                        if (wood != null)
                            wood.GetComponent<Wood>().SetAttribute(init.exp_1.node.belief.name, init.exp_2.node.value);
                        break;
                    case "stone":
                        GameObject stone = null;
                        foreach (GameObject g in stones)
                        {
                            if (g.GetComponent<Stone>().GetName() == init.exp_1.node.belief.param[0].name)
                            {
                                stone = g;
                            }
                        }
                        if(stone != null)
                            stone.GetComponent<Stone>().SetAttribute(init.exp_1.node.belief.name, init.exp_2.node.value);
                        break;
                    case "storage":
                        GameObject storage = null;
                        foreach (GameObject g in storages)
                        {
                            if (g.GetComponent<Storage>().GetName() == init.exp_1.node.belief.param[0].name)
                            {
                                storage = g;
                            }
                        }
                        if (storage != null)
                            storage.GetComponent<Storage>().SetAttribute(init.exp_1.node.belief.name, init.exp_2.node.value);
                        break;
                    case "r_station":
                        GameObject rechargeStation = null;
                        foreach (GameObject g in rechargeStations)
                        {
                            if (g.GetComponent<RechargeStation>().GetName() == init.exp_1.node.belief.param[0].name)
                            {
                                rechargeStation = g;
                            }
                        }
                        if (rechargeStation != null)
                            rechargeStation.GetComponent<RechargeStation>().SetAttribute(init.exp_1.node.belief.name, init.exp_2.node.value);
                        break;
                    default:
                        break;
                }
            } else
            {
                //Enter this scope if the expression represents a constant
                constants.Add(init.exp_1.node.belief.name, int.Parse(init.exp_2.node.value));
            }
        }

        Destroy(referenceCollector);
        Destroy(referenceProducer);
        Destroy(referenceWood);
        Destroy(referenceStone);
        Destroy(referenceStorage);
        Destroy(referenceRechargeStation);
    }

	private void PositionEntities()
	{
		//Move the controllers to their location
		foreach (GameObject g in collectors)
		{
			g.GetComponent<Collector>().MoveToDestination(grid.GetTileSize(), grid.GetPostion());
		}
		foreach (GameObject g in producers)
		{
			g.GetComponent<Producer>().MoveToDestination(grid.GetTileSize(), grid.GetPostion());
		}
		foreach (GameObject g in woods)
		{
			g.GetComponent<Wood>().MoveToDestination(grid.GetTileSize(), grid.GetPostion());
		}
		foreach (GameObject g in stones)
		{
			g.GetComponent<Stone>().MoveToDestination(grid.GetTileSize(), grid.GetPostion());
		}
		foreach (GameObject g in storages)
		{
			g.GetComponent<Storage>().MoveToDestination(grid.GetTileSize(), grid.GetPostion());
		}
		foreach (GameObject g in rechargeStations)
		{
			g.GetComponent<RechargeStation>().MoveToDestination(grid.GetTileSize(), grid.GetPostion());
		}
	}

	public static int GetBatteryDecrease(string actionName)
	{
		Action a = parser.retrieveAction(actionName);
		foreach (Expression pc in a.postConditions)
		{
			if(pc.exp_1.node.value == "decrease" && pc.exp_1.exp_1.node.belief.name == "battery-amount")
			{
				return int.Parse(pc.exp_1.exp_2.node.value);
			}
		}

		return 0;
	}

	public static float GetTileSize()
	{
		return tileSize;
	}

	public static Vector2 GetGridPosition()
	{
		return gridPosition;
	}

	public static int GetFrame()
	{
		return frame;
	}

	public static Dictionary<string, int> GetConstants()
	{
		return constants;
	}

	public void GoPause()
	{
		state = State.Pause;
		UIManager.InPause();

		foreach (GameObject c in collectors)
		{
			c.GetComponent<Collector>().Pause();
		}
		foreach (GameObject p in producers)
		{
			p.GetComponent<Producer>().Pause();
		}
		foreach (GameObject s in storages)
		{
			s.GetComponent<Storage>().Pause();
		}
	}

	public void GoPlay()
	{
		state = State.Playing;
		UIManager.InPlay();

		foreach(GameObject c in collectors)
		{
			c.GetComponent<Collector>().Resume();
		}
		foreach (GameObject p in producers)
		{
			p.GetComponent<Producer>().Resume();
		}
		foreach (GameObject s in storages)
		{
			s.GetComponent<Storage>().Resume();
		}
	}

	private string ExtractAction(string groundedAction)
	{
		string result = string.Empty;

		bool stop = false;
		foreach (Char c in groundedAction)
		{
			if (!stop)
			{
				if(c == '_')
				{
					stop = true;
				} else
				{
					result = result + c;
				}
			}
		}

		return parser.MapAction(result);
	}

	private List<string> ExtractEntities(string groundedAction)
	{
		bool addingEntity = false;

		List<string> result = new List<string>();
		string tempResult = "";

		int counter = 0;

		foreach (Char c in groundedAction)
		{
			if(c == '_')
			{
				if (addingEntity)
				{
					result.Add(tempResult);
				}

				addingEntity = true;
				tempResult = string.Empty;
			} else
			{
				if (addingEntity)
				{
					tempResult = tempResult + c;
				}
			}

			if(counter == groundedAction.Length - 1)
			{
				result.Add(tempResult);

			}

			counter++;
		}

		return result;
	}

	private GameObject SearchEntity(string name)
	{
		foreach (GameObject c in collectors)
		{
			if(c.GetComponent<Collector>().GetName().Equals(name))
			{
				return c;
			}
		}

		foreach (GameObject p in producers)
		{
			if (p.GetComponent<Producer>().GetName().Equals(name))
			{
				return p;
			}
		}

		foreach (GameObject s in storages)
		{
			if (s.GetComponent<Storage>().GetName().Equals(name))
			{
				return s;
			}
		}

		return null;
	}

	private void ExecuteAction(string action, GameObject entity)
	{
		entity.SendMessage(action);
	}
}
