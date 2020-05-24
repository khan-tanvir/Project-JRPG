using UnityEngine;

public interface IStoreable
{
    bool CanPickUp
    {
        get;
        set;
    }

    void AddItemToInventory();
}
