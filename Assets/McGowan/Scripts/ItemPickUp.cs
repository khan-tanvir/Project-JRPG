using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    void OnTriggerEnter() // this will check when something enters a collision box that this script it attacted too
    {
        PickUp();
    }

    void PickUp()
    {
        Debug.Log("Picking Up " + item.name); // this will display the item name that was set in item
        bool wasPickedUp = Inventory.instance.Add(item); // this will allow or not allow the player to pick up item

        if (wasPickedUp)
        Destroy(gameObject); // if the previous statement is true it will allow the object to be destoryed
    }
}
