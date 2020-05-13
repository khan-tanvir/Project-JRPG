using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptInventory : MonoBehaviour
{
    [SerializeField]
    private List<scriptSlot> _slots = new List<scriptSlot>();

    public static scriptInventory _inventory;

    // TODO: Dynamic creation of slots?

    public List<scriptSlot> Slot
    {
        get { return _slots; }
        set { _slots = value; }
    }

    public scriptInventory Inventory
    {
        get { return _inventory; }
    }

    private void Awake()
    {
        _inventory = this;
    }

    public int GetItemCount(string item)
    {
        int total = 0;

        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].IsFilled && _slots[i].ItemName == item)
                total++;
        }

        Debug.Log(total);

        return total;
    }
}
