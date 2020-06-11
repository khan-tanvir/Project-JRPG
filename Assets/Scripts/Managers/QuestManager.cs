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

    [SerializeField]
    private TMP_Text _descriptionPanel;

    private Transform _listTransform;

    private string _npcID;

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
        Quests = new List<Quest>();

        QuestsDatabase database = new QuestsDatabase();
        database.ReadDatabase(GameData.Instance.CurrentSaveFile);
    }

    private void LoadVariables()
    {
        Transform journal = FindObjectOfType<Canvas>().transform.Find("Journal");

        _descriptionPanel = journal.Find("Selected").Find("Text Box").Find("Text").GetComponent<TMP_Text>();
        _listTransform = journal.Find("List").Find("Quest Area");
        _numberOfQuests = journal.Find("Capacity").Find("Text").GetComponent<TMP_Text>();
    }

    private void OnDisable()
    {
        EventsManager.Instance.OnSceneChange -= LoadVariables;
        EventsManager.Instance.OnSceneChange -= HandleQuests;
    }

    private void OnEscortObjectiveComplete(EscortObjective objective)
    {
        AIController npc = null;

        foreach (AIController ai in FindObjectsOfType<AIController>())
        {
            if (ai.GetComponent<IDGenerator>().ObjectID == _npcID && ai.gameObject.scene.buildIndex == -1)
            {
                npc = ai;
            }
        }

        if (npc == null)
        {
            return;
        }

        EventsManager.Instance.OnToggleFollower -= npc.ToggleFollower;

        npc.ResetSpeed();

        npc.State = State.IDLE;
        npc.Target = null;

        npc.EscortObjective = null;

        _npcID = "";

        npc.GetComponent<IDGenerator>().IsDestroyed = true;

        SceneManagerScript.Instance.AddToSceneObjectList(npc.GetComponent<IDGenerator>());

        Destroy(npc.gameObject);
    }

    private List<Objective> ReadObjectives(QuestEntry quest, Quest parent)
    {
        List<Objective> loadedObjectives = new List<Objective>();

        foreach (ObjectivesEntry objective in quest.ObjectivesEntry)
        {
            Objective temp = null;
            switch (objective.Type)
            {
                case "GATHER":

                    // first entry is item to gather followed by, required amount and current amount
                    temp = new GatherObjective(objective.Description, objective.FirstEntry, int.Parse(objective.SecondEntry));
                    break;

                case "ESCORT":

                    // first entry is npc to escort, second is target location
                    // TODO: make third entry a bool whether to escort target
                    temp = new EscortObjective(objective.Description, objective.FirstEntry, objective.SecondEntry);
                    break;

                case "SEARCH":

                    // First entry is place to find
                    temp = new SearchObjective(objective.Description, objective.FirstEntry);
                    break;

                case "ACTIVATE":

                    // First entry is item / npc to interact with
                    // TODO: Make second entry a bool whether first entry is an npc
                    temp = new ActivateObjective(objective.Description, objective.FirstEntry);
                    break;

                case "DELIVER":
                    temp = new DeliverObjective(objective.Description, objective.FirstEntry, objective.SecondEntry);
                    break;
            }

            if (temp != null)
            {
                temp.Complete = objective.Complete;

                if (!temp.Complete)
                {
                    temp.Parent = parent;
                }

                loadedObjectives.Add(temp);
            }
        }

        return loadedObjectives;
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
        AIController npc = null;

        if (!string.IsNullOrEmpty(_npcID))
        {
            foreach (AIController ai in FindObjectsOfType<AIController>())
            {
                if (ai.GetComponent<IDGenerator>().ObjectID == _npcID)
                {
                    if (ai.gameObject.scene.buildIndex == -1)
                    {
                        npc = ai;
                    }
                    else
                    {
                        Destroy(ai.gameObject);
                    }
                }
            }
        }
        else
        {
            foreach (AIController ai in FindObjectsOfType<AIController>())
            {
                if (ai.NPCName == objective.FollowerName)
                {
                    npc = ai;
                }
            }
        }

        if (npc == null)
        {
            return;
        }

        npc.EscortObjective = objective;

        npc.State = State.IDLE;

        npc.Target = FindObjectOfType<Player>().transform;
        npc.GetComponent<Rigidbody2D>().position = RespawnManager.Instance.CurrentCheckpoint;

        if (string.IsNullOrEmpty(_npcID))
        {
            EventsManager.Instance.OnToggleFollower += npc.ToggleFollower;
            _npcID = npc.GetComponent<IDGenerator>().ObjectID;
        }

        npc.transform.parent = null;
        DontDestroyOnLoad(npc.gameObject);
    }

    private void Start()
    {
        LoadVariables();
        LoadQuestDatabase();
        HandleQuests();

        EventsManager.Instance.OnSceneChange += LoadVariables;
        EventsManager.Instance.OnSceneChange += HandleQuests;
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
        int index = Quests.FindIndex(a => a.Title == quest.Title);

        if (index == -1)
        {
            return;
        }

        Quests[index].Status = quest.Status;

        GameObject questObject = Instantiate(_questPrefab, _listTransform);

        // Both questscript and non mb version need to have reference of each other
        QuestMB temp = questObject.GetComponent<QuestMB>();
        Quests[index].QuestMB = temp;
        temp.Quest = Quests[index];

        Quests[index].QuestMB.UpdateColor();

        questObject.GetComponent<TMP_Text>().text = quest.Title;

        if (Quests[index].Status == QuestStatus.GIVEN)
        {
            foreach (Objective objective in Quests[index].Objectives)
            {
                if (!objective.Complete)
                {
                    UpdateDescription(objective);
                    SubscribeToEvent(objective);
                }
            }
        }

        UpdateQuestsCapacity();
    }

    public void AddToQuestsList(QuestList questList)
    {
        List<Quest> quests = new List<Quest>();

        foreach (QuestEntry quest in questList.QuestEntry)
        {
            Quest loadedQuest = new Quest();

            loadedQuest.Title = quest.Title;
            loadedQuest.Description = quest.Description;
            loadedQuest.QuestGiverName = quest.Giver;

            loadedQuest.Status = (QuestStatus)quest.Status;

            if (quest.ObjectivesEntry.Count == 0)
            {
                Debug.LogError("Quest: " + loadedQuest.Title + " has no objectives.");
            }

            loadedQuest.Objectives = ReadObjectives(quest, loadedQuest);

            quests.Add(loadedQuest);
        }

        Quests = quests;
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
                if (questGiver.Quests.Contains(quest))
                {
                    return;
                }

                questGiver.CreateQuest(quest);
            }
        }
    }

    public void HandleQuests()
    {
        foreach (Quest quest in Quests)
        {
            if (quest.Status == QuestStatus.NOTACCEPTED)
            {
                GiveQuestToQuestGiver(quest);
            }
            else // QuestStatus == Complete || QuestStatus = given
            {
                AddQuestToJournal(quest);
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