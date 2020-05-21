using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // All items that exist in the game will be stored in this database
   public List<Item> Items
    {
        get;
        internal set;
    }

    public static ItemDatabase Instance
    {
        get;
        internal set;
    }

    private void Awake()
    {
        Instance = this;

        CreateDatabase();
    }

    public void CreateDatabase()
    {
        // All items need to be hardcoded
        Items = new List<Item>()
        {
            new Item(1, "Test", "Test")
        };
    }

    public Item GetItemByID(int id)
    {
        return Items.Find(item => item.ID == id);
    }

    public Item GetItemByName(string name)
    {
        return Items.Find(item => item.Name == name);
    }

    public UnityEngine.Object GetItemPrefab(int id)
    {
        Item temp = GetItemByID(id);
        UnityEngine.Object loadedObject = null;

        if (temp != null)
            loadedObject = Resources.Load("Items/" + temp.Name, typeof(GameObject));

        if (loadedObject == null)
            Debug.LogError("loadedObject is null");

        return loadedObject;
    }
}
