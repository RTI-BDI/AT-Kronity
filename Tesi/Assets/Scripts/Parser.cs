using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

public class Parser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DoStuff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DoStuff()
    {
        //Read Json file to string
        string jsonDomain = File.ReadAllText("./Assets/JSON/Domain.json");
        string jsonProblem = File.ReadAllText("./Assets/JSON/Problem.json");

        //Var containing PDDL domain and problem
        string pddlDomain = "";
        string pddlProblem = "";

        //Initial parsing
        JObject objectDomain = JObject.Parse(jsonDomain);
        JArray domain = objectDomain["domain"] as JArray;

        JObject objectProblem = JObject.Parse(jsonProblem);
        JArray problem = objectProblem["problem"] as JArray;

        //Evalutation
        Domain domainObject = Domain.Evaluate(domain);
        Problem problemObject = Problem.Evaluate(problem);

        Debug.Log(domainObject.ToPDDL());
        Debug.Log(problemObject.ToPDDL());

        GenerateBeliefSet(domainObject, problemObject);

    }

    public void GenerateBeliefSet(Domain domain, Problem problem)
    {
        string jsonStr = "{ \"0\": [ ";

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

                List<List<Parameter>> beliefsVersions = ExtractBeliefsVersions(grounding);

                //create the actual beliefs based on all the versions
                foreach (List<Parameter> lp in beliefsVersions)
                {
                    Belief toAdd = new Belief(b.type, b.name, lp);
                    generatedBeliefs.Add(toAdd, 0);
                }
            }
        }

        List<Belief> keys = new List<Belief>(generatedBeliefs.Keys);

        foreach (Belief b in keys)
        {
            foreach (Expression e in problem.initializations)
            {
                if (e.exp_1.node.belief != null && e.exp_1.node.belief.type != Belief.BeliefType.Constant && e.exp_1.node.belief.Equals(b))
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
            }

        }

        foreach (KeyValuePair<Belief, int> entry in generatedBeliefs)
        {
            string debug = "";
            debug = debug + entry.Key.name + " ";
            foreach (Parameter p in entry.Key.param)
            {
                debug = debug + p.name + " ";
            }
            debug = debug + ": " + entry.Value;
            Debug.Log(debug);
        }

        jsonStr = jsonStr + " ] }";

        JObject jobject = JObject.Parse(jsonStr);

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText("./Assets/JSON/BeliefSet.json"))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            jobject.WriteTo(writer);
        }
    }

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

    public List<List<Parameter>> ExtractBeliefsVersions(List<List<Parameter>> grounding)
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

            List<List<Parameter>> tempResult = ExtractBeliefsVersions(grounding);
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

}
