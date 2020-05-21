using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestMB : MonoBehaviour
{
    // Get reference to non-MB 
    public Quest Quest
    {
        get;
        set;
    }

    public void OnClick()
    {
        QuestManager.Instance.ShowDescription(Quest);
        GetComponent<TMP_Text>().color = Color.yellow;
    }

    public void OnDeselect()
    {
        GetComponent<TMP_Text>().color = Color.white;
    }
}
