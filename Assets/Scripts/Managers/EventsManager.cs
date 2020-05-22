using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance
    {
        get;
        internal set;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public event Action<string> OnGatherObjectiveChange;
    public event Action<string> OnInteractionWithItem;
    public event Action<string> OnLocationEntered;

    public void GatherObjectiveChange(string item)
    {
        OnGatherObjectiveChange?.Invoke(item);
    }

    public void InteractionWithItem(string interactable)
    {
        OnInteractionWithItem?.Invoke(interactable);
    }

    public void LocationEntered(string location)
    {
        OnLocationEntered?.Invoke(location);
    }
}
