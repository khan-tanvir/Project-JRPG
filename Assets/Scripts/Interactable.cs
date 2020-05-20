using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    // Max range for interactable
    //float Range
    //{
    //    get;
    //}
    
    // Can player interact with object
    bool EnabledInteraction
    {
        get;
        set;
    }

    // Is it a one use object?
    bool InfiniteUses
    {
        get;
    }

    void Focus();

    void UnFocus();

    void OnInteract();
}
