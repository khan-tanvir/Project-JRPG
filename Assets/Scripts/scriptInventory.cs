using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scriptInventory : MonoBehaviour
{
    [SerializeField]
    private int _numberOfSlots = 0;

    private List<scriptSlot> _slots = new List<scriptSlot>();

    private static scriptInventory _inventory;

    [SerializeField]
    private GameObject _slotObject;

    [SerializeField]
    private Transform _slotContainer;

    public List<scriptSlot> Slot
    {
        get { return _slots; }
        set { _slots = value; }
    }

    public static scriptInventory Inventory
    {
        get { return _inventory; }
    }

    private void Awake()
    {
        _inventory = this;

        // Create slots based on number 
        for (int i = 0; i < _numberOfSlots; i++)
        {
            GameObject temp = Instantiate(_slotObject, _slotContainer);
            _slots.Add(temp.GetComponent<scriptSlot>());
        }

        try
        {
            if (scriptGameData.GameDataManager.InventoryItems != null)
                LoadInventory();
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Load from the menu scene to avoid errors.\n" + e.Message);
        }
    }

    public void LoadInventory()
    {
        InventoryItem temp = null;

        for (int i = 0; i < scriptGameData.GameDataManager.InventoryItems.Count; i++)
        {
            temp = scriptGameData.GameDataManager.InventoryItems[i];

            var loadedObject = scriptItemList.ItemDatabase.GetItemPrefab(temp.itemID);

            GameObject createdGameObject = Instantiate(loadedObject, _inventory.Slot[temp.Position].transform, false) as GameObject;

            createdGameObject.transform.localScale = new UnityEngine.Vector3(1.0f, 1.0f, 1.0f);

            createdGameObject.gameObject.GetComponent<Image>().enabled = true;
            createdGameObject.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            _slots[temp.Position].StoreItem(scriptItemList.ItemDatabase.GetItemByID(temp.itemID));
        }
    }

    public int GetItemCount(string item)
    {
        int total = 0;

        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].IsFilled && _slots[i].ItemName == item)
                total++;
        }

        return total;
    }
    
    public void SaveInventory()
    {
        List<InventoryItem> itemsToStore = new List<InventoryItem>();

        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].IsFilled)
            {
                InventoryItem item = new InventoryItem(scriptItemList.ItemDatabase.GetItemByName(_slots[i].ItemName).ID, i);
                itemsToStore.Add(item);
            }
        }

        scriptGameData.GameDataManager.InventoryItems = itemsToStore;
    }
}
