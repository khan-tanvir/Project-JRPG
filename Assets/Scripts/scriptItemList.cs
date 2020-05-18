using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class scriptItemList : MonoBehaviour
{
    // All items to store in inventory will be here

    public List<Item> _items = new List<Item>();

    public static scriptItemList ItemDatabase { get; set; }

    private void Awake()
    {
        ItemDatabase = this;

        CreateDatabase();
    }

    public void CreateDatabase()
    {
        // All items need to be hardcoded
        _items = new List<Item>()
        {
            new Item(1, "Test", "Test")
        };
    }

    public Item GetItemByID(int id)
    {
        return _items.Find(item => item.ID == id);
    }

    public Item GetItemByName(string name)
    {
        return _items.Find(item => item.Name == name);
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
