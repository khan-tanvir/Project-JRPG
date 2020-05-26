using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class ItemMB : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected Material interactionMat;

    [HideInInspector]
    public Material defaultMat;

    [SerializeField]
    protected int _itemID;

    protected bool _canInteract;

    public abstract Item Item
    {
        get;
        set;
    }
    public abstract bool EnabledInteraction
    {
        get;
        set;
    }

    public abstract bool InfiniteUses
    {
        get;
        set;
    }
    public Image ImageComp
    {
        get { return gameObject.GetComponent<Image>(); }
    }

    public SpriteRenderer SpriteRendererComp
    {
        get { return gameObject.GetComponent<SpriteRenderer>(); }
    }

    public virtual void Start()
    {
        Item = new Item(ItemDatabase.Instance.GetItemByID(_itemID));
        Item.ItemMB = this;
        
        defaultMat = gameObject.GetComponent<Renderer>().material;

        _canInteract = true;

    }

    public virtual void Focus()
    {
        if (!_canInteract)
        {
            return;
        }

        if (EnabledInteraction)
        {
            gameObject.GetComponent<Renderer>().material = interactionMat;
        }
    }

    public virtual void OnInteract()
    {
        if (!_canInteract)
        {
            return;
        }

        if (EnabledInteraction)
        {
            EventsManager.Instance.InteractionWithItem(Item.Name);
        }
    }

    public virtual void UnFocus()
    {
        if (!_canInteract)
        {
            return;
        }

        if (EnabledInteraction)
        {
            gameObject.GetComponent<Renderer>().material = defaultMat;
        }
    }

    public void CallItemChangeEvent()
    {
        // Calls the Gather Objective Change Event
        EventsManager.Instance.GatherObjectiveChange(Item.Name);
    }
}