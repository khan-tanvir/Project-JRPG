using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : ItemMB, IStoreable
{
    #region Private Fields

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
        get => _enableInteraction;
        set => _enableInteraction = value;
    }

    public bool EnablePickUp
    {
        get => _enablePickUp;
        set => _enablePickUp = value;
    }

    public override bool InfiniteUses
    {
        get => _infiniteUses;
        set => _infiniteUses = value;
    }

    public bool IsItemStored
    {
        get;
        set;
    }

    public override Item Item
    {
        get => _item;
        set => _item = value;
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