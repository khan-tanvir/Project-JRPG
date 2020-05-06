using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singlton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of Inventory found");
        }

        instance = this;
    }
    #endregion

    public int space = 20;

    public List<Item> items = new List<Item>();
    public bool Add (Item item)
    {
        if (!item.isDefaultItme)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room");
                return false; 
            }
            items.Add(item);
        }
        return true;
    }

    public void Removed(Item item)
    {
        items.Remove(item);
    }
}
