using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Slot : MonoBehaviour
{
    public Item InvItem
    {
        get;
        internal set;
    }

    public void StoreItem(Item item)
    {
        InvItem = item;

        foreach (Transform child in transform)
        {
            if (child.name != "Cross") child.name = InvItem.Name;
        }
    }

    public void DisablePickUp()
    {
        GetComponentInChildren<PickUp>().enabled = false;
    }

    public void DropItem()
    {
        if (InvItem != null)
        {
            Vector2 playerPos = FindObjectOfType<Player>().GetComponent<Rigidbody2D>().transform.position;

            // Set the range for dropping them item and the z value has to be -1 so it can be seen from the main camera
            Vector3 dropPos = new Vector3(playerPos.x + Random.Range(-1.5f, 1.5f), playerPos.y + Random.Range(-1.5f, 1.5f), -1.0f);

            Transform child = transform.GetComponentInChildren<PickUp>().gameObject.transform;

            // Store it in temp after creating it so that it can be referenced
            GameObject temp = Instantiate(child.gameObject, dropPos, Quaternion.identity);

            // Adjust the instaniated object
            temp.transform.localScale = new Vector2(1.0f, 1.0f);

            // Toggle components that are needed and not needed
            temp.gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
            temp.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            temp.gameObject.GetComponent<PickUp>().enabled = true;

            GameObject.Destroy(child.gameObject);

            InvItem = null;

            // Calls the Gather Objective Change Event
            EventsManager.Instance.GatherObjectiveChange(temp.GetComponent<PickUp>().ItemName);
        }
    }
}
