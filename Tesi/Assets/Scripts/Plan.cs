using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plan
{
    public List<KeyValuePair<float, Action>> steps;

    public Plan(List<KeyValuePair<float, Action>> steps)
    {
        this.steps = new List<KeyValuePair<float, Action>>();
        foreach (KeyValuePair<float, Action> entry in steps)
        {
            this.steps.Add(new KeyValuePair<float, Action>(entry.Key, new Action(entry.Value)));
        }
    }

    //Parse the output of optic into the Plan object
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
                bool areEqual = true;
                if (action != a.name)
                {
                    areEqual = false;
                }
                else
                {
                    for (int j = 0; j < a.parameters.Count; j++)
                    {
                        if (j < param.Count && param[j] != a.parameters[j].name)
                        {
                            areEqual = false;
                        }
                    }
                }

                if (areEqual)
                {
                    tempSteps.Add(new KeyValuePair<float, Action>(float.Parse(arrivalTime), new Action(a)));
                }
            }
        }

        result = new Plan(tempSteps);

        return result;
    }

    //Change comma to dot
    public static string CommaToDot(KeyValuePair<float, Action> entry)
    {
        string result = string.Empty;
        foreach (Char c in entry.Key.ToString())
        {
            if(c == ',')
            {
                result += '.';
            } else
            {
                result += c;
            }
        }
        return result;
    }
}
