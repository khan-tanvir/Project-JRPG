using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public struct DataEntry
{
    public string FirstEntry;
    public string SecondEntry;
}

[System.Serializable]
public struct ObjectivesEntry
{
    public string Type;
    public string Description;

    public List<DataEntry> DataEntry;
}

[System.Serializable]
public struct QuestEntry
{
    public int ID;

    public string Title;
    public string Description;
    public string Giver;

    public bool IsComplete;

    public List<ObjectivesEntry> ObjectivesEntry;
}

public class QuestList
{
    public List<QuestEntry> QuestEntry = new List<QuestEntry>();
}

public class QuestsDatabase
{
    public QuestList _questList = new QuestList();

    public TextAsset FileToRead
    {
        internal get;
        set;
    }

    public void ReadDatabase()
    {
        _questList = JsonUtility.FromJson<QuestList>(FileToRead.text);
        
        foreach(QuestEntry quest in _questList.QuestEntry)
        {
            Quest loadedQuest = new Quest();

            loadedQuest.Title = quest.Title;
            loadedQuest.Description = quest.Description;
            loadedQuest.QuestGiverName = quest.Giver;

            loadedQuest.QuestComplete = quest.IsComplete;

            loadedQuest.Objectives = ReadObjectives(quest);

            if (loadedQuest.QuestComplete)
                GiveQuestToJournal(loadedQuest);
            else
                CallGiveQuestToQuestGiver(loadedQuest);
        }
    }

    private void GiveQuestToJournal(Quest quest)
    {
        QuestManager.Instance.AddQuestToJournal(quest);
    }

    private void CallGiveQuestToQuestGiver(Quest quest)
    {
        // Find NPC with the quest name
        //Object.FindObjectOfType<scriptQuestGiver>();
        QuestManager.Instance.GiveQuestToQuestGiver(quest);

    }

    private List<Objective> ReadObjectives(QuestEntry quest)
    {
        List<Objective> loadedObjectives = new List<Objective>();

        foreach (ObjectivesEntry objective in quest.ObjectivesEntry)
        {
            Objective temp = null;
            switch (objective.Type)
            {
                case "Gather":
                    temp = new GatherObjective(objective.Description, objective.DataEntry[0].FirstEntry, int.Parse(objective.DataEntry[0].SecondEntry));
                    break;
                case "Escort":
                    break;
                case "Search":
                    break;
                case "Activate":
                    break;
                case "Deliver":
                    break;
                default:
                    break;
            }
            if (temp != null)
                loadedObjectives.Add(temp);
        }

        return loadedObjectives;
    }

}



