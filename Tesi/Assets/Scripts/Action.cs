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

    public Action(string name, List<Parameter> parameters, string duration, List<Expression> conditions, List<Expression> effects)
    {
        this.name = name;
        this.parameters = parameters;
        this.duration = duration;
        this.conditions = conditions;
        this.effects = effects;
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

            return new Action(name, parameters, duration, conditions, effects);

        } else {
            return null;
        }
    }

    //Translate from Object to PDDL
    public string ToPDDL()
    {
        string pddl = "";

        pddl = pddl + "(:durative-action " + this.name + "\n";

        //Parameters
        pddl = pddl + ":parameters (";
        foreach(Parameter p in this.parameters)
        {
            pddl = pddl + p.ToPDDL(true);
        }
        pddl = pddl + ")\n";

        //Duration
        pddl = pddl + ":duration (= ?duration " + this.duration + ")\n";

        //Conditions
        pddl = pddl + ":condition (and\n";
        foreach (Expression e in this.conditions)
        {
            pddl = pddl + e.ToPDDL(true) + "\n";
        }
        pddl = pddl + ")\n";

        //Effects
        pddl = pddl + ":effect (and\n";
        foreach (Expression e in this.effects)
        {
            pddl = pddl + e.ToPDDL(true) + "\n";
        }
        pddl = pddl + ")\n";

        pddl = pddl + ")";

        return pddl;
    }
}
