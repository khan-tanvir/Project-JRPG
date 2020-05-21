using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour, IInteractable
{
    // Attach this to any object that should/can be stored in the inventory

    [SerializeField]
    private string _itemName;

    [SerializeField]
    private Material interactionMat;

    [SerializeField]
    private Material defaultMat;

    public string ItemName
    {
        get { return _itemName; }
    }

    public bool EnabledInteraction
    {
        // check this
        get;
        set;
    }

    public bool InfiniteUses
    {
        // check this
        get
        {
            return true;
        }
    }

    private void Start()
    {
        if (ItemDatabase.Instance.GetItemByName(_itemName) == null)
        {
            Destroy(gameObject);
        }

        EnabledInteraction = true;
    }
    
    public void Focus()
    {
        if (EnabledInteraction)
            gameObject.GetComponent<Renderer>().material = interactionMat;
    }

    public void OnInteract()
    {
        AddItemToInventory();
    }

    public void UnFocus()
    {
        if (EnabledInteraction)
            gameObject.GetComponent<Renderer>().material = defaultMat;
    }

    private void AddItemToInventory()
    {

        for (int i = 0; i < Inventory.Instance.Slots.Count; i++)
        {
            if (Inventory.Instance.Slots[i].InvItem == null)
            {
                EnabledInteraction = false;
                // Create the item we want to spawn and store it in temp so it can be referenced
                GameObject temp = Instantiate(gameObject, Inventory.Instance.Slots[i].transform, false);

                // Reset the local scale
                temp.transform.localScale = new UnityEngine.Vector3(1.0f, 1.0f, 1.0f);

                // Toggle the components so that they'll only be used when needed
                temp.gameObject.GetComponent<Image>().enabled = true;
                temp.gameObject.GetComponent<SpriteRenderer>().enabled = false;

                Inventory.Instance.Slots[i].StoreItem(ItemDatabase.Instance.GetItemByName(_itemName));

                // Calls the Gather Objective Change Event
                EventsManager.Instance.GatherObjectiveChange(Inventory.Instance.Slots[i].InvItem.Name);

                Inventory.Instance.Slots[i].DisablePickUp();

                Destroy(gameObject);

                return;
            }
        }
    }
}
