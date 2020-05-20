using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

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
            scriptQuest loadedQuest = new scriptQuest();

            loadedQuest.Title = quest.Title;
            loadedQuest.Description = quest.Description;
            loadedQuest.QuestGiverName = quest.Giver;


            loadedQuest.Objectives = ReadObjectives(quest);

            if (quest.IsComplete)
                GiveQuestToJournal(loadedQuest);
            else
                GiveQuestToQuestGiver(loadedQuest);
        }
    }

    private void GiveQuestToJournal(scriptQuest quest)
    {
        scriptQuestManager._questManager.AddQuestToJournal(quest);
    }

    private void GiveQuestToQuestGiver(scriptQuest quest)
    {
        // Find NPC with the quest name
        //Object.FindObjectOfType<scriptQuestGiver>();
    }

    private List<scriptsObjective> ReadObjectives(QuestEntry quest)
    {
        List<scriptsObjective> loadedObjectives = new List<scriptsObjective>();

        foreach (ObjectivesEntry objective in quest.ObjectivesEntry)
        {
            scriptsObjective temp = null;
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



