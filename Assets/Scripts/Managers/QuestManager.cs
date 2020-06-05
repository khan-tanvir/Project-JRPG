using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Private Fields

    //DELETE THIS
    [SerializeField]
    private GameObject _ARROW;

    private Quest _currentSelectedQuest;

    private TMP_Text _descriptionPanel;

    private Transform _listTransform;

    private TMP_Text _numberOfQuests;

    [SerializeField]
    private GameObject _questPrefab;

    #endregion Private Fields

    #region Public Properties

    public static QuestManager Instance
    {
        get;
        internal set;
    }

    public List<Quest> Quests
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Private Methods

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void LoadQuestDatabase()
    {
        QuestsDatabase database = new QuestsDatabase();
        database.ReadDatabase(GameData.Instance.CurrentSaveFile);
    }

    private void LoadVariables()
    {
        Transform journal = FindObjectOfType<Canvas>().transform.Find("Journal");

        _descriptionPanel = journal.Find("Selected").Find("Text Box").Find("Text").GetComponent<TMP_Text>();
        _listTransform = journal.Find("List").Find("Quest Area");
        _numberOfQuests = journal.Find("Capacity").Find("Text").GetComponent<TMP_Text>();

        Quests = new List<Quest>();
    }

    private void OnEscortObjectiveComplete(EscortObjective objective)
    {
        AIController temp = GameObject.Find(objective.FollowerName).GetComponent<AIController>();

        EventsManager.Instance.OnToggleFollower -= temp.ToggleFollower;

        temp.ResetSpeed();

        temp.State = State.IDLE;
        temp.Target = null;

        temp.EscortObjective = null;
    }

    private void SetupDeliverObjective(DeliverObjective objective)
    {
        int index = Inventory.Instance.Slots.FindIndex(a => a.ObjectID == "Quest Item: " + objective.Item);

        if (index != -1)
        {
            Inventory.Instance.Slots[index].GetComponentInChildren<IDroppable>().EnableDrop = false;
        }
        else
        {
            GameObject temp = Instantiate(_ARROW);

            if (temp.GetComponent<IDroppable>() != null)
            {
                temp.GetComponent<IDroppable>().EnableDrop = false;
            }

            if (Inventory.Instance.FillSlot(temp, objective.Item))
            {
                Destroy(temp);
            }
        }
    }

    private void SetupEscortObjective(EscortObjective objective)
    {
        AIController temp = GameObject.Find("NPCs").transform.Find(objective.FollowerName).GetComponent<AIController>();

        if (temp == null)
        {
            Debug.Log("No AIController component could be found with the given name, " + objective.FollowerName);
        }

        temp.EscortObjective = objective;

        temp.State = State.IDLE;

        temp.Target = FindObjectOfType<Player>().transform;

        EventsManager.Instance.OnToggleFollower += temp.ToggleFollower;
    }

    private void Start()
    {
        LoadVariables();
        LoadQuestDatabase();
    }

    private void SubscribeToEvent(Objective objective)
    {
        switch (objective.ObjectiveType)
        {
            case GoalType.GATHER:
                var gatherCast = (GatherObjective)objective;
                EventsManager.Instance.OnGatherObjectiveChange += gatherCast.UpdateCurrentAmount;
                EventsManager.Instance.GatherObjectiveChange(gatherCast.Type);
                break;

            case GoalType.ESCORT:
                var escortCast = (EscortObjective)objective;
                EventsManager.Instance.OnLocationEntered += escortCast.ValidateLocation;

                SetupEscortObjective(escortCast);
                break;

            case GoalType.DELIVER:
                var deliverCast = (DeliverObjective)objective;
                EventsManager.Instance.OnInteractionWithItem += deliverCast.InteractedWithWorld;

                SetupDeliverObjective(deliverCast);
                break;

            case GoalType.ACTIVATE:
                var activateCast = (ActivateObjective)objective;
                EventsManager.Instance.OnInteractionWithItem += activateCast.CheckInteractedItem;
                break;

            case GoalType.SEARCH:
                var searchCast = (SearchObjective)objective;
                EventsManager.Instance.OnLocationEntered += searchCast.LocationEntered;
                break;
        }
    }

    private void UpdateQuestsCapacity()
    {
        int i = 0;

        foreach (Quest Quest in Quests)
        {
            if (Quest.Status == QuestStatus.GIVEN)
            {
                i++;
            }
        }

        _numberOfQuests.text = i.ToString() + "/-";
    }

    #endregion Private Methods

    #region Public Methods

    public void AddQuestToJournal(Quest quest)
    {
        GameObject questObject = Instantiate(_questPrefab, _listTransform);

        // Both questscript and non mb version need to have reference of each other
        QuestMB temp = questObject.GetComponent<QuestMB>();
        quest.QuestMB = temp;
        temp.Quest = quest;

        quest.QuestMB.UpdateColor();

        questObject.GetComponent<TMP_Text>().text = quest.Title;

        if (quest.Status == QuestStatus.GIVEN)
        {
            foreach (Objective objective in quest.Objectives)
            {
                if (!objective.Complete)
                {
                    UpdateDescription(objective);
                    SubscribeToEvent(objective);
                }
            }
        }

        if (!Quests.Contains(quest))
        {
            Quests.Add(quest);
        }

        UpdateQuestsCapacity();
    }

    public void ClearCurrentQuest()
    {
        if (_currentSelectedQuest != null)
        {
            _currentSelectedQuest.QuestMB.OnDeselect();
            _currentSelectedQuest.QuestMB.UpdateColor();

            _descriptionPanel.text = "";
        }
    }

    public void EvaluateQuest(Quest quest)
    {
        if (quest.EvaluateObjectives)
        {
            Debug.Log("Quest: " + quest.Title + " is Complete");
            quest.QuestMB.UpdateColor();
        }
    }

    public void GiveQuestToQuestGiver(Quest quest)
    {
        foreach (QuestGiver questGiver in FindObjectsOfType<QuestGiver>())
        {
            if (questGiver.NPCName == quest.QuestGiverName)
            {
                questGiver.CreateQuest(quest);
            }
            if (!Quests.Contains(quest))
            {
                Quests.Add(quest);
            }
        }
    }

    public void ShowDescription(Quest quest)
    {
        if (quest == null)
            return;

        if (_currentSelectedQuest != quest)
        {
            ClearCurrentQuest();
        }

        _currentSelectedQuest = quest;

        string objectives = "\n";

        foreach (Objective objective in _currentSelectedQuest.Objectives)
        {
            if (objective.Complete)
            {
                objectives += "<color=green>" + objective.Information + "</color> \n";
            }
            else
            {
                objectives += objective.Information + "\n";
            }
        }

        _descriptionPanel.text = string.Format("{0}\n\n<size=25>{1}</size><size=20>{2}</size>", _currentSelectedQuest.Title, _currentSelectedQuest.Description, objectives);
    }

    public void UnSubscribeToEvent(Objective objective)
    {
        switch (objective.ObjectiveType)
        {
            case GoalType.GATHER:
                var gatherCast = (GatherObjective)objective;
                EventsManager.Instance.OnGatherObjectiveChange -= gatherCast.UpdateCurrentAmount;
                break;

            case GoalType.ESCORT:
                var escortCast = (EscortObjective)objective;
                EventsManager.Instance.OnLocationEntered -= escortCast.ValidateLocation;

                OnEscortObjectiveComplete(escortCast);
                break;

            case GoalType.DELIVER:

                break;

            case GoalType.ACTIVATE:
                var activateCast = (ActivateObjective)objective;
                EventsManager.Instance.OnInteractionWithItem -= activateCast.CheckInteractedItem;
                break;

            case GoalType.SEARCH:
                var searchCast = (SearchObjective)objective;
                EventsManager.Instance.OnLocationEntered -= searchCast.LocationEntered;
                break;
        }
    }

    public void UpdateDescription(Objective objective)
    {
        // If a description is not set in the inspector
        switch (objective.ObjectiveType)
        {
            // Cast the objective as it's type and then work from that stored variable
            case GoalType.GATHER:
                var gatherCast = (GatherObjective)objective;
                gatherCast.Information = "Gather [" + gatherCast.Type + "]  " + gatherCast.CurrentAmount.ToString() + "/" + gatherCast.RequiredAmount.ToString();
                break;

            case GoalType.ESCORT:
                var escortCast = (EscortObjective)objective;
                escortCast.Information = "Escort [" + escortCast.FollowerName + "] to [" + escortCast.Location + "]";
                break;

            case GoalType.DELIVER:
                var deliverCast = (DeliverObjective)objective;
                deliverCast.Information = "Take [" + deliverCast.Item + "] to [" + deliverCast.TargetName + "]";
                break;

            default:
                if (objective.Information == null)
                    Debug.LogError("You haven't set a description for " + objective);
                break;
        }

        ShowDescription(_currentSelectedQuest);
    }

    #endregion Public Methods
}