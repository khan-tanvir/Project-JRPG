using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class ItemMB : MonoBehaviour, IInteractable
{
    #region Protected Fields

    protected bool _canInteract;

    [SerializeField]
    protected int _itemID;

    [SerializeField]
    protected Material interactionMat;

    #endregion Protected Fields

    #region Public Fields

    [HideInInspector]
    public Material defaultMat;

    #endregion Public Fields

    #region Public Properties

    public abstract bool EnabledInteraction
    {
        get;
        set;
    }

    public Image ImageComp
    {
        get => gameObject.GetComponent<Image>();
    }

    public abstract bool InfiniteUses
    {
        get;
        set;
    }

    public abstract Item Item
    {
        get;
        set;
    }

    public SpriteRenderer SpriteRendererComp
    {
        get => gameObject.GetComponent<SpriteRenderer>();
    }

    #endregion Public Properties

    #region Public Methods

    public void CallItemChangeEvent()
    {
        // Calls the Gather Objective Change Event
        EventsManager.Instance.GatherObjectiveChange(Item.Name);
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

    public virtual void Start()
    {
        Item = new Item(ItemDatabase.Instance.GetItemByID(_itemID))
        {
            ItemMB = this
        };

        defaultMat = gameObject.GetComponent<Renderer>().material;
        _canInteract = true;
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

    #endregion Public Methods
}