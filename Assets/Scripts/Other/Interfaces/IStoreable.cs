using UnityEngine;

public interface IStoreable
{
    #region Public Properties

    bool EnablePickUp
    {
        get;
        set;
    }

    UnityEngine.UI.Image ImageComp
    {
        get;
    }

    bool IsItemStored
    {
        get;
        set;
    }

    SpriteRenderer SpriteRendererComp
    {
        get;
    }

    #endregion Public Properties

    #region Public Methods

    void AddItemToInventory();

    #endregion Public Methods
}