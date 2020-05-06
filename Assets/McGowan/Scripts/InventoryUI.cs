using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemParents;

    Inventory inventory;

    InventorySlot[] Slots;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangeCallback += UpdateUI;

        Slots = itemParents.GetComponentsInChildren<InventorySlot>();
    }
    // Update is called once per frame
    void UpdateUI()
    {
        for (int i = 0; i<Slots.Length; i++)
        {
            if (i< inventory.items.Count)
            {
                Slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                Slots[i].ClearSlot();
            }
        }
    }
}
