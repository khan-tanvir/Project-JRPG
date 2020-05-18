using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    // Max range for interactable
    float Range
    {
        get;
    }
    
    // Can player interact with object
    bool EnableInteraction
    {
        get;
    }

    bool InfiniteUses
    {
        get;
    }

    void Focus();

    void UnFocus();

    void OnInteract();
}
