using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class scriptQuestGiver : MonoBehaviour
{
    // Reference to the quest manager
    [SerializeField]
    private scriptQuestManager _questManager;

    [SerializeField]
    private List<scriptQuest> _quests = new List<scriptQuest>();

    [SerializeField]
    private GameObject _questPrefab;

    [SerializeField]
    private GameObject _questGiverPanel;

    private bool _questCreated;

    private List<scriptsObjective> _objectives = new List<scriptsObjective>();

    public List<scriptQuest> Quests
    {
        get { return _quests; }
    }

    private void Awake()
    {
        foreach (scriptQuest quest in _quests)
        {
            AddAllObjectives(quest);
            CreateQuest(quest);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _questGiverPanel.SetActive(true);
            _questGiverPanel.GetComponent<scriptQuestGiverPanel>().ShowQuests(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _questGiverPanel.SetActive(false);
        }
    }

    private void AddAllObjectives(scriptQuest quest)
    {
        foreach (GatherObjective obj in quest._gatherObjectives)
        {
            _objectives.Add(obj);
        }

        foreach (EscortObjective obj in quest._escortObjectives)
        {
            _objectives.Add(obj);
        }

        foreach (DeliverObjective obj in quest._deliverObjectives)
        {
            _objectives.Add(obj);
        }

        foreach (ActivateObjective obj in quest._activateObjectives)
        {
            _objectives.Add(obj);
        }

        foreach (SearchObjective obj in quest._searchObjectives)
        {
            obj.AssignTrigger();
            _objectives.Add(obj);
        }

        ClearObjectives(quest);
    }

    public void CreateQuest(scriptQuest quest)
    {
        foreach (scriptsObjective objective in _objectives)
        {
            _questManager.UpdateDescription(objective);
        }

        quest.Objectives.AddRange(_objectives);
        _objectives.Clear();
    }

    private void ClearObjectives(scriptQuest quest)
    {
        quest._gatherObjectives = null;
        quest._activateObjectives = null;
        quest._searchObjectives = null;
        quest._escortObjectives = null;
        quest._deliverObjectives = null;
    }
}
