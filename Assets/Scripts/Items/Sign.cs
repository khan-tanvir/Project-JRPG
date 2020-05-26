using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : ItemMB
{
    [HideInInspector]
    private Item _item;

    [SerializeField]
    private bool _enableInteraction;

    [SerializeField]
    private bool _infiniteUses;

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
}
