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
    [SerializeField]
    private List<GameObject> collectors;

    // Start is called before the first frame update
    void Start()
    {
        parser.Parse();
        grid.GenerateGrid();
        InstantiateGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void InstantiateGame()
    {
        //References
        GameObject referenceCollector = (GameObject)Instantiate(Resources.Load("Collector"));

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
                        collector.GetComponent<Collector>().SetAttribute(init.exp_1.node.belief.name, init.exp_2.node.value);
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

        Destroy(referenceCollector);
    }

}
