using UnityEngine;

public class Arrow : ItemMB, IStoreable, IDroppable
{
    #region Private Fields

    [SerializeField]
    private bool _enableDrop;

    [SerializeField]
    private bool _enableInteraction;

    [SerializeField]
    private bool _enablePickUp;

    [SerializeField]
    private bool _infiniteUses;

    [HideInInspector]
    private Item _item;

    #endregion Private Fields

    #region Public Properties

    public override bool EnabledInteraction
    {
        get { return _enableInteraction; }
        set { _enableInteraction = value; }
    }

    public bool EnableDrop
    {
        get { return _enableDrop; }
        set { _enableDrop = value; }
    }

    public bool EnablePickUp
    {
        get { return _enablePickUp; }
        set { _enablePickUp = value; }
    }

    public override bool InfiniteUses
    {
        get { return _infiniteUses; }
        set { _infiniteUses = value; }
    }

    public bool IsItemStored
    {
        get;
        set;
    }

    public override Item Item
    {
        get { return _item; }
        set { _item = value; }
    }

    #endregion Public Properties

    #region Public Methods

    public void AddItemToInventory()
    {
        IsItemStored = Inventory.Instance.FillSlot(gameObject);

        if (IsItemStored)
        {
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
        if (IsItemStored)
        {
            _canInteract = false;
        }
    }

    #endregion Public Methods
}