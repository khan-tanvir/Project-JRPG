using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scriptQuestManager : MonoBehaviour
{
    public static scriptQuestManager _questManager;

    [SerializeField]
    private GameObject _journalMenu;

    [SerializeField]
    private Transform _listTransform;

    private scriptQuest currentSelectedQuest;

    [SerializeField]
    private TMP_Text _descriptionPanel;

    private void Awake()
    {
        if (_questManager == null)
        {
            DontDestroyOnLoad(gameObject);
            _questManager = this;
        }
        else if (_questManager != null)
            Destroy(gameObject);
    }

    public void AddQuestToJournal(GameObject questObjectPrefab, scriptQuest quest)
    {
        GameObject questObject = Instantiate(questObjectPrefab, _listTransform);

        // Both questscript and non mb version need to have reference of each other
        scriptQuestMB temp = questObject.GetComponent<scriptQuestMB>();
        quest.QuestMB = temp;
        temp.Quest = quest;

        questObject.GetComponent<TMP_Text>().text = quest.Title;
    }

    public void ShowDescription(scriptQuest quest)
    {
        if (currentSelectedQuest != null && currentSelectedQuest != quest)
        {
            currentSelectedQuest.QuestMB.OnDeselect();
        }

        currentSelectedQuest = quest;
        string objectives = "\n";

        for (int i = 0; i < currentSelectedQuest.Objectives.Count; i++)
        {
            if (currentSelectedQuest.Objectives[i] != null)
            {
                objectives += currentSelectedQuest.Objectives[i].Information + "\n";
            }
        }

        string textToDisplay = string.Format("{0}\n\n<size=25>{1}</size><size=20>{2}</size>", currentSelectedQuest.Title, currentSelectedQuest.Description, objectives);

        _descriptionPanel.text = textToDisplay;
    }
}
