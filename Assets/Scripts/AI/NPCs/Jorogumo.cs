using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jorogumo : AIController
{
    #region Public Methods

    public override void Focus()
    {
        base.Focus();
    }

    public override void OnInteract()
    {
        if (EnabledInteraction)
        {
            if (GetComponent<QuestGiver>().Quests.Count != 0 && QuestManager.Instance.Quests.Find(a => a.Title == "Find my Baby").Status == QuestStatus.COMPLETE)
            {
                GetComponent<QuestGiver>().OpenPanel();
            }

            if (QuestManager.Instance.Quests.Find(a => a.Title == "Helping Hand").Status == QuestStatus.COMPLETE)
            {
                Inventory.Instance.RemoveItem("Hand");

                //FindObjectOfType<Hand>().AddItemToInventory();
            }
        }

        base.OnInteract();
    }

    public override void UnFocus()
    {
        base.UnFocus();
    }

    #endregion Public Methods
}