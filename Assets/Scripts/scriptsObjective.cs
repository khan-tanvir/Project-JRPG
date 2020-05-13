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
    public abstract bool Evaluate();

    // Executes after the target is met
    public abstract void Complete();
}

// Gather x items to complete
[System.Serializable]
public class GatherObjective : scriptsObjective
{
    // pointer to inventory

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

    // Object to gather
    [SerializeField]
    private string _type;

[SerializeField]
    private int _currentAmount;
    [SerializeField]
    private int _requiredAmount;

    public GatherObjective(string itemToGather, int amountRequired, string information)
    {
        _type = itemToGather;
        _requiredAmount = amountRequired;
        _information = information;

        // pointer to inventory item
    }

    public string Type
    {
        get { return _type; }
    }

    public int CurrentAmount
    {
        get { return _currentAmount; }
        set { _currentAmount = value; }
    }

    public int RequiredAmount
    {
        get { return _requiredAmount; }
        set { _requiredAmount = value; }
    }

    public void UpdateCurrentAmount(string item)
    {
        if (string.Equals(item, _type, StringComparison.OrdinalIgnoreCase))
        {
            _currentAmount = scriptInventory._inventory.GetItemCount(_type);
            scriptQuestManager._questManager.UpdateDescription(this);
        }
    }

    public override bool Evaluate()
    {   
        return (_currentAmount >= _requiredAmount);
    }

    public override void Complete()
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (string.Equals(other.tag, _type))
        {
            // Handle picking up object
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

    public override bool Evaluate()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        throw new System.NotImplementedException();
    }
}

// Deliever an item from one point to another
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
    public override bool Evaluate()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        throw new System.NotImplementedException();
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

    public override bool Evaluate()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        throw new System.NotImplementedException();
    }
}

// Discover an area
[System.Serializable]
public class SearchObjective : scriptsObjective
{
    [SerializeField]
    private string _information;

    public override GoalType ObjectiveType
    {
        get { return GoalType.SEARCH; }
    }

    public override string Information
    {
        get { return _information; }
        set { _information = value; }
    }

    public override bool Evaluate()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        throw new System.NotImplementedException();
    }
}
