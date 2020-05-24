using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int _numberOfSlots;

    [SerializeField]
    private GameObject _slotObject;

    [SerializeField]
    private Transform _slotContainer;

    public List<Slot> Slots
    {
        get;
        set;
    }

    public static Inventory Instance
    {
        get;
        internal set;
    }

    private void Awake()
    {
        Instance = this;
        Slots = new List<Slot>();

        CreateInventorySlots();

        if (GameData.Instance.PlayerData?.InventoryItems != null)
        {
            LoadInventory();
        }
    }

    private void CreateInventorySlots()
    {
        for (int i = 0; i < _numberOfSlots; i++)
        {
            GameObject temp = Instantiate(_slotObject, _slotContainer);
            Slots.Add(temp.GetComponent<Slot>());
        }
    }

    public void LoadInventory()
    {
        for (int i = 0; i < GameData.Instance.PlayerData.InventoryItems.Count; i++)
        {
            InventoryItem item = GameData.Instance.PlayerData.InventoryItems[i];

            var loadedObject = ItemDatabase.Instance.GetItemPrefab(item.ID);

            if (loadedObject == null)
            {
                Debug.LogError("loadedObject is null, check the item ID");
                return;
            }
                
            GameObject createdGameObject = Instantiate(loadedObject, Slots[item.Position].transform, false) as GameObject;

            createdGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            createdGameObject.gameObject.GetComponent<Image>().enabled = true;
            createdGameObject.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            Slots[item.Position].StoreItem(ItemDatabase.Instance.GetItemByID(item.ID));
        }
    }

    public int GetItemCount(string item)
    {
        int total = 0;

        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].InvItem != null && Slots[i].InvItem.Name == item)
            {
                total++;
            }
        }

        return total;
    }
    
    public void SaveInventory()
    {
        List<InventoryItem> itemsToStore = new List<InventoryItem>();

        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].InvItem != null)
            {
                InventoryItem item = new InventoryItem(ItemDatabase.Instance.GetItemByName(Slots[i].InvItem.Name).ID, i);
                itemsToStore.Add(item);
            }
        }

        GameData.Instance.PlayerData.InventoryItems = itemsToStore;
    }
}
