using UnityEngine;
using UnityEngine.UI;

public interface IStoreable
{
    bool EnablePickUp
    {
        get;
        set;
    }

    bool IsItemStored
    {
        get;
        set;
    }

    UnityEngine.UI.Image ImageComp
    {
        get;
    }

    SpriteRenderer SpriteRendererComp
    {
        get;
    }

    void AddItemToInventory();
}
