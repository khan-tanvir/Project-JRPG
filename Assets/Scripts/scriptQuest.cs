using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class scriptQuest
{
    [SerializeField]
    private string _title;
    [SerializeField]
    private string _description;

    [SerializeField]
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
}
