using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class scriptSlot : MonoBehaviour
{
    [SerializeField]
    private Item _item;

    [SerializeField]
    private bool _isFilled;

    public Item InvItem
    {
        get { return _item; }
    }

    public string ItemName
    {
        get { return _item.Name; }
    }

    public bool IsFilled
    {
        get
        {
            if (_item is null)
                return false;
            else
                return true;
        }
    }

    public void StoreItem(Item item)
    {
        _item = item;

        foreach (Transform child in transform)
        {
            if (child.name != "Cross") child.name = _item.Name;
        }
    }

    public void DisablePickUp()
    {
        GetComponentInChildren<scriptPickUp>().enabled = false;
    }

    public void DropItem()
    {
        if (_item != null)
        {
            Vector2 playerPos = FindObjectOfType<scriptPlayer>().GetComponent<Rigidbody2D>().transform.position;

            // Set the range for dropping them item and the z value has to be -1 so it can be seen from the main camera
            Vector3 dropPos = new Vector3(playerPos.x + Random.Range(-1.5f, 1.5f), playerPos.y + Random.Range(-1.5f, 1.5f), -1.0f);

            Transform child = transform.GetComponentInChildren<scriptPickUp>().gameObject.transform;

            // Store it in temp after creating it so that it can be referenced
            GameObject temp = Instantiate(child.gameObject, dropPos, Quaternion.identity);

            // Adjust the instaniated object
            temp.transform.localScale = new Vector2(1.0f, 1.0f);

            // Toggle components that are needed and not needed
            temp.gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
            temp.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            temp.gameObject.GetComponent<scriptPickUp>().enabled = true;

            GameObject.Destroy(child.gameObject);

            _item = null;

            // Calls the Gather Objective Change Event
            scriptGameEvents._gameEvents.GatherObjectiveChange(temp.GetComponent<scriptPickUp>().ItemName);
        }
    }
}
