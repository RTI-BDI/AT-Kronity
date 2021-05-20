using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plan
{
    public Dictionary<float, Action> step;

    public Plan(Dictionary<float, Action> step)
    {
        this.step = new Dictionary<float, Action>();
        foreach (KeyValuePair<float, Action> entry in step)
        {
            this.step.Add(entry.Key, new Action(entry.Value));
        }
    }

    public static Plan FromPlainToObject(string[] plain, List<Action> groundedActions)
    {
        Plan result = null;

        bool searchingIndex = true;
        int counter = 0;
        while (searchingIndex && counter < plain.Length)
        {
            if(plain[counter].StartsWith("; Time"))
            {
                searchingIndex = false;
            } else
            {
                counter++;
            }
        }

        List<KeyValuePair<float, Action>> tempSteps = new List<KeyValuePair<float, Action>>();
        for(int i=counter+1; i<plain.Length; i++)
        {
            string arrivalTime = string.Empty;
            string action = string.Empty;
            string tempParam = string.Empty;
            List<string> param = new List<string>();
            int considering = 0;

            foreach (char c in plain[i])
            {
                if(c == ':')
                {
                    considering = 3;
                }
                else if(c == '(')
                {
                    considering = 1;
                } else if(c == ')')
                {
                    considering = 3;
                    param.Add(tempParam);
                    tempParam = string.Empty;
                } else if(Char.IsWhiteSpace(c) && considering == 1)
                {
                    considering = 2;
                } else if(Char.IsWhiteSpace(c) && considering == 2)
                {
                    param.Add(tempParam);
                    tempParam = string.Empty;
                } else if(c == '.' && considering == 0)
                {
                    arrivalTime += ',';
                }
                else
                {
                    switch (considering)
                    {
                        //considering arrivalTime
                        case 0:
                            arrivalTime += c;
                            break;
                        //considering action
                        case 1:
                            action += c;
                            break;
                        //considering params
                        case 2:
                            tempParam += c;
                            break;
                        //skipping
                        case 3:
                            break;
                    }
                }
            }

            foreach (Action a in groundedActions)
            {
                if(action == a.name)
                {
                    for(int j=0; j<a.parameters.Count; j++)
                    {
                        if(j < param.Count && param[j] == a.parameters[j].name)
                        {
                            tempSteps.Add(new KeyValuePair<float, Action>(float.Parse(arrivalTime), new Action(a)));
                        }
                    }
                }
            }
        }

        foreach (KeyValuePair<float, Action> entry in tempSteps)
        {
            string log = "AT: " + entry.Key + " - Action: " + entry.Value.name;
            foreach (Parameter p in entry.Value.parameters)
            {
                log = log + " " + p.name;
            }
            log = log + " " + entry.Value.effects.Count;
            Debug.Log(log);
        }

        return result;
    }
}
