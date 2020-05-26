using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestMB : MonoBehaviour
{
    public TMP_Text TextComp
    {
        get { return GetComponent<TMP_Text>(); }
    }

    // Get reference to non-MB 
    public Quest Quest
    {
        get;
        set;
    }
    public void UpdateColor()
    {
        switch (Quest.Status)
        {
            case QuestStatus.NOTACCEPTED:
                break;
            case QuestStatus.GIVEN:
                TextComp.color = Color.white;
                break;
            case QuestStatus.COMPLETE:
                TextComp.color = Color.green;
                break;
            default:
                break;
        }
    }

    public void OnClick()
    {
        QuestManager.Instance.ShowDescription(Quest);
        TextComp.color = Color.yellow;
    }

    public void OnDeselect()
    {
        
    }
}
