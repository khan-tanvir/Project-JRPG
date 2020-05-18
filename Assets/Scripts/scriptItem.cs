using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private int _id;
    private string _name;
    private string _description;

    public int ID
    {
        get { return _id; }
    }

    public string Name
    {
        get { return _name; }
    }

    public string Description
    {
        get { return _description; }
    }

    public Item(int id, string name, string description)
    {
        _id = id;
        _name = name;
        _description = description;
    }
}
