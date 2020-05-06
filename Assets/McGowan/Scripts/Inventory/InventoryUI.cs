using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemParents;
    public GameObject inventoryUI;

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
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        
    }
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
