using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class QuestGiver : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject _questPrefab;
    [SerializeField]
    private GameObject _questGiverPanel;

    // Create a separate script for this
    [SerializeField]
    private Material _interactionMat;
    [SerializeField]
    private Material _defaultMat;

    public List<Quest> Quests
    {
        get;
        internal set;
    }

    public string NPCName
    {
        get;
        set;
    }

    public bool EnabledInteraction
    {
        get;
        set;
    }

    public bool InfiniteUses
    {
        get
        {
            // TODO: Setup conditions for this
            return true;
        }
    }
    private void Awake()
    {
        Quests = new List<Quest>();
        EnabledInteraction = true;

        // TODO: Prefab npcs for name
        // Debug
        NPCName = "NPC_NAME";
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _questGiverPanel.SetActive(false);
        }
    }

    public void CreateQuest(Quest quest)
    {
        foreach (Objective objective in quest.Objectives)
        {
            objective.Parent = quest;
            QuestManager.Instance.UpdateDescription(objective);
        }

        Quests.Add(quest);
    }

    public void Focus()
    {
        gameObject.GetComponent<Renderer>().material = _interactionMat;
    }

    public void UnFocus()
    {
        if (EnabledInteraction)
            gameObject.GetComponent<Renderer>().material = _defaultMat;
    }

    public void OnInteract()
    {
        _questGiverPanel.SetActive(true);
        QuestGiverPanel.Instance.ShowQuests(this);
    }
}