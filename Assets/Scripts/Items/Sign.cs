using UnityEngine;

public class Sign : ItemMB
{
    #region Private Fields

    [SerializeField]
    private bool _enableInteraction;

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

    public override bool InfiniteUses
    {
        get { return _infiniteUses; }
        set { _infiniteUses = value; }
    }

    public override Item Item
    {
        get { return _item; }
        set { _item = value; }
    }

    #endregion Public Properties
}