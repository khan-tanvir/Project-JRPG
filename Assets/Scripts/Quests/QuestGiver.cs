using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private string _giverName;

    [SerializeField]
    private GameObject _questGiverPanel;

    [SerializeField]
    private GameObject _questPrefab;

    #endregion Private Fields

    #region Public Properties

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

    public string NPCName
    {
        get => _giverName;
        set => _giverName = value;
    }

    public List<Quest> Quests
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Private Methods

    private void Awake()
    {
        Quests = new List<Quest>();
        EnabledInteraction = true;

        if (string.IsNullOrEmpty(_giverName))
        {
            _giverName = GetComponent<AIController>().NPCName;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _questGiverPanel.SetActive(false);
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void CreateQuest(Quest quest)
    {
        foreach (Objective objective in quest.Objectives)
        {
            objective.Parent = quest;
            QuestManager.Instance.UpdateDescription(objective);
        }

        Quests.Add(quest);
    }

    public void OpenPanel()
    {
        _questGiverPanel.SetActive(true);
        QuestGiverPanel.Instance.ShowQuests(this);
    }

    #endregion Public Methods
}