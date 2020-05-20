using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scriptQuestManager : MonoBehaviour
{
    public static scriptQuestManager _questManager;

    [SerializeField]
    private GameObject _journalMenu;

    [SerializeField]
    private Transform _listTransform;

    private scriptQuest currentSelectedQuest;

    private List<scriptQuest> _quests = new List<scriptQuest>();

    [SerializeField]
    private TMP_Text _numberOfQuests;

    [SerializeField]
    private TMP_Text _descriptionPanel;

    [SerializeField]
    private GameObject _questPrefab;

    [SerializeField]
    private TextAsset _jsonFile;

    private void Awake()
    {
        if (_questManager == null)
        {
            DontDestroyOnLoad(gameObject);
            _questManager = this;
        }
        else if (_questManager != null)
            Destroy(gameObject);

        QuestsDatabase questsDatabase = new QuestsDatabase();
        questsDatabase.FileToRead = _jsonFile;
        questsDatabase.ReadDatabase();
    }

    public void AddQuestToJournal(scriptQuest quest)
    {
        GameObject questObject = Instantiate(_questPrefab, _listTransform);

        // Both questscript and non mb version need to have reference of each other
        scriptQuestMB temp = questObject.GetComponent<scriptQuestMB>();
        quest.QuestMB = temp;
        temp.Quest = quest;

        questObject.GetComponent<TMP_Text>().text = quest.Title;

        foreach(scriptsObjective objective in quest.Objectives)
        {
            objective.Parent = quest;
            SubscribeToEvent(objective);
        }

        _quests.Add(quest);
        UpdateQuestsCapacity();

        Debug.Log("Added quest to journal");
    }

    public void ShowDescription(scriptQuest quest)
    {
        if (quest == null)
            return;

        if (currentSelectedQuest != null && currentSelectedQuest != quest)
        {
            currentSelectedQuest.QuestMB.OnDeselect();
        }

        currentSelectedQuest = quest;
        string objectives = "\n";

        foreach (scriptsObjective objective in currentSelectedQuest.Objectives)
        {
            objectives += objective.Information + "\n";
        }

        string textToDisplay = string.Format("{0}\n\n<size=25>{1}</size><size=20>{2}</size>", currentSelectedQuest.Title, currentSelectedQuest.Description, objectives);

        _descriptionPanel.text = textToDisplay;
    }

    private void UpdateQuestsCapacity()
    {
        _numberOfQuests.text = _quests.Count.ToString() + "/-";
    }

    private void SubscribeToEvent(scriptsObjective objective)
    {
        switch (objective.ObjectiveType)
        {
            case GoalType.GATHER:
                var gatherCast = (GatherObjective)objective;
                scriptGameEvents._gameEvents.onGatherObjectiveChange += gatherCast.UpdateCurrentAmount;
                scriptGameEvents._gameEvents.GatherObjectiveChange(gatherCast.Type);
                break;
            case GoalType.ESCORT:
                
                break;
            case GoalType.DELIVER:
                
                break;
            case GoalType.ACTIVATE:
                
                break;
            case GoalType.SEARCH:
                var searchCast = (SearchObjective)objective;
                scriptGameEvents._gameEvents.onLocationEntered += searchCast.LocationEntered; 
                break;
            default:
                break;
        }
    }
    public void UnSubscribeToEvent(scriptsObjective objective)
    {
        switch (objective.ObjectiveType)
        {
            case GoalType.GATHER:
                var gatherCast = (GatherObjective)objective;
                scriptGameEvents._gameEvents.onGatherObjectiveChange -= gatherCast.UpdateCurrentAmount;
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

    public void UpdateDescription(scriptsObjective objective)
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

        ShowDescription(currentSelectedQuest);
    }

    public void EvaluateQuest(scriptQuest quest)
    {   
        if (quest.ObjectivesComplete)
        {
            Debug.Log("Quest: " + quest.Title + " is Complete");

            foreach (scriptsObjective objective in quest.Objectives)
                UnSubscribeToEvent(objective);
        }
    }
}
