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
        set;
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
}

// Gather x items to complete
[System.Serializable]
public class GatherObjective : Objective
{

    [SerializeField]
    private string _information;

    public override GoalType ObjectiveType
    {
        get { return GoalType.GATHER; }
    }

    public override string Information
    {
        get { return _information; }
        set { _information = value; }
    }

    [SerializeField]
    private string _type;

    [SerializeField]
    private int _currentAmount;
    [SerializeField]
    private int _requiredAmount;

    public string Type
    {
        get { return _type; }
    }

    public int CurrentAmount
    {
        get { return _currentAmount; }
    }

    public int RequiredAmount
    {
        get { return _requiredAmount; }
    }

    public GatherObjective() {; }

    public GatherObjective(string desc, string type, int required)
    {
        this._information = desc;
        this._requiredAmount = required;
        this._type = type;
    }

    public void UpdateCurrentAmount(string item)
    {
        if (string.Equals(item, _type, StringComparison.OrdinalIgnoreCase))
        {
            _currentAmount = Inventory.Instance.GetItemCount(_type);
            QuestManager.Instance.UpdateDescription(this);
            QuestManager.Instance.EvaluateQuest(Parent);
        }
    }

    public override bool Evaluate
    {   
        get
        {
            if (_currentAmount >= _requiredAmount)
                return true;
            else 
                return false;
        }
    }
}

// Escort a person from one point to another
[System.Serializable]
public class EscortObjective : Objective
{
    [SerializeField]
    private string _followerName;
    [SerializeField]
    private Rigidbody2D _follower;

    [SerializeField]
    private string _targetName;
    [SerializeField]
    private Rigidbody2D _target;

    [SerializeField]
    private string _information;

    public override GoalType ObjectiveType
    {
        get { return GoalType.ESCORT; }
    }

    public override string Information
    {
        get { return _information; }
        set { _information = value; }
    }

    public Rigidbody2D FollowerRB
    {
        get { return _follower; }
    }

    public Rigidbody2D TargetRB
    {
        get { return _target; }
    }

    public string FollowerName
    {
        get { return _followerName; }
    }

    public string TargetName
    {
        get { return _targetName; }
    }

    public override bool Evaluate
    {
        get
        {
            return false;
        }
    }
}

// Deliver an item from one point to another
[System.Serializable]
public class DeliverObjective : Objective
{
    [SerializeField]
    private string _item;

    [SerializeField]
    private string _targetName;

    [SerializeField]
    private Rigidbody2D _targetRB;

    private string _information;

    public override GoalType ObjectiveType
    {
        get { return GoalType.DELIVER; }
    }

    public override string Information
    {
        get { return _information; }
        set { _information = value; }
    }

    public string Item
    { 
        get { return _item; } 
        set { _item = value; }
    }

    public string TargetName
    {
        get { return _targetName; }
    }

    public Rigidbody2D TargetRB
    {
        get { return _targetRB; }
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
    [SerializeField]
    private string _information;

    public override GoalType ObjectiveType
    {
        get { return GoalType.ACTIVATE; }
    }

    public override string Information
    {
        get { return _information; }
        set { _information = value; }
    }

    public override bool Evaluate
    {
        get
        {
            return false;
        }
    }
}

// Discover an area
[System.Serializable]
public class SearchObjective : Objective
{
    [SerializeField]
    private string _information;

    private bool _objectiveFound = false;

    public string Location
    {
        get;
        set;
    }

    public override GoalType ObjectiveType
    {
        get { return GoalType.SEARCH; }
    }

    public override string Information
    {
        get { return _information; }
        set { _information = value; }
    }

    public void LocationEntered(string place)
    {
        if (_objectiveFound != true)
        {
            if (place == Location)
            {
                _objectiveFound = true;
                QuestManager.Instance.EvaluateQuest(Parent);
            }
            else
                return;
        }
    }
    public override bool Evaluate
    {
        get
        {
            return _objectiveFound;
        }
    }
}
