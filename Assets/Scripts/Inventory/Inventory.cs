using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
                
                if (itemObject.GetComponent<IStoreable>().ImageComp == null)
                {
                    objectToInstantiate.GetComponent<Image>();
                }

                if (itemObject.GetComponent<IStoreable>().SpriteRendererComp == null)
                {
                    objectToInstantiate.GetComponent<SpriteRenderer>();
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

    public void DropItem(Item item)
    {
        int index = Slots.FindIndex(slot => slot.InvItem.Equals(item));

        Vector2 playerPos = FindObjectOfType<Player>().GetComponent<Rigidbody2D>().transform.position;

        // Set the range for dropping them item and the z value has to be -1 so it can be seen from the main camera
        Vector3 dropPos = new Vector3(playerPos.x + UnityEngine.Random.Range(-1.5f, 1.5f), playerPos.y + UnityEngine.Random.Range(-1.5f, 1.5f), -1.0f);

        Transform child = Slots[index].transform.Find(item.Name);

        Instantiate(child.gameObject, dropPos, Quaternion.identity);

        Destroy(child.gameObject);

        //// Adjust the instaniated object
        //temp.transform.localScale = new Vector2(1.0f, 1.0f);

        //Destroy
    }
}
