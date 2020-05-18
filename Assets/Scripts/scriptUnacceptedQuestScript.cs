using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scriptUnacceptedQuestScript : MonoBehaviour
{
    public scriptQuest Quest
    {
        get;
        set;
    }
    
    // Start is called before the first frame update
    public void OnClick()
    {
        scriptQuestGiverPanel.QuestGiverPanel.ShowDescription(Quest);
    }

    public void OnDeselect()
    {
        GetComponent<TMP_Text>().color = Color.white;
    }
}
