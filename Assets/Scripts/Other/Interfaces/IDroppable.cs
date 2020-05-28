public interface IDroppable
{
    #region Public Properties

    bool EnableDrop
    {
        get;
        set;
    }

    #endregion Public Properties

    #region Public Methods

    void ItemDropped();

    #endregion Public Methods
}