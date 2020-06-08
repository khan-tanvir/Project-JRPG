using System.Collections.Generic;
using System.Linq;
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

    private void ReOrderSlots()
    {
        Slots.OrderByDescending(i => i.InvItem);
    }

    public bool FillSlot(GameObject objectToInstantiate, string questItem = "")
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].InvItem == null)
            {
                GameObject itemObject = Instantiate(objectToInstantiate, Slots[i].transform, false);

                itemObject.GetComponent<ItemMB>().Item = objectToInstantiate.GetComponent<ItemMB>().Item;

                if (itemObject.GetComponent<ItemMB>().Item == null)
                {
                    return false;
                }
                itemObject.GetComponent<Renderer>().material = objectToInstantiate.GetComponent<ItemMB>().defaultMat;

                itemObject.GetComponent<IDGenerator>().Instance = objectToInstantiate.GetComponent<IDGenerator>().Instance;

                itemObject.GetComponent<IDGenerator>().Instance.InInventory = true;

                if (string.IsNullOrEmpty(questItem))
                {
                    Slots[i].ObjectID = itemObject.GetComponent<IDGenerator>().Instance.ObjectID;
                    Slots[i].InitialPosition = itemObject.GetComponent<IDGenerator>().Instance.InitialPosition;
                }
                else
                {
                    Slots[i].ObjectID = "Quest Item: " + questItem;
                    Slots[i].InitialPosition = default;
                }

                itemObject.GetComponent<IStoreable>().ImageComp.enabled = true;
                itemObject.GetComponent<IStoreable>().SpriteRendererComp.enabled = false;

                Slots[i].StoreItem(itemObject.GetComponent<ItemMB>().Item);

                SceneManagerScript.Instance.AddToSceneObjectList(itemObject.GetComponent<IDGenerator>().Instance);

                EventsManager.Instance.GatherObjectiveChange(Slots[i].InvItem.Name);

                ReOrderSlots();

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

            Slots[item.Position].ObjectID = item.ObjectID;

            createdGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            createdGameObject.gameObject.GetComponent<Image>().enabled = true;
            createdGameObject.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            Slots[item.Position].StoreItem(ItemDatabase.Instance.GetItemByID(item.ID));
        }
    }

    public void RemoveItem(string item)
    {
        int index = Slots.FindIndex(slot => slot.ObjectID == "Quest Item: " + item);

        if (index == -1)
        {
            Debug.LogError("Item not found.");
            return;
        }

        Slots.RemoveAt(index);
    }

    public void SaveInventory()
    {
        List<InventoryItem> itemsToStore = new List<InventoryItem>();

        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].InvItem != null)
            {
                Debug.Log("Saving at postion " + i);
                InventoryItem item = new InventoryItem(ItemDatabase.Instance.GetItemByName(Slots[i].InvItem.Name).ID, i, Slots[i].ObjectID);
                itemsToStore.Add(item);
            }
        }

        GameData.Instance.PlayerData.InventoryItems = itemsToStore;
    }

    #endregion Public Methods
}