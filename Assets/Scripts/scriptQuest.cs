using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class scriptQuest
{
    [SerializeField]
    private string _title;
    [SerializeField]
    private string _description;

    [SerializeField]
    public List<GatherObjective> _gatherObjectives;

    [SerializeField]
    public List<EscortObjective> _escortObjectives;

    [SerializeField]
    public List<DeliverObjective> _deliverObjectives;

    [SerializeField]
    public List<ActivateObjective> _activateObjectives;

    [SerializeField]
    public List<SearchObjective> _searchObjectives;

    private List<scriptsObjective> _objectives = new List<scriptsObjective>();

    public scriptQuestMB QuestMB
    {
        get;
        set;
    }

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public List<scriptsObjective> Objectives
    {
        get { return _objectives; }
        set { _objectives = value; }
    }

    public bool ObjectivesComplete
    {
        get
        {
            int i = 0;

            foreach (scriptsObjective objective in _objectives)
            {
                if (objective.Evaluate)
                {
                    i++;
                    scriptQuestManager._questManager.UnSubscribeToEvent(objective);
                    Debug.Log("An objective is complete");
                }
            }
            if (i == _objectives.Count)
                return true;
            else
                return false;
        }
    }
}
