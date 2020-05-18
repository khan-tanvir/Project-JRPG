using JetBrains.Annotations;
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
public abstract class scriptsObjective
{
    private scriptQuest _parent = null;

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

    public scriptQuest Parent
    {
        get { return _parent; }
        set { _parent = value; }
    }
}

// Gather x items to complete
[System.Serializable]
public class GatherObjective : scriptsObjective
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

    public void UpdateCurrentAmount(string item)
    {
        if (string.Equals(item, _type, StringComparison.OrdinalIgnoreCase))
        {
            _currentAmount = scriptInventory.Inventory.GetItemCount(_type);
            scriptQuestManager._questManager.UpdateDescription(this);
            scriptQuestManager._questManager.EvaluateQuest(Parent);
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
public class EscortObjective : scriptsObjective
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
public class DeliverObjective : scriptsObjective
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
public class ActivateObjective : scriptsObjective
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
public class SearchObjective : scriptsObjective
{
    [SerializeField]
    private string _information;

    private bool _objectiveFound = false;

    [SerializeField]
    private scriptOnTriggerEnter _collider;

    public override GoalType ObjectiveType
    {
        get { return GoalType.SEARCH; }
    }

    public override string Information
    {
        get { return _information; }
        set { _information = value; }
    }

    public scriptOnTriggerEnter ColliderObject
    {
        get { return _collider; }
        set { _collider = value; }
    }

    public void AssignTrigger()
    {
        _collider.SearchObjective = this;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _objectiveFound = true;
            scriptQuestManager._questManager.EvaluateQuest(Parent);
            _collider = null;
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
