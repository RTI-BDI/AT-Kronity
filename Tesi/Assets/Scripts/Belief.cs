using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Belief
{
    public enum BeliefType
    {
        Predicate,
        Function,
        Constant
    }

    public BeliefType type;
    public string name;
    public List<Parameter> param;

    public Belief(BeliefType type, string name, List<Parameter> param)
    {
        this.type = type;
        this.name = name;
        this.param = param;
    }

    public bool Equals(Belief other)
    {
        bool result = true;
        result = result && this.name == other.name && this.type.Equals(other.type);
        if(this.param.Count == other.param.Count)
        {
            for (int i = 0; i < this.param.Count; i++)
            {
                result = result && this.param[i].Equals(other.param[i]);
            }
        }

        return result;
    }

    public static Belief Evaluate(JToken token)
    {
        switch (token.First.ToString())
        {
            case ("predicate"):
                return new Belief(BeliefType.Predicate, token.First.Next.ToString(), Parameter.Evaluate(token.First.Next.Next));
                break;
            case ("function"):
                return new Belief(BeliefType.Function, token.First.Next.ToString(), Parameter.Evaluate(token.First.Next.Next));
                break;
            case ("constant"):
                return new Belief(BeliefType.Constant, token.First.Next.ToString(), null);
                break;
            default:
                return null;
                break;
        }
    }

    public string ToPDDL(bool questionMark)
    {
        string paramsStr = "";

        if (this.type == BeliefType.Constant)
        {
            return "(" + this.name + ")";
        }
        else
        {
            foreach (Parameter p in this.param)
            {
                paramsStr = paramsStr + " " + p.ToPDDL(questionMark);
            }
            return "(" + this.name + " " + paramsStr + ")";
        }
    }

}
