using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GoalType
{
    GATHER,
    ESCORT,
    DELIVER,
    ACTIVATE,
    SEARCH
}

[System.Serializable]
public abstract class Objective
{
    [System.NonSerialized]
    private Quest _parent;
    public abstract GoalType ObjectiveType
    {
        get;
    }

    public abstract string Information
    {
        get;
        internal set;
    }

    // Returns true if the objective is met
    public abstract bool Evaluate
    {
        get;
    }

    public Quest Parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    public bool Complete
    {
        get;
        set;
    }
}

// Gather x items to complete
[System.Serializable]
public class GatherObjective : Objective
{
    public override GoalType ObjectiveType
    {
        get { return GoalType.GATHER; }
    }

    public override string Information
    {
        get;
        internal set;
    }

    public string Type
    {
        get;
        internal set;
    }

    public int CurrentAmount
    {
        get;
        internal set;
    }

    public int RequiredAmount
    {
        get;
        internal set;
    }

    public GatherObjective(string desc, string type, int required)
    {
        Information = desc;
        RequiredAmount = required;
        Type = type;
    }

    public void UpdateCurrentAmount(string item)
    {
        if (string.Equals(item, Type, StringComparison.OrdinalIgnoreCase))
        {
            CurrentAmount = Inventory.Instance.GetItemCount(Type);
            QuestManager.Instance.UpdateDescription(this);
            QuestManager.Instance.EvaluateQuest(Parent);
        }
    }

    public override bool Evaluate
    {   
        get
        {
            return CurrentAmount >= RequiredAmount;
        }
    }
}

// Escort a person from one point to another
[System.Serializable]
public class EscortObjective : Objective
{
    private bool _locationEntered;
    
    public bool IsFollowing
    {
        get;
        set;
    }

    public string Location
    {
        get;
        internal set;
    }

    public override GoalType ObjectiveType
    {
        get { return GoalType.ESCORT; }
    }

    public override string Information
    {
        get;
        internal set;
    }

    public string FollowerName
    {
        get;
        internal set;
    }

    public EscortObjective(string desc, string followerName, string targetLocation)
    {
        Information = desc;
        FollowerName = followerName;
        Location = targetLocation;
    }

    public void ValidateLocation(string place)
    {
        if (!IsFollowing)
        {
            return;
        }

        if (Location == place)
        {
            _locationEntered = true;
            QuestManager.Instance.EvaluateQuest(Parent);
        }
    }

    public override bool Evaluate
    {
        get
        {
            return _locationEntered;
        }
    }
}

// Deliver an item from one point to another
[System.Serializable]
public class DeliverObjective : Objective
{

    [SerializeField]
    private Rigidbody2D _targetRB;

    public override GoalType ObjectiveType
    {
        get { return GoalType.DELIVER; }
    }

    public override string Information
    {
        get;
        internal set;
    }

    public string Item
    {
        get;
        internal set;
    }

    public string TargetName
    {
        get;
        internal set;
    }

    public Rigidbody2D TargetRB
    {
        get { return _targetRB; }
    }

    public DeliverObjective() {; }

    public DeliverObjective(string desc, string item, string target)
    {
        Information = desc;
        Item = item;
        TargetName = target;
    }

    public override bool Evaluate
    {
        get
        {
            return false;
        }
    }
}

// Interact with an item
[System.Serializable]
public class ActivateObjective : Objective
{

    private bool _hasInteracted = false;

    public override GoalType ObjectiveType
    {
        get { return GoalType.ACTIVATE; }
    }

    public override string Information
    {
        get;
        internal set;
    }

    public string ObjectToInteractWith
    {
        get;
        internal set;
    }

    public ActivateObjective() {; }

    public ActivateObjective(string desc, string objectToInteract)
    {
        Information = desc;
        ObjectToInteractWith = objectToInteract;
    }

    public void CheckInteractedItem(string item)
    {
        if (ObjectToInteractWith == item)
        {
            _hasInteracted = true;
            QuestManager.Instance.EvaluateQuest(Parent);
        }
    }

    public override bool Evaluate
    {
        get { return _hasInteracted; }
    }
}

// Discover an area
[System.Serializable]
public class SearchObjective : Objective
{
    private bool _locationEntered;

    public string Location
    {
        get;
        internal set;
    }

    public override GoalType ObjectiveType
    {
        get { return GoalType.SEARCH; }
    }

    public override string Information
    {
        get;
        internal set;
    }

    public void LocationEntered(string place)
    {
        if (Location == place)
        {
            _locationEntered = true;
            QuestManager.Instance.EvaluateQuest(Parent);
        }    
    }

    public SearchObjective() {; }

    public SearchObjective(string desc, string location)
    {
        Information = desc;
        Location = location;
    }

    public override bool Evaluate
    {
        get
        {
            return _locationEntered;
        }
    }
}
