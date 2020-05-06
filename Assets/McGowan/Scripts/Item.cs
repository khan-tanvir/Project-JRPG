
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")] // this will allow you to add item in the assets menu
public class Item : ScriptableObject
{
    new public string name = "New Item"; // This will allow you to give it a name
    public Sprite Icon = null; // give it an icon 
    public bool isDefaultItme = false;
}
