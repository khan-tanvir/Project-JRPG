using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour, IInteractable
{
    #region Private Fields

    [SerializeField]
    private Material _defaultMat;

    [SerializeField]
    private string _giverName;

    // Create a separate script for this
    [SerializeField]
    private Material _interactionMat;

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
        get { return _giverName; }
        set { _giverName = value; }
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

    public void Focus()
    {
        gameObject.GetComponent<Renderer>().material = _interactionMat;
    }

    public void OnInteract()
    {
        _questGiverPanel.SetActive(true);
        QuestGiverPanel.Instance.ShowQuests(this);
    }

    public void UnFocus()
    {
        if (EnabledInteraction)
            gameObject.GetComponent<Renderer>().material = _defaultMat;
    }

    #endregion Public Methods
}