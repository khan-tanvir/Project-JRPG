using UnityEngine;
using UnityEngine.UI;

public class Arrow : ItemMB, IStoreable, IDroppable
{
    [HideInInspector]
    private Item _item;
    
    [SerializeField]
    private bool _enableInteraction;

    [SerializeField]
    private bool _infiniteUses;

    [SerializeField]
    private bool _enablePickUp;

    [SerializeField]
    private bool _enableDrop;
    
    public override Item Item
    {
        get { return _item; }
        set { _item = value; }
    }

    public override bool EnabledInteraction
    {
        get { return _enableInteraction; }
        set { _enableInteraction = value; }
    }

    public override bool InfiniteUses
    {
        get { return _infiniteUses; }
        set { _infiniteUses = value; }
    }

    public bool EnablePickUp 
    {
        get { return _enablePickUp; }
        set { _enablePickUp = value; }
    }

    public bool IsItemStored
    {
        get;
        set;
    }

    public bool EnableDrop
    {
        get { return _enableDrop; }
        set { _enableDrop = value; }
    }

    public void AddItemToInventory()
    {
        IsItemStored = Inventory.Instance.FillSlot(gameObject);

        if (IsItemStored)
        {
            
            _canInteract = false;
            Destroy(gameObject);
        }
        else
        {
            // TODO: If Inventory is full
        }
    }

    public void ItemDropped()
    {
        if (!IsItemStored)
        { 
            //transform.localScale = new Vector2(1.0f, 1.0f);

            ImageComp.enabled = false;
            SpriteRendererComp.enabled = true;

            CallItemChangeEvent();
            _canInteract = true;
        }
    }

    public override void OnInteract()
    {
        if (_enablePickUp)
        {
            AddItemToInventory();
        }

        base.OnInteract();
    }
}
