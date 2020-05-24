using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemMB : MonoBehaviour
{
    // Every item in the game world will have this class
    [SerializeField]
    private int _itemID;

    public Image ImageComp
    {
        get;
        internal set;
    }

   public SpriteRenderer SpriteRendererComp
    {
        get;
        internal set;
    }

    public Item Item
    {
        get;
        internal set;
    }

    public Sprite Sprite
    {
        get;
        internal set;
    }

    public void Start()
    {
        Item = new Item(ItemDatabase.Instance.GetItemByID(_itemID));
        Item.ItemMB = this;

        SetupComponents();
    }

    private void SetupComponents()
    {
        ImageComp = gameObject.AddComponent(typeof(Image)) as Image;
        SpriteRendererComp = gameObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;

        _ = ImageComp.sprite;
        _ = SpriteRendererComp.sprite;
        
    }
}
