using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Globalization;

public class Parser : MonoBehaviour
{

    private Domain domainObject;
    private Problem problemObject;
    private Dictionary<Belief, int> groundedBeliefs;
    private List<Action> groundedActions;
    private Plan plan;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Domain GetDomain()
    {
        return domainObject;
    }

    public Problem GetProblem()
    {
        return problemObject;
    }

    public Dictionary<Belief, int> GetGroundedBeliefs()
    {
        return groundedBeliefs;
    }

    public List<Action> GetGroundedActions()
    {
        return groundedActions;
    }

    public Plan GetPlan()
    {
        return plan;
    }

	public Action retrieveAction(string actionName)
	{
		return domainObject.actions.Find(a => a.name == actionName);
	}

    public void Parse()
    {
        //Read Json file to string
        string jsonDomain = File.ReadAllText("./Assets/JSON/Domain.json");
        string jsonProblem = File.ReadAllText("./Assets/JSON/Problem.json");

        //Initial parsing
        JObject objectDomain = JObject.Parse(jsonDomain);
        JArray domain = objectDomain["domain"] as JArray;

        JObject objectProblem = JObject.Parse(jsonProblem);
        JArray problem = objectProblem["problem"] as JArray;

        //Evalutation
        domainObject = Domain.Evaluate(domain);
        problemObject = Problem.Evaluate(problem);

        WritePDDLOnFile(domainObject.ToPDDL(), "DomainPDDL.pddl");
        WritePDDLOnFile(problemObject.ToPDDL(), "ProblemPDDL.pddl");

        //Belief grounding
        groundedBeliefs = GenerateBeliefGrounding(domainObject, problemObject);
        GenerateBeliefSet(groundedBeliefs);

        //ActionGrounding
        groundedActions = GenerateActionGrounding(domainObject, problemObject);
        GenerateSkillSet(groundedActions);
        GenerateDesireSet(problemObject, groundedActions);

        //Plan generation
        string[] plainPlan = File.ReadAllLines("./Assets/JSON/PlainPlan.txt");
        plan = Plan.FromPlainToObject(plainPlan, groundedActions);
        GeneratePlanSet(plan, problemObject);

		GenerateServers();
    }

    public void WritePDDLOnFile(string PDDL, string file)
    {
        string path = "./Assets/PDDL/" + file;
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine(PDDL);
        }
    }

    /*GenerateBeliefSet steps:
     * 1 - Extract from non-constant beliefs the types of the parameters (called involvedTypes)
     * 2 - For each belief, generates a list of lists of parameters; each list represents a differents 'version' of the belief
     * 3 - Creates a new belief for each 'version'
     * 4 - Initialize the beliefs that appear in the problem file (otherwise, the belief will have value 0)
     * 5 - Translates the beliefs in JSON
    */
    public Dictionary<Belief, int> GenerateBeliefGrounding(Domain domain, Problem problem)
    {
        Dictionary<Belief, int> generatedBeliefs = new Dictionary<Belief, int>();
        //Belief grounding
        foreach(Belief b in domain.beliefs)
        {
            if (b.type != Belief.BeliefType.Constant)
            {
                List<List<Parameter>> grounding = new List<List<Parameter>>();
                foreach (Parameter p in b.param)
                {
                    //extract all related types to this objects
                    List<string> involvedTypes = ExtractInvolvedTypes(p.type, domain);

                    //extract from problem the right objects
                    List<Parameter> involvedObjects = new List<Parameter>();
                    foreach (Parameter o in problem.objects)
                    {
                        if (involvedTypes.Contains(o.type))
                        {
                            involvedObjects.Add(o);
                        }
                    }
                    grounding.Add(involvedObjects);
                }

                List<List<Parameter>> beliefsVersions = ExtractVersions(grounding);

                //create the actual beliefs based on all the versions
                foreach (List<Parameter> lp in beliefsVersions)
                {
                    Belief toAdd = new Belief(b.type, b.name, new List<Parameter>(lp));
                    generatedBeliefs.Add(toAdd, 0);
                }
            } else
            {
                generatedBeliefs.Add(b, 0);
            }
        }

        List<Belief> keys = new List<Belief>(generatedBeliefs.Keys);

        foreach (Belief b in keys)
        {
            foreach (Expression e in problem.initializations)
            {
                if (b.type != Belief.BeliefType.Constant && e.exp_1.node.belief != null && e.exp_1.node.belief.type != Belief.BeliefType.Constant)
                {
                    if (b.Equals(e.exp_1.node.belief))
                    {
                        
                        if (b.type == Belief.BeliefType.Function)
                        {
                            generatedBeliefs[b] = int.Parse(e.exp_2.node.value);
                        }
                        else
                        {
                            if (e.node.value == "true")
                            {
                                generatedBeliefs[b] = 1;
                            }
                        }
                    }
                } else if (b.type == Belief.BeliefType.Constant && e.exp_1.node.belief != null && e.exp_1.node.belief.type == Belief.BeliefType.Constant)
                {
                    if(b.name == e.exp_1.node.belief.name)
                    {
                        generatedBeliefs[b] = int.Parse(e.exp_2.node.value);
                    }
                }
            }
        }

        return generatedBeliefs;
    }

    //Utility function to extract involved types; Used in GenerateBeliefGrounding & GenerateActionGrounding
    public List<string> ExtractInvolvedTypes(string type, Domain domain)
    {
        List<string> result = new List<string>();
        result.Add(type);

        Type relatedType = domain.types.Find(t => t.typeOf == type);
        if(relatedType != null)
            foreach (string s in relatedType.subjects)
            {
                result.AddRange(ExtractInvolvedTypes(s, domain));
            }

        return result;
    }

    //Utility function to extract all the version of the same belief/action; Used in GenerateBeliefGrounding & GenerateActionGrounding
    public List<List<Parameter>> ExtractVersions(List<List<Parameter>> grounding)
    {
        
        if(grounding.Count == 1)
        {
            //base case
            List<List<Parameter>> result = new List<List<Parameter>>();
            foreach (Parameter p in grounding[0])
            {
                List<Parameter> temp = new List<Parameter>();
                temp.Add(p);
                result.Add(temp);
            }
            return result;

        } else {
            //iterative case 
            List<Parameter> temp = grounding[0];
            grounding.RemoveAt(0);

            List<List<Parameter>> tempResult = ExtractVersions(grounding);
            List<List<Parameter>> newTempResult = new List<List<Parameter>>();

            foreach (Parameter p_t in temp)
            {
                foreach (List<Parameter> lp_r in tempResult)
                {
                    List<Parameter> newTemp = new List<Parameter>();
                    newTemp.Add(p_t);
                    foreach (Parameter p_r in lp_r)
                    {
                        newTemp.Add(p_r);
                    }
                    newTempResult.Add(newTemp);
                }
            }

            return newTempResult;

        }
    } 

    //Function to generate the BeliefSet for Kronosim
    public void GenerateBeliefSet(Dictionary<Belief, int> beliefs)
    {
        string jsonStr = "{ \"0\": [ ";

        List<Belief> keys = new List<Belief>(beliefs.Keys);

        int counter = 0;
        foreach (Belief b in keys)
        {
            jsonStr = jsonStr + BeliefToJson(b, beliefs[b]);
            if (counter != beliefs.Count - 1)
            {
                jsonStr = jsonStr + ",\n";
            }
            counter++;
        }

        jsonStr = jsonStr + " ] }";
        JObject jobject = JObject.Parse(jsonStr);

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText("./Assets/kronosim/inputs/beliefset.json"))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            writer.Formatting = Formatting.Indented;
            jobject.WriteTo(writer);
        }

        using (StreamWriter file = File.CreateText("./Assets/kronosim/inputs/update-beliefSet.json"))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            writer.Formatting = Formatting.Indented;
            jobject.WriteTo(writer);
        }
    }

    //Utility function used to translate a single belief into JSON; Used in GenerateBeliefSet
    public string BeliefToJson(Belief belief, int value)
    {
        string result = "";
        result = result + "{ \n";
        result = result + "\"name\" : \"" + belief.GetGroundedName() + "\", \n";
		if(belief.type == Belief.BeliefType.Predicate)
		{
			if(value == 0)
			{
				result = result + "\"value\" : false \n";
			} else
			{
				result = result + "\"value\" : true \n";
			}
		} else
		{
			result = result + "\"value\" : " + value + "\n";
		}
        
        result = result + "}";
        return result;
    }

    //Generate all the versions of all the actions present in the domain
    public List<Action> GenerateActionGrounding(Domain domain, Problem problem)
    {
        List<Action> generatedActions = new List<Action>();
        //Action grounding
        foreach (Action a in domain.actions)
        {
            List<List<Parameter>> grounding = new List<List<Parameter>>();
            foreach (Parameter p in a.parameters)
            {
                //extract all related types to this objects
                List<string> involvedTypes = ExtractInvolvedTypes(p.type, domain);

                //extract from problem the right objects
                List<Parameter> involvedObjects = new List<Parameter>();
                foreach (Parameter o in problem.objects)
                {
                    if (involvedTypes.Contains(o.type))
                    {
                        involvedObjects.Add(o);
                    }
                }
                grounding.Add(involvedObjects);

            }

            List<List<Parameter>> actionVersions = ExtractVersions(grounding);

            //create the actual actions based on all the versions
            foreach (List<Parameter> lp in actionVersions)
            {
                List<Expression> newConditions = new List<Expression>();
                List<Expression> newEffects = new List<Expression>();
                List<Expression> newPostConditions = new List<Expression>();
                foreach (Expression e in a.conditions)
                {
                    newConditions.Add(GenerateInstantiatedExpression(e, lp, a.parameters));
                }
                foreach (Expression e in a.effects)
                {
                    newEffects.Add(GenerateInstantiatedExpression(e, lp, a.parameters));
                }
                foreach (Expression e in a.postConditions)
                {
                    newPostConditions.Add(GenerateInstantiatedExpression(e, lp, a.parameters));
                }
                
                generatedActions.Add(new Action(a.name, new List<Parameter>(lp), a.duration, new List<Expression>(newConditions), new List<Expression>(newEffects), new List<Expression>(newPostConditions), a.period, a.computation_cost));
            }
        }
        return generatedActions;
    }

    //Function to generate the SkillSet for Kronosim
    public void GenerateSkillSet(List<Action> groundedActions)
    {
        string jsonStr = "{ \"0\": [ ";

        jsonStr = jsonStr + "{ \"goal_name\" : \"plan_execution\" }, ";

        int counter = 0;
        foreach (Action a in groundedActions)
        {

            jsonStr = jsonStr + "{ \"goal_name\" : \"" + a.GetGroundedName() + "\" } ";

            if (counter != groundedActions.Count - 1)
            {
                jsonStr = jsonStr + ",\n";
            }
            counter++;
        }

        jsonStr = jsonStr + " ] }";

        JObject jobject = JObject.Parse(jsonStr);

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText("./Assets/kronosim/inputs/skillset.json"))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            writer.Formatting = Formatting.Indented;
            jobject.WriteTo(writer);
        }
    }

    //Function to generate the desire of the Plan
    public void GenerateDesireSet(Problem problem, List<Action> groundedActions)
    {
        string jsonStr = "{ \"0\": [ ";

        jsonStr = jsonStr + "{ \"priority\" : { \"computedDynamically\" : true, \"formula\" : [ 0.8 ], \"reference_table\" : [] },";
        jsonStr = jsonStr + "\"deadline\" : -1,";
        jsonStr = jsonStr + "\"preconditions\" : [ [ \"AND\", ";

        jsonStr = jsonStr + ToKronosimExpressionInitializations(problem.initializations);

        jsonStr = jsonStr + " ] ],";
        jsonStr = jsonStr + " \"goal_name\" : \"plan_execution\"";
        jsonStr = jsonStr + "} ";

        // To generate all the desires
        //foreach (Action a in groundedActions)
        //{
        //    jsonStr = jsonStr + ", " + a.ToDesire();
        //}

        jsonStr = jsonStr + " ] }";

        JObject jobject = JObject.Parse(jsonStr);

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText("./Assets/kronosim/inputs/desireset.json"))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            writer.Formatting = Formatting.Indented;
            jobject.WriteTo(writer);
        }
    }

    //Translate list of expressions into a json expression readable by kronosim
    public string ToKronosimExpressionInitializations(List<Expression> le)
    {
        string jsonStr = "";

        int counter = 0;
        foreach (Expression e in le)
        {
            jsonStr = jsonStr + e.ToKronosimExpInitialization();

            if (counter != le.Count - 1)
            {
                jsonStr = jsonStr + ", ";
            }
            counter++;
        }

        return jsonStr;
    }

    //Substitute the parameters with actual instantiations of the beliefs
    public Expression GenerateInstantiatedExpression(Expression e, List<Parameter> lp, List<Parameter> originalLp)
    {
        switch (e.node.type) {
            case (Node.NodeType.LeafBelief):
                //Change the belief (not if a constant one)
                if (e.node.belief.type == Belief.BeliefType.Constant)
                    return new Expression(new Node(e.node.type, new Belief(e.node.belief.type, e.node.belief.name, null)));

                int index = 0;
                List<Parameter> tempParams = new List<Parameter>();
                foreach (Parameter p in e.node.belief.param)
                {
                    foreach (Parameter p_2 in originalLp)
                    {
                        if (p.Equals(p_2))
                        {
                            index = originalLp.IndexOf(p_2);
                            tempParams.Add(new Parameter(lp[index].name, null));
                        }
                    }
                }

                return new Expression(new Node(e.node.type, new Belief(e.node.belief.type, e.node.belief.name, tempParams)));
                break;
            case (Node.NodeType.LeafValue):
                //Return without doing a thing
                return (new Expression(new Node(e.node.type, e.node.value)));
                break;
            case (Node.NodeType.Unary):
                //Dive into the expression, then return
                return (new Expression(new Node(e.node.type, e.node.value), GenerateInstantiatedExpression(e.exp_1, lp, originalLp)));
                break;
            case (Node.NodeType.Operator):
                //Dive into the expression, then return
                return (new Expression(new Node(e.node.type, e.node.value), GenerateInstantiatedExpression(e.exp_1, lp, originalLp), GenerateInstantiatedExpression(e.exp_2, lp, originalLp)));
                break;
            default:
                return null;
                break;
        }
    }

    //function to generate the PlanSet for Kronosim
    public void GeneratePlanSet(Plan plan, Problem problem)
    {
        // -- GENERAL PLAN ---

        string jsonStr = "";

        //goal_name
        jsonStr = jsonStr + "{ \"0\": [ { \"goal_name\": \"plan_execution\", ";
        //body
        jsonStr = jsonStr + "\"body\": [ ";

        int counter = 0;
        foreach (KeyValuePair<float, Action> entry in plan.steps)
        {
            jsonStr = jsonStr + "{ \"action\": { \"priority\": 0.5, \"deadline\": " + float.Parse(Plan.CommaToDotDuration(entry), CultureInfo.InvariantCulture) + ", ";
            jsonStr = jsonStr + "\"goal_name\": \"" + entry.Value.GetGroundedName() + "\", ";
            jsonStr = jsonStr + "\"arrivalTime\": " + Plan.CommaToDotArrivalTime(entry) + " }, ";
            jsonStr = jsonStr + "\"action_type\": \"GOAL\", ";

            if (counter == 0)
            {
                jsonStr = jsonStr + "\"execution\": \"SEQUENTIAL\", ";
            } else
            {
                jsonStr = jsonStr + "\"execution\": \"PARALLEL\", ";
            }

            jsonStr = jsonStr + " }";

            if (counter != plan.steps.Count - 1)
            {
                jsonStr = jsonStr + ", ";
            }
            counter++;
        }

        jsonStr = jsonStr + "], ";

        //cont_conditions
        jsonStr = jsonStr + "\"cont_conditions\": [ ], ";

        //effects_at_begin
        jsonStr = jsonStr + "\"effects_at_begin\": [ ], ";

        //effects_at_end
        jsonStr = jsonStr + "\"effects_at_end\": [ ], ";

        //post_coonditions
        jsonStr = jsonStr + "\"post-conditions\": [ ], ";

        //preconditions
        jsonStr = jsonStr + "\"preconditions\": [ [ \"AND\", ";
        jsonStr = jsonStr + ToKronosimExpressionInitializations(problem.initializations);
        jsonStr = jsonStr + " ] ], ";

        //preference
        jsonStr = jsonStr + "\"preference\" : { \"computedDynamically\" : true, \"formula\" : [ 0.8 ], \"reference_table\" : [ ] } ";

        jsonStr = jsonStr + "}";

        //Sub-goals
        jsonStr = jsonStr + plan.ToSubGoals();

        jsonStr = jsonStr + " ] }";

        
        JObject jobject = JObject.Parse(jsonStr);

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText("./Assets/kronosim/inputs/planset.json"))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            writer.Formatting = Formatting.Indented;
            jobject.WriteTo(writer);
        }
    }

	//function to generate the Servers file for Kronosim
	public void GenerateServers()
	{
		string jsonStr = "";
		jsonStr = "{ \"0\" : [ { \"budget\" : 5.00, \"id\" : 0, \"period\" : 20.00 } ] }";


		JObject jobject = JObject.Parse(jsonStr);

		// write JSON directly to a file
		using (StreamWriter file = File.CreateText("./Assets/kronosim/inputs/servers.json"))
		using (JsonTextWriter writer = new JsonTextWriter(file))
		{
			writer.Formatting = Formatting.Indented;
			jobject.WriteTo(writer);
		}
	}

	//function to update value using sensors
	public static void UpdateSensors(Dictionary<string, int> new_beliefs, string mode, int time)
	{
		string jsonStr = "";
		jsonStr += "{ \"0\" : [ ";

		int counter = 0;
		foreach (KeyValuePair<string, int> e in new_beliefs)
		{
			jsonStr += " { ";
			jsonStr += "\"belief_name\" : \"" + e.Key + "\", ";
			jsonStr += "\"mode\" : \"" + mode + "\", ";
			jsonStr += "\"time\" : " + time + ", ";
			jsonStr += "\"value\" : " + e.Value + " ";
			jsonStr += " } ";

			if(counter != new_beliefs.Count - 1)
			{
				jsonStr += ", ";
			}
			counter++;
		}
		
		jsonStr += " ] }";

		JObject jobject = JObject.Parse(jsonStr);

		// write JSON directly to a file
		using (StreamWriter file = File.CreateText("./Assets/kronosim/inputs/sensors.json"))
		using (JsonTextWriter writer = new JsonTextWriter(file))
		{
			writer.Formatting = Formatting.Indented;
			jobject.WriteTo(writer);
		}

	}
}
