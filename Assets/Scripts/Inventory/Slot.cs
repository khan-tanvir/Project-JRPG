using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Slot : MonoBehaviour
{
    public Item InvItem
    {
        get 
        {
            try
            {
                return GetComponentInChildren<ItemMB>().Item;
            }
            catch (System.NullReferenceException e)
            {
                return null;
            }
        }
    }

    public void StoreItem(Item item)
    {

        if (InvItem.ItemMB == null)
        {
            InvItem.ItemMB = item.ItemMB;
        }

        foreach (Transform child in transform)
        {
            if (child.name != "Cross")
            {
                child.name = InvItem.Name;
            }
        }
    }

    public void DropItem()
    {
        if (InvItem?.ItemMB != null)
        {
            if (transform.GetComponentInChildren<IDroppable>()?.EnableDrop != true)
            {
                return;
            }

            Vector2 playerPos = FindObjectOfType<Player>().GetComponent<Rigidbody2D>().transform.position;

            // Set the range for dropping them item and the z value has to be -1 so it can be seen from the main camera
            Vector3 dropPos = new Vector3(playerPos.x + Random.Range(-1.5f, 1.5f), playerPos.y + Random.Range(-1.5f, 1.5f), -1.0f);

            Transform child = transform.GetComponentInChildren<ItemMB>().gameObject.transform;

            // Store it in temp after creating it so that it can be referenced
            GameObject temp = Instantiate(child.gameObject, dropPos, Quaternion.identity);

            if (temp.GetComponent<ItemMB>()?.Item == null)
            {
                temp.GetComponent<ItemMB>().Item = InvItem;
            }

            temp.GetComponentInChildren<IDroppable>().ItemDropped();

            Destroy(child.gameObject);
        }
    }
}
