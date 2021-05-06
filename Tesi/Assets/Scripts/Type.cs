using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Type
{
    public List<string> subjects;
    public string typeOf;

    public Type(List<string> subjects, string typeOf)
    {
        this.subjects = subjects;
        this.typeOf = typeOf;
    }

    public static Type Evaluate(JToken token)
    {
        if (token.First.ToString() == "type")
        {
            List<string> subjects = new List<string>();

            foreach(JToken j in token.First.Next)
            {
                if(j != token.First.Next.First)
                {
                    subjects.Add(j.ToString());
                }
            }

            return new Type(subjects, token.First.Next.Next.ToString());

        } else {
            return null;
        }
    }

    public string ToPDDL()
    {
        string pddl = "";

        foreach(string s in this.subjects)
        {
            pddl = pddl + s + " ";
        }

        pddl = pddl + "- ";
        pddl = pddl + this.typeOf + "\n";

        return pddl;
    }

}
