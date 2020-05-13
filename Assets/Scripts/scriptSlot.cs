using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptSlot : MonoBehaviour
{
    // Reference to the inventory
    private scriptInventory _inventory;

    [SerializeField]
    private string _itemName;
    [SerializeField]
    private bool _isFilled;

    public string ItemName
    {
        get { return _itemName; }
        set { _itemName = value; }
    }

    public bool IsFilled
    {
        get { return _isFilled; }
        set { _isFilled = value; }
    }

    private void Awake()
    {
        _inventory = GameObject.FindObjectOfType<scriptPlayer>().GetComponent<scriptInventory>();

        // TODO: Saving and Loading inventory

        if (transform.childCount != 0)
        {
            _isFilled = true;
            _itemName = transform.GetChild(0).name;
        }
        else
            _isFilled = false;
    }

    private void Update()
    {
        //// An item in the inventory is stored as a child of the related slot so if it doesn't have a child, that means it is not filled
        //if (transform.childCount <= 0)
        //    _isFilled = false;
        //else
        //    _isFilled = true;
    }

    public void DropItem()
    {
        Vector2 playerPos = FindObjectOfType<scriptPlayer>().GetComponent<Rigidbody2D>().transform.position;
        foreach (Transform child in transform)
        {
            _isFilled = false;

            // Set the range for dropping them item and the z value has to be -1 so it can be seen from the main camera
            Vector3 dropPos = new Vector3(playerPos.x + Random.Range(-1.5f, 1.5f), playerPos.y + Random.Range(-1.5f, 1.5f), -1.0f);

            // Store it in temp after creating it so that it can be referenced
            GameObject temp = Instantiate(child.gameObject, dropPos, Quaternion.identity);

            // Adjust the instaniated object
            temp.transform.localScale = new Vector2(1.0f, 1.0f);
            temp.name = child.name;

            // Toggle components that are needed and not needed
            temp.gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
            temp.gameObject.GetComponent<SpriteRenderer>().enabled = true;

            GameObject.Destroy(child.gameObject);

            // Calls the Gather Objective Change Event
            scriptGameEvents._gameEvents.GatherObjectiveChange(temp.name);
        }

        
    }
}
