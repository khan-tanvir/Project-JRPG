using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kosodate : AIController
{
    #region Public Methods

    public override void Focus()
    {
        base.Focus();
    }

    public override void OnInteract()
    {
        // TODO: Instantly hand out quests, scrap the quest giver panel

        if (EnabledInteraction)
        {
            if (GetComponent<QuestGiver>().Quests.Count != 0)
            {
                GetComponent<QuestGiver>().OpenPanel();
            }

            if (QuestManager.Instance.Quests.Find(a => a.Title == "Find my Baby").Status == QuestStatus.COMPLETE)
            {
                Inventory.Instance.RemoveItem("Baby");
                FindObjectOfType<Hand>().AddItemToInventory();
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