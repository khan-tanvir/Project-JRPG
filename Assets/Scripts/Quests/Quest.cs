using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public QuestMB QuestMB
    {
        get;
        set;
    }
    
    public string Title
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public bool QuestComplete
    {
        get;
        set;
    }

    public string QuestGiverName
    {
        get;
        set;
    }
    
    public List<Objective> Objectives
    {
        get;
        set;
    }

    public bool EvaluateObjectives
    {
        get
        {
            int i = 0;

            foreach (Objective obj in Objectives)
            {
                if (obj.Evaluate)
                {
                    i++;
                    QuestManager.Instance.UnSubscribeToEvent(obj);
                    Debug.Log("An objective is complete");
                }

                if (i == Objectives.Count)
                {
                    QuestComplete = true;
                    return true;
                }
            }

            return false;
        }
    }
}