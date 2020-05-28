public interface IInteractable
{
    #region Public Properties

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

    #endregion Public Properties

    #region Public Methods

    void Focus();

    void OnInteract();

    void UnFocus();

    #endregion Public Methods
}