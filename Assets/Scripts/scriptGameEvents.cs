using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptGameEvents : MonoBehaviour
{
    [SerializeField]
    public static scriptGameEvents _gameEvents;
    
    // Start is called before the first frame update
    void Awake()
    {
        _gameEvents = this;
    }

    public event Action<string> onGatherObjectiveChange;
    public event Action<string> onInteraction;
    public event Action<string> onLocationEntered;

    public void GatherObjectiveChange(string item)
    {
        if (onGatherObjectiveChange != null)
            onGatherObjectiveChange(item);
    }

    public void Interaction(string interactable)
    {
        if (onInteraction != null)
            onInteraction(interactable);
    }

    public void LocationEntered(string location)
    {
        if (onLocationEntered != null)
            onLocationEntered(location);
    }
}
