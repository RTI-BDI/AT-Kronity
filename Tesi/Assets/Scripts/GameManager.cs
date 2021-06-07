using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GridManager grid;
    [SerializeField]
    private Parser parser;
    
    private List<GameObject> collectors = new List<GameObject>();
    private List<GameObject> producers = new List<GameObject>();
    private List<GameObject> woods = new List<GameObject>();
    private List<GameObject> stones = new List<GameObject>();
    private List<GameObject> storages = new List<GameObject>();
    private List<GameObject> rechargeStations = new List<GameObject>();

    bool test;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        parser.Parse();
        grid.GenerateGrid();
        InstantiateGame();

        test = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("g"))
            StartCoroutine(collectors[0].GetComponent<Collector>().CollectWood(grid.GetTileSize(), 120));

        test = false;
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
            if(init.node.belief != null)
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
            }
        }

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

        Destroy(referenceCollector);
        Destroy(referenceProducer);
        Destroy(referenceWood);
        Destroy(referenceStone);
        Destroy(referenceStorage);
        Destroy(referenceRechargeStation);
    }


}
