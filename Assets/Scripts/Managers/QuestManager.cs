using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _journalMenu;

    [SerializeField]
    private Transform _listTransform;

    [SerializeField]
    private TMP_Text _numberOfQuests;

    [SerializeField]
    private TMP_Text _descriptionPanel;

    [SerializeField]
    private GameObject _questPrefab;

    [SerializeField]
    private TextAsset _jsonFile;

    private List<Quest> _quests;

    public static QuestManager Instance
    {
        get;
        internal set;
    }

    private Quest _currentSelectedQuest;

    private void Awake()
    {
        Instance = this;

        _quests = new List<Quest>();
    }

    private void OnEnable()
    {
        QuestsDatabase questsDatabase = new QuestsDatabase();
        questsDatabase.FileToRead = _jsonFile;
        questsDatabase.ReadDatabase();
    }

    public void AddQuestToJournal(Quest quest)
    {
        GameObject questObject = Instantiate(_questPrefab, _listTransform);

        // Both questscript and non mb version need to have reference of each other
        QuestMB temp = questObject.GetComponent<QuestMB>();
        quest.QuestMB = temp;
        temp.Quest = quest;

        questObject.GetComponent<TMP_Text>().text = quest.Title;

        foreach(Objective objective in quest.Objectives)
        {
            SubscribeToEvent(objective);
        }

        _quests.Add(quest);
        UpdateQuestsCapacity();

        Debug.Log("Added quest to journal");
    }

    public void GiveQuestToQuestGiver(Quest quest)
    {
        foreach (QuestGiver questGiver in FindObjectsOfType<QuestGiver>())
        {
            if (questGiver.NPCName == quest.QuestGiverName)
                questGiver.CreateQuest(quest);
        }
    }

    public void ShowDescription(Quest quest)
    {
        if (quest == null)
            return;

        if (_currentSelectedQuest != null && _currentSelectedQuest != quest)
        {
            _currentSelectedQuest.QuestMB.OnDeselect();
        }

        _currentSelectedQuest = quest;
        string objectives = "\n";

        foreach (Objective objective in _currentSelectedQuest.Objectives)
        {
            objectives += objective.Information + "\n";
        }

        string textToDisplay = string.Format("{0}\n\n<size=25>{1}</size><size=20>{2}</size>", _currentSelectedQuest.Title, _currentSelectedQuest.Description, objectives);

        _descriptionPanel.text = textToDisplay;
    }

    private void UpdateQuestsCapacity()
    {
        _numberOfQuests.text = _quests.Count.ToString() + "/-";
    }

    private void SubscribeToEvent(Objective objective)
    {
        switch (objective.ObjectiveType)
        {
            case GoalType.GATHER:
                var gatherCast = (GatherObjective)objective;
                EventsManager.Instance.onGatherObjectiveChange += gatherCast.UpdateCurrentAmount;
                EventsManager.Instance.GatherObjectiveChange(gatherCast.Type);
                break;
            case GoalType.ESCORT:
                
                break;
            case GoalType.DELIVER:
                
                break;
            case GoalType.ACTIVATE:
                
                break;
            case GoalType.SEARCH:
                var searchCast = (SearchObjective)objective;
                EventsManager.Instance.onLocationEntered += searchCast.LocationEntered; 
                break;
            default:
                break;
        }
    }
    public void UnSubscribeToEvent(Objective objective)
    {
        switch (objective.ObjectiveType)
        {
            case GoalType.GATHER:
                var gatherCast = (GatherObjective)objective;
                EventsManager.Instance.onGatherObjectiveChange -= gatherCast.UpdateCurrentAmount;
                break;
            case GoalType.ESCORT:

                break;
            case GoalType.DELIVER:

                break;
            case GoalType.ACTIVATE:

                break;
            case GoalType.SEARCH:

                break;
            default:
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
                escortCast.Information = "Escort [" + escortCast.FollowerName + "] to [" + escortCast.TargetName + "]";
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

    public void EvaluateQuest(Quest quest)
    {   
        if (quest.EvaluateObjectives)
        {
            Debug.Log("Quest: " + quest.Title + " is Complete");
        }
    }
}
