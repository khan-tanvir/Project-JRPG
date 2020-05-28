using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private int _numberOfSlots;

    [SerializeField]
    private Transform _slotContainer;

    [SerializeField]
    private GameObject _slotObject;

    #endregion Private Fields

    #region Public Properties

    public static Inventory Instance
    {
        get;
        internal set;
    }

    public List<Slot> Slots
    {
        get;
        set;
    }

    #endregion Public Properties

    #region Private Methods

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

    #endregion Private Methods

    #region Public Methods

    public bool FillSlot(GameObject objectToInstantiate)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].InvItem == null)
            {
                objectToInstantiate.GetComponent<Renderer>().material = objectToInstantiate.GetComponent<ItemMB>().defaultMat;

                GameObject itemObject = Instantiate(objectToInstantiate, Slots[i].transform, false);

                itemObject.GetComponent<ItemMB>().Item = objectToInstantiate.GetComponent<ItemMB>().Item;

                if (itemObject.GetComponent<ItemMB>().Item == null)
                {
                    return false;
                }

                itemObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                itemObject.GetComponent<IStoreable>().ImageComp.enabled = true;
                itemObject.GetComponent<IStoreable>().SpriteRendererComp.enabled = false;

                Slots[i].StoreItem(itemObject.GetComponent<ItemMB>().Item);

                EventsManager.Instance.GatherObjectiveChange(Slots[i].InvItem.Name);

                return true;
            }
        }

        return false;
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

            if (createdGameObject.GetComponent<ItemMB>().Item == null)
            {
                createdGameObject.GetComponent<ItemMB>().Item = new Item(ItemDatabase.Instance.GetItemByID(item.ID));
            }

            createdGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            createdGameObject.gameObject.GetComponent<Image>().enabled = true;
            createdGameObject.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            Slots[item.Position].StoreItem(ItemDatabase.Instance.GetItemByID(item.ID));
        }
    }

    public void RemoveItem(string item)
    {
        int index = Slots.FindIndex(slot => slot.InvItem.Name.Equals(item));

        if (index == -1)
        {
            Debug.LogError("Item not found.");
            return;
        }

        Slots[index].RemoveItem();
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

    #endregion Public Methods
}