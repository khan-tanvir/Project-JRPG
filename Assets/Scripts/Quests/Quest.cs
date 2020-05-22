using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum QuestStatus
{
    NOTACCEPTED,
    GIVEN,
    COMPLETE
}

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

    public QuestStatus Status
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
                if (obj.Evaluate && !obj.Complete)
                {
                    i++;
                    QuestManager.Instance.UnSubscribeToEvent(obj);
                    obj.Complete =  true;
                    Debug.Log("An objective is complete");
                }

                if (i == Objectives.Count)
                {
                    Status = QuestStatus.COMPLETE;
                    return true;
                }
            }

            return false;
        }
    }
}