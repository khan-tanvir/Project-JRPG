using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class scriptPickUp : IInteractable, MonoBehaviour
{
    // Attach this to any object that can be stored in the inventory

    [SerializeField]
    private string _itemName;

    public string ItemName
    {
        get { return _itemName; }
    }

    private void Start()
    {
        if (scriptItemList.ItemDatabase.GetItemByName(_itemName) == null)
        {
            DestroyObject(gameObject);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        // Pick up if player is within collision radius and if the player is pressing the E key
        if (other.CompareTag("Player")) //&& Input.GetKey(KeyCode.E))
        {
            scriptInventory _inventory = scriptInventory.Inventory;
            
            for (int i = 0; i < _inventory.Slot.Count; i++)
            {
                if (!(_inventory.Slot[i].IsFilled))
                {
                    // Create the item we want to spawn and store it in temp so it can be referenced
                    GameObject temp = Instantiate(gameObject, _inventory.Slot[i].transform, false);

                    // Reset the local scale
                    temp.transform.localScale = new UnityEngine.Vector3(1.0f, 1.0f, 1.0f);

                    // Toggle the components so that they'll only be used when needed
                    temp.gameObject.GetComponent<Image>().enabled = true;
                    temp.gameObject.GetComponent<SpriteRenderer>().enabled = false;

                    _inventory.Slot[i].StoreItem(scriptItemList.ItemDatabase.GetItemByName(_itemName));

                    Destroy(gameObject);

                    // Calls the Gather Objective Change Event
                    scriptGameEvents._gameEvents.GatherObjectiveChange(_inventory.Slot[i].ItemName);

                    _inventory.Slot[i].DisablePickUp();

                    return;
                }
            }
        }
    }
}
