public class Item
{
    public int ID
    {
        get;
        internal set;
    }

    public string Name
    {
        get;
        internal set;
    }

    public string Description
    {
        get;
        internal set;
    }

    public Item(int id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
    }
}
