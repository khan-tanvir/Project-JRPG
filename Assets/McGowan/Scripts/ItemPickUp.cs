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
        // add item to inventory
        Destroy(gameObject);
    }
}
