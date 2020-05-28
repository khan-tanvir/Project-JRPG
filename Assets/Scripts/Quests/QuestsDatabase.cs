using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct ObjectivesEntry
{
    #region Public Fields

    public bool Complete;

    public string Description;

    public string FirstEntry;

    public string SecondEntry;

    public string ThirdEntry;

    public string Type;

    #endregion Public Fields
}

[System.Serializable]
public struct QuestEntry
{
    #region Public Fields

    public string Description;

    public string Giver;

    public int ID;

    public List<ObjectivesEntry> ObjectivesEntry;

    // JSON doesn't support enum data types
    public int Status;

    public string Title;

    #endregion Public Fields
}

public class QuestList
{
    #region Public Fields

    public List<QuestEntry> QuestEntry = new List<QuestEntry>();

    #endregion Public Fields
}

public class QuestsDatabase
{
    //public QuestList _questList = new QuestList();

    #region Private Methods

    private void CloneDatabase(int saveFileID)
    {
        // Clone original database as fileName
        var clonedDatabase = Resources.Load<TextAsset>("Databases/ExampleQuestDatabase");

        if (clonedDatabase != null)
        {
            File.WriteAllText(Application.persistentDataPath + "/playerQuestDB" + saveFileID + ".json", clonedDatabase.text);
        }
    }

    private string GetPlayerQuestDatabase(int saveFileID)
    {
        if (!File.Exists(Application.persistentDataPath + "/playerQuestDB" + saveFileID + ".json"))
        {
            CloneDatabase(saveFileID);
        }

        return File.ReadAllText(Application.persistentDataPath + "/playerQuestDB" + saveFileID + ".json");
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

    private List<ObjectivesEntry> WriteObjectives(Quest quest)
    {
        List<ObjectivesEntry> objectivesEntries = new List<ObjectivesEntry>();

        foreach (Objective objective in quest.Objectives)
        {
            ObjectivesEntry entry = new ObjectivesEntry();

            entry.Complete = objective.Complete;
            entry.Description = objective.Information;

            switch (objective.ObjectiveType)
            {
                case GoalType.GATHER:
                    var gatherCast = (GatherObjective)objective;
                    entry.FirstEntry = gatherCast.Type;
                    entry.SecondEntry = gatherCast.RequiredAmount.ToString();
                    entry.ThirdEntry = gatherCast.CurrentAmount.ToString();
                    break;

                case GoalType.ESCORT:
                    var escortCast = (EscortObjective)objective;
                    entry.FirstEntry = escortCast.FollowerName;
                    entry.SecondEntry = escortCast.Location;
                    break;

                case GoalType.DELIVER:
                    var deliverCast = (DeliverObjective)objective;
                    entry.FirstEntry = deliverCast.Item;
                    entry.SecondEntry = deliverCast.TargetName;
                    break;

                case GoalType.ACTIVATE:
                    var activateCast = (ActivateObjective)objective;
                    entry.FirstEntry = activateCast.ObjectToInteractWith;
                    break;

                case GoalType.SEARCH:
                    var searchCast = (SearchObjective)objective;
                    entry.FirstEntry = searchCast.Location;
                    break;

                default:
                    break;
            }

            entry.Type = Enum.GetName(typeof(GoalType), objective.ObjectiveType);

            objectivesEntries.Add(entry);
        }

        return objectivesEntries;
    }

    #endregion Private Methods

    #region Public Methods

    public void ReadDatabase(int saveFileID)
    {
        QuestList _questList = new QuestList();

        _questList = JsonUtility.FromJson<QuestList>(GetPlayerQuestDatabase(saveFileID));

        if (_questList.QuestEntry.Count == 0)
        {
            Debug.LogError("There are no quests in the database.");
        }

        foreach (QuestEntry quest in _questList.QuestEntry)
        {
            Quest loadedQuest = new Quest();

            loadedQuest.Title = quest.Title;
            loadedQuest.Description = quest.Description;
            loadedQuest.QuestGiverName = quest.Giver;

            if (quest.ObjectivesEntry.Count == 0)
            {
                Debug.LogError("Quest: " + loadedQuest.Title + " has no objectives.");
            }

            loadedQuest.Objectives = ReadObjectives(quest, loadedQuest);

            switch (quest.Status)
            {
                case 0:

                    // Quest has not been added to journal
                    loadedQuest.Status = QuestStatus.NOTACCEPTED;
                    QuestManager.Instance.GiveQuestToQuestGiver(loadedQuest);
                    break;

                case 1:

                    // Quest has been accepted but is not completed
                    loadedQuest.Status = QuestStatus.GIVEN;
                    QuestManager.Instance.AddQuestToJournal(loadedQuest);
                    break;

                case 2:

                    // Quest has been accepted and completed
                    loadedQuest.Status = QuestStatus.COMPLETE;
                    QuestManager.Instance.AddQuestToJournal(loadedQuest);
                    break;
            }
        }
    }

    public void SaveToDatabase(int saveFileID, List<Quest> quests)
    {
        QuestList _questList = new QuestList();

        foreach (Quest quest in quests)
        {
            QuestEntry entry = new QuestEntry();
            entry.Title = quest.Title;
            entry.Description = quest.Description;
            entry.Giver = quest.QuestGiverName;

            entry.Status = (int)quest.Status;

            entry.ObjectivesEntry = WriteObjectives(quest);

            _questList.QuestEntry.Add(entry);
        }

        File.Delete(Application.persistentDataPath + "/playerQuestDB" + saveFileID + ".json");
        File.WriteAllText(Application.persistentDataPath + "/playerQuestDB" + saveFileID + ".json", JsonUtility.ToJson(_questList));
    }

    #endregion Public Methods
}