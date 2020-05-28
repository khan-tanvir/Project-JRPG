[System.Serializable]
public class Item
{
    #region Private Fields

    [UnityEngine.HideInInspector]
    private ItemMB _itemMB;

    #endregion Private Fields

    #region Public Constructors

    public Item(int id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
    }

    public Item(Item copyItem)
    {
        this.ID = copyItem.ID;
        this.Name = copyItem.Name;
        this.Description = copyItem.Description;
    }

    #endregion Public Constructors

    #region Public Properties

    public string Description
    {
        get;
        internal set;
    }

    public int ID
    {
        get;
        internal set;
    }

    public ItemMB ItemMB
    {
        get { return _itemMB; }
        set { _itemMB = value; }
    }

    public string Name
    {
        get;
        internal set;
    }

    #endregion Public Properties
}