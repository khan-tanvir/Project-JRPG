using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class scriptQuestGiver : MonoBehaviour
{
    // Reference to the quest manager
    [SerializeField]
    public scriptQuestManager _questManager;

    [SerializeField]
    private List<scriptQuest> quest;

    [SerializeField]
    private GameObject _questPrefab;

    private bool _questCreated;

    // TODO: I wish I could find a better way to do this but empty array declaration does not take up much memory
    [SerializeField]
    private List<GatherObjective> gatherObjectives;

    [SerializeField]
    private List<EscortObjective> escortObjectives;

    [SerializeField]
    private List<DeliverObjective> deliverObjectives;

    [SerializeField]
    private List<ActivateObjective> activateObjectives;

    [SerializeField]
    private List<SearchObjective> searchObjectives;


    private void Awake()
    {
        // DEBUGGING
        CreateQuest(quest[0]);
        _questManager.AddQuestToJournal(_questPrefab, quest[0]);
    }

    public void CreateQuest(scriptQuest quest)
    {
        // TODO: turn this in to a list
        if (gatherObjectives != null)
        {
            for (int i = 0; i < gatherObjectives.Count; i++)
            {
                _questManager.UpdateDescription(gatherObjectives[i]);
                quest.Objectives.Add(gatherObjectives[i]);
            }
        }

        if (escortObjectives != null)
        {
            for (int i = 0; i < escortObjectives.Count; i++)
            {
                _questManager.UpdateDescription(escortObjectives[i]);
                quest.Objectives.Add(escortObjectives[i]);
            }
        }

        if (deliverObjectives != null)
        {
            for (int i = 0; i < deliverObjectives.Count; i++)
            {
                _questManager.UpdateDescription(deliverObjectives[i]);
                quest.Objectives.Add(deliverObjectives[i]);
            }
        }

        if (activateObjectives != null)
        {
            for (int i = 0; i < activateObjectives.Count; i++)
            {
                quest.Objectives.Add(activateObjectives[i]);
            }
        }

        if (searchObjectives != null)
        {
            for (int i = 0; i < searchObjectives.Count; i++)
            {
                quest.Objectives.Add(searchObjectives[i]);
            }
        }
    }
}
