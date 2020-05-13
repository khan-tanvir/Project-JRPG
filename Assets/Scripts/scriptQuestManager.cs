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

    [SerializeField]
    private TMP_Text _descriptionPanel;

    private void Awake()
    {
        if (_questManager == null)
        {
            DontDestroyOnLoad(gameObject);
            _questManager = this;
        }
        else if (_questManager != null)
            Destroy(gameObject);
    }

    public void AddQuestToJournal(GameObject questObjectPrefab, scriptQuest quest)
    {
        GameObject questObject = Instantiate(questObjectPrefab, _listTransform);

        // Both questscript and non mb version need to have reference of each other
        scriptQuestMB temp = questObject.GetComponent<scriptQuestMB>();
        quest.QuestMB = temp;
        temp.Quest = quest;

        questObject.GetComponent<TMP_Text>().text = quest.Title;

        SubscribeToEvent(quest);
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

        for (int i = 0; i < currentSelectedQuest.Objectives.Count; i++)
        {
            if (currentSelectedQuest.Objectives[i] != null)
            {
                objectives += currentSelectedQuest.Objectives[i].Information + "\n";
            }
        }

        string textToDisplay = string.Format("{0}\n\n<size=25>{1}</size><size=20>{2}</size>", currentSelectedQuest.Title, currentSelectedQuest.Description, objectives);

        _descriptionPanel.text = textToDisplay;
    }

    public void SubscribeToEvent(scriptQuest quest)
    {
        foreach (GatherObjective gather in quest.Objectives)
        {
            scriptGameEvents._gameEvents.onGatherObjectiveChange += gather.UpdateCurrentAmount;
            scriptGameEvents._gameEvents.GatherObjectiveChange(gather.Type);
            UpdateDescription(gather);
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
            // The following two types should have a custom description
            case GoalType.ACTIVATE:
                Debug.LogError("You haven't set a description for " + objective);
                break;
            case GoalType.SEARCH:
                Debug.LogError("You haven't set a description for " + objective);
                break;
            default:
                Debug.LogError("Objective doesn't have a type.");
                break;
        }

        ShowDescription(currentSelectedQuest);
    }
}
