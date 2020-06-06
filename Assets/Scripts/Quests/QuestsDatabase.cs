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
            return;
        }

        QuestManager.Instance.AddToQuestsList(_questList);
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