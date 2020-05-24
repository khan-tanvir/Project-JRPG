﻿using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{   
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