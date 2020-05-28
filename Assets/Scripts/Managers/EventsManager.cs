using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    #region Public Events

    public event Action<string> OnGatherObjectiveChange;

    public event Action<string> OnInteractionWithItem;

    public event Action<string> OnLocationEntered;

    public event Action OnToggleFollower;

    #endregion Public Events

    #region Public Properties

    public static EventsManager Instance
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Private Methods

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    #endregion Private Methods

    #region Public Methods

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

    public void ToggleFollower()
    {
        OnToggleFollower?.Invoke();
    }

    #endregion Public Methods
}