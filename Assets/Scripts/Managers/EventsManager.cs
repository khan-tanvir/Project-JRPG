using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    #region Public Events

    public event Action<int> OnCheckPointCall;

    public event Action<string> OnCutsceneFinish;

    public event Action<string> OnGatherObjectiveChange;

    public event Action<string> OnInteractionWithItem;

    public event Action<string> OnLocationEntered;

    public event Action OnPlayerDeath;

    public event Action OnRespawn;

    public event Action OnSceneChange;

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
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void CheckPointCall(int point)
    {
        OnCheckPointCall?.Invoke(point);
    }

    public void CutsceneFinish(string cutscene)
    {
        OnCutsceneFinish?.Invoke(cutscene);
    }

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

    public void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public void Respawn()
    {
        OnRespawn?.Invoke();
    }

    public void SceneChange()
    {
        OnSceneChange?.Invoke();
    }

    public void ToggleFollower()
    {
        OnToggleFollower?.Invoke();
    }

    #endregion Public Methods
}