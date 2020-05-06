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

    public delegate void OnItemChange();
    public OnItemChange onItemChangeCallback;

    public int space = 20; // number of slot avaible in Inventory

    public List<Item> items = new List<Item>();
    public bool Add (Item item)
    {
        if (!item.isDefaultItme)
        {
            if (items.Count >= space) // checks if the number within the list more than or eqaul the same as the space varaible 
            {
                Debug.Log("Not enough room"); // if so the player can't add items 
                return false; 
            }
            items.Add(item); // else this will allow the item to be added
            
            if (onItemChangeCallback != null)
            onItemChangeCallback.Invoke();
        }
        return true;
    }

    public void Removed(Item item)
    {
        items.Remove(item);

        if (onItemChangeCallback != null)
            onItemChangeCallback.Invoke();  
    }
}
