using UnityEngine;

[System.Serializable]
public class Slot : MonoBehaviour
{
    #region Private Fields

    [HideInInspector]
    private Vector2 _initialPosition;

    #endregion Private Fields

    #region Public Properties

    public Vector2 InitialPosition
    {
        get => _initialPosition;
        set => _initialPosition = value;
    }

    public Item InvItem
    {
        get;
        internal set;
    }

    public string ObjectID
    {
        get;
        set;
    }

    #endregion Public Properties

    #region Public Methods

    public void DropItem()
    {
        if (transform.GetComponentInChildren<ItemMB>() != null)
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

            temp.GetComponent<IDroppable>().ItemDropped();

            temp.GetComponent<IDGenerator>().ObjectID = ObjectID;
            temp.GetComponent<IDGenerator>().InitialPosition = InitialPosition;

            temp.GetComponent<IDGenerator>().InInventory = false;

            Destroy(child.gameObject);

            InvItem = null;
            ObjectID = "";
        }
    }

    public void StoreItem(Item item)
    {
        InvItem = item;

        foreach (Transform child in transform)
        {
            if (child.name != "Cross")
            {
                child.name = InvItem.Name;
                child.GetComponent<IDGenerator>().ObjectID = ObjectID;
                child.GetComponent<IDGenerator>().InInventory = true;
            }
        }
    }

    #endregion Public Methods
}