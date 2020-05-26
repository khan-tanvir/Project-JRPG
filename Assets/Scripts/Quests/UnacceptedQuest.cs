using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnacceptedQuest : MonoBehaviour
{
    public Quest Quest
    {
        get;
        set;
    }
    
    // Start is called before the first frame update
    public void OnClick()
    {
        QuestGiverPanel.Instance.ShowDescription(Quest);
    }

    public void OnDeselect()
    {
        GetComponent<TMP_Text>().color = Color.white;
    }
}
