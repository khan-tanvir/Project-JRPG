using System;

public enum GoalType
{
    GATHER,

    ESCORT,

    DELIVER,

    ACTIVATE,

    SEARCH
}

// Interact with an item
[System.Serializable]
public class ActivateObjective : Objective
{
    #region Private Fields

    private bool _hasInteracted = false;

    #endregion Private Fields

    #region Public Constructors

    public ActivateObjective()
    {
        ;
    }

    public ActivateObjective(string desc, string objectToInteract)
    {
        Information = desc;
        ObjectToInteractWith = objectToInteract;
    }

    #endregion Public Constructors

    #region Public Properties

    public override bool Evaluate
    {
        get { return _hasInteracted; }
    }

    public override string Information
    {
        get;
        internal set;
    }

    public override GoalType ObjectiveType
    {
        get { return GoalType.ACTIVATE; }
    }

    public string ObjectToInteractWith
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Public Methods

    public void CheckInteractedItem(string item)
    {
        if (ObjectToInteractWith == item)
        {
            _hasInteracted = true;
            QuestManager.Instance.EvaluateQuest(Parent);
        }
    }

    #endregion Public Methods
}

// Deliver an item from one point to another
[System.Serializable]
public class DeliverObjective : Objective
{
    #region Private Fields

    private bool _itemDelivered;

    #endregion Private Fields

    #region Public Constructors

    public DeliverObjective(string desc, string item, string target)
    {
        Information = desc;
        Item = item;
        TargetName = target;
    }

    #endregion Public Constructors

    #region Public Properties

    public override bool Evaluate
    {
        get
        {
            return _itemDelivered;
        }
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

    public override GoalType ObjectiveType
    {
        get { return GoalType.DELIVER; }
    }

    public string TargetName
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Public Methods

    public void InteractedWithWorld(string objectName)
    {
        if (TargetName == objectName)
        {
            _itemDelivered = true;
            Inventory.Instance.RemoveItem(Item);
            QuestManager.Instance.EvaluateQuest(Parent);
        }
    }

    #endregion Public Methods
}

// Escort a person from one point to another
[System.Serializable]
public class EscortObjective : Objective
{
    #region Private Fields

    private bool _locationEntered;

    #endregion Private Fields

    #region Public Constructors

    public EscortObjective(string desc, string followerName, string targetLocation)
    {
        Information = desc;
        FollowerName = followerName;
        Location = targetLocation;
    }

    #endregion Public Constructors

    #region Public Properties

    public override bool Evaluate
    {
        get
        {
            return _locationEntered;
        }
    }

    public string FollowerName
    {
        get;
        internal set;
    }

    public override string Information
    {
        get;
        internal set;
    }

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

    #endregion Public Properties

    #region Public Methods

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

    #endregion Public Methods
}

// Gather x items to complete
[System.Serializable]
public class GatherObjective : Objective
{
    #region Public Constructors

    public GatherObjective(string desc, string type, int required)
    {
        Information = desc;
        RequiredAmount = required;
        Type = type;
    }

    #endregion Public Constructors

    #region Public Properties

    public int CurrentAmount
    {
        get;
        internal set;
    }

    public override bool Evaluate
    {
        get
        {
            return CurrentAmount >= RequiredAmount;
        }
    }

    public override string Information
    {
        get;
        internal set;
    }

    public override GoalType ObjectiveType
    {
        get { return GoalType.GATHER; }
    }

    public int RequiredAmount
    {
        get;
        internal set;
    }

    public string Type
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Public Methods

    public void UpdateCurrentAmount(string item)
    {
        if (string.Equals(item, Type, StringComparison.OrdinalIgnoreCase))
        {
            CurrentAmount = Inventory.Instance.GetItemCount(Type);
            QuestManager.Instance.UpdateDescription(this);
            QuestManager.Instance.EvaluateQuest(Parent);
        }
    }

    #endregion Public Methods
}

[System.Serializable]
public abstract class Objective
{
    #region Private Fields

    [System.NonSerialized]
    private Quest _parent;

    #endregion Private Fields

    #region Public Properties

    public bool Complete
    {
        get;
        set;
    }

    // Returns true if the objective is met
    public abstract bool Evaluate
    {
        get;
    }

    public abstract string Information
    {
        get;
        internal set;
    }

    public abstract GoalType ObjectiveType
    {
        get;
    }

    public Quest Parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    #endregion Public Properties
}

// Discover an area
[System.Serializable]
public class SearchObjective : Objective
{
    #region Private Fields

    private bool _locationEntered;

    #endregion Private Fields

    #region Public Constructors

    public SearchObjective(string desc, string location)
    {
        Information = desc;
        Location = location;
    }

    #endregion Public Constructors

    #region Public Properties

    public override bool Evaluate
    {
        get
        {
            return _locationEntered;
        }
    }

    public override string Information
    {
        get;
        internal set;
    }

    public string Location
    {
        get;
        internal set;
    }

    public override GoalType ObjectiveType
    {
        get { return GoalType.SEARCH; }
    }

    #endregion Public Properties

    #region Public Methods

    public void LocationEntered(string place)
    {
        if (Location == place)
        {
            _locationEntered = true;
            QuestManager.Instance.EvaluateQuest(Parent);
        }
    }

    #endregion Public Methods
}