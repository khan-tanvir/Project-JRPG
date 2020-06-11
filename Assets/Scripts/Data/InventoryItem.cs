[System.Serializable]
public class InventoryItem
{
    #region Public Constructors

    public InventoryItem(int itemID, int pos, string objectID)
    {
        ID = itemID;
        Position = pos;
        ObjectID = objectID;
    }

    #endregion Public Constructors

    #region Public Properties

    public int ID
    {
        get;
        internal set;
    }

    public string ObjectID
    {
        get;
        internal set;
    }

    public int Position
    {
        get;
        internal set;
    }

    #endregion Public Properties
}