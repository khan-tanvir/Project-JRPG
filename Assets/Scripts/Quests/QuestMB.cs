using TMPro;
using UnityEngine;

public class QuestMB : MonoBehaviour
{
    #region Public Properties

    // Get reference to non-MB
    public Quest Quest
    {
        get;
        set;
    }

    public TMP_Text TextComp
    {
        get { return GetComponent<TMP_Text>(); }
    }

    #endregion Public Properties

    #region Public Methods

    public void OnClick()
    {
        QuestManager.Instance.ShowDescription(Quest);
        TextComp.color = Color.yellow;
    }

    public void OnDeselect()
    {
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

    #endregion Public Methods
}