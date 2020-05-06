using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    void OnTriggerEnter()
    {
        Debug.Log("Picked up" + transform.name);
        PickUp();
    }

    void PickUp()
    {
        Debug.Log("Picking Up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp)
        Destroy(gameObject);
    }
}
