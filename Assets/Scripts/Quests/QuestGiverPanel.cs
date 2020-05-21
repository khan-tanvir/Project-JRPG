using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGiverPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _questPrefab;

    [SerializeField]
    private Transform _questArea;

    [SerializeField]
    private GameObject _descriptionPanel;

    [SerializeField]
    private GameObject _questsPanel;

    [SerializeField]
    private GameObject _acceptButton;

    [SerializeField]
    private GameObject _backButton;

    private Quest _currentSelectedQuest;

    private QuestGiver _questGiver;

    public static QuestGiverPanel Instance
    {
        get;
        internal set;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        ClearPanel();
    }

    public void ShowQuests(QuestGiver questGiver)
    {
        _questGiver = questGiver;

        ClearPanel();

        foreach (Quest quest in questGiver.Quests)
        {
            GameObject temp = Instantiate(_questPrefab, _questArea);
            temp.GetComponent<TMP_Text>().text = quest.Title;

            temp.GetComponent<UnacceptedQuest>().Quest = quest;
        }
    }

    public void ClearPanel()
    {
        foreach (Transform child in _questArea)
        {
            Destroy(child.gameObject);
        }
    }

    public void SwitchBetweenWindows()
    {
        _descriptionPanel.SetActive(!_descriptionPanel.activeInHierarchy);
        _questsPanel.SetActive(!_questsPanel.activeInHierarchy);
        _acceptButton.SetActive(!_acceptButton.activeInHierarchy);
        _backButton.SetActive(!_backButton.activeInHierarchy);

        _currentSelectedQuest = null;
    }

    public void ShowDescription(Quest quest)
    {
        SwitchBetweenWindows();

        _currentSelectedQuest = quest;

        string objectives = "\n";

        foreach (Objective objective in _currentSelectedQuest.Objectives)
        {
            objectives += objective.Information + "\n";
        }

        string textToDisplay = string.Format("{0}\n\n<size=25>{1}</size><size=20>{2}</size>", _currentSelectedQuest.Title, _currentSelectedQuest.Description, objectives);

        _descriptionPanel.GetComponentInChildren<TMP_Text>().text = textToDisplay;
    }

    public void GiveQuestToPlayer()
    {
        if (_currentSelectedQuest != null)
        {
            QuestManager.Instance.AddQuestToJournal(_currentSelectedQuest);

            foreach (Transform child in _questArea)
            {
                if (child.GetComponent<UnacceptedQuest>().Quest == _currentSelectedQuest)
                    Destroy(child.gameObject);
            }

            _questGiver.Quests.Remove(_currentSelectedQuest);
            SwitchBetweenWindows();
        }
    }
}