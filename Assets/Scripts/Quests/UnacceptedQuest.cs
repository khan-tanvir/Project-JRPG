using TMPro;
using UnityEngine;

public class UnacceptedQuest : MonoBehaviour
{
    #region Public Properties

    public Quest Quest
    {
        get;
        set;
    }

    #endregion Public Properties

    #region Public Methods

    // Start is called before the first frame update
    public void OnClick()
    {
        QuestGiverPanel.Instance.ShowDescription(Quest);
    }

    public void OnDeselect()
    {
        GetComponent<TMP_Text>().color = Color.white;
    }

    #endregion Public Methods
}