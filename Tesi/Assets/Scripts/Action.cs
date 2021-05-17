using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Action
{
    //Action's name
    public string name;
    //List of parameters (not bound to expressions' parameters)
    public List<Parameter> parameters;
    //Action's duration
    public string duration;
    //List of expression representing the preconditions (AND-connected)
    public List<Expression> conditions;
    //List of expression representing the effects (AND-connected)
    public List<Expression> effects;
    //Kronosim parameters
    public int period;
    public int computation_cost;

    public Action(string name, List<Parameter> parameters, string duration, List<Expression> conditions, List<Expression> effects, int period, int computation_cost)
    {
        this.name = name;
        this.parameters = parameters;
        this.duration = duration;
        this.conditions = conditions;
        this.effects = effects;
        this.period = period;
        this.computation_cost = computation_cost;
    }

    //Translate from JSON to Object
    public static Action Evaluate(JToken token)
    {
        if(token.First.ToString() == "action")
        {
            //2nd element in token
            string name = token.First.Next.ToString();

            //3rd element in token
            List<Parameter> parameters = new List<Parameter>();
            if(token.First.Next.Next.First.ToString() == "defineParameters")
            {
                parameters = Parameter.Evaluate(token.First.Next.Next.First.Next);
            }

            //4th element in token
            string duration = token.First.Next.Next.Next.First.Next.ToString();

            //5th element in token
            List<Expression> conditions = new List<Expression>();
            foreach  (JToken j in token.First.Next.Next.Next.Next)
            {
                if(j != token.First.Next.Next.Next.Next.First)
                    conditions.Add(Expression.Evaluate(j));
            }

            //6th element in token
            List<Expression> effects = new List<Expression>();
            foreach (JToken j in token.First.Next.Next.Next.Next.Next)
            {
                if (j != token.First.Next.Next.Next.Next.Next.First)
                    effects.Add(Expression.Evaluate(j));
            }

            //7th element in token
            int period = 0;
            if(token.First.Next.Next.Next.Next.Next.Next.First.ToString() == "kronosim_period")
            {
                period = int.Parse(token.First.Next.Next.Next.Next.Next.Next.First.Next.ToString());
            }

            //8th element in token
            int computation_cost = 0;
            if (token.First.Next.Next.Next.Next.Next.Next.Next.First.ToString() == "kronosim_computation_cost")
            {
                computation_cost = int.Parse(token.First.Next.Next.Next.Next.Next.Next.Next.First.Next.ToString());
            }

            return new Action(name, parameters, duration, conditions, effects, period, computation_cost);

        } else {
            return null;
        }
    }

    //Translate from Object to PDDL
    public string ToPDDL(bool questionMark)
    {
        string pddl = "";

        pddl = pddl + "(:durative-action " + this.name + "\n";

        //Parameters
        pddl = pddl + ":parameters (";
        foreach(Parameter p in this.parameters)
        {
            pddl = pddl + p.ToPDDL(questionMark) + " ";
        }
        pddl = pddl + ")\n";

        //Duration
        pddl = pddl + ":duration (= ?duration " + this.duration + ")\n";

        //Conditions
        pddl = pddl + ":condition (and\n";
        foreach (Expression e in this.conditions)
        {
            pddl = pddl + e.ToPDDL(questionMark) + "\n";
        }
        pddl = pddl + ")\n";

        //Effects
        pddl = pddl + ":effect (and\n";
        foreach (Expression e in this.effects)
        {
            pddl = pddl + e.ToPDDL(questionMark) + "\n";
        }
        pddl = pddl + ")\n";

        pddl = pddl + ")";

        return pddl;
    }

    //Translate from Object to Desire (json)
    public string ToDesire()
    {
        string result = "";
        result = result + "{ \"priority\" : { \"computedDynamically\" : true, \"formula\" : [ 0.8 ], \"reference_table\" : [] },";
        result = result + "\"deadline\" : 1000.0,";
        result = result + "\"preconditions\" : [ [ \"AND\", ";

        int counter = 0;
        foreach (Expression e in this.conditions)
        {
            result = result + e.ToDesire();

            if (counter != this.conditions.Count - 1)
            {
                result = result + ", ";
            }
            counter++;
        }

        result = result + " ] ],";
        result = result + " \"goal_name\" : \"" + this.name;
        foreach (Parameter p in this.parameters)
        {
            result = result + "_" + p.name;
        }
        result = result + "\"";
        result = result + "} ";

        return result;
    }
}
