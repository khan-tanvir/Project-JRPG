using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CycleMenu : MonoBehaviour
{
    #region Private Fields

    private Animator _anim;

    private Button _inventoryButton;

    private Button _journalButton;

    #endregion Private Fields

    #region Public Fields

    public GameObject Menu;

    #endregion Public Fields

    #region Public Properties

    public PlayerInputActions InputActions
    {
        get;
        set;
    }

    #endregion Public Properties

    #region Private Methods

    private void BindControls()
    {
        InputActions.InGameUI.Cancel.performed += ctx => MenuOnDisable();
        InputActions.InGameUI.Submit.performed += ctx => ButtonClick();
    }

    private void ButtonClick()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
    }

    private void LoadComponents()
    {
        Menu.SetActive(true);
        _anim = Menu.GetComponent<Animator>();
        InputActions = FindObjectOfType<Player>().PlayerInput;

        _inventoryButton = Menu.transform.GetChild(0).gameObject.GetComponent<Button>();
        _journalButton = Menu.transform.GetChild(1).gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        LoadComponents();
        BindControls();

        Menu.SetActive(false);
    }

    private void SwitchActionMap()
    {
        if (Menu.activeInHierarchy)
        {
            InputActions.PlayerControls.Disable();
            InputActions.InGameUI.Enable();
        }
        else
        {
            InputActions.PlayerControls.Enable();
            InputActions.InGameUI.Disable();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Menu.transform.position = new Vector2(Camera.main.WorldToScreenPoint(FindObjectOfType<Player>().transform.position).x, Camera.main.WorldToScreenPoint(FindObjectOfType<Player>().transform.position).y + 75.0f);

        if (Menu.activeInHierarchy)
        {
            if (EventSystem.current.currentSelectedGameObject != _journalButton.gameObject || EventSystem.current.currentSelectedGameObject != _inventoryButton.gameObject)
            {
                if (_inventoryButton.interactable)
                {
                    EventSystem.current.SetSelectedGameObject(_inventoryButton.gameObject);
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(_journalButton.gameObject);
                }
            }
        }
    }

    #endregion Private Methods

    #region Public Methods

    private void SwitchInteraction()
    {
        if (_inventoryButton.interactable)
        {
            _inventoryButton.interactable = false;
            _journalButton.interactable = true;
        }
        else
        {
            _inventoryButton.interactable = true;
            _journalButton.interactable = false;
        }
    }

    public void Cycle(BaseEventData data)
    {
        AxisEventData axisData = data as AxisEventData;

        if (axisData.moveDir == MoveDirection.Left || axisData.moveDir == MoveDirection.Right)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject == Menu.transform.GetChild(0).gameObject)
        {
            _anim.SetBool("Switch", false);

            SwitchInteraction();
        }
        else if (EventSystem.current.currentSelectedGameObject == Menu.transform.GetChild(1).gameObject)
        {
            _anim.SetBool("Switch", true);

            SwitchInteraction();
        }
    }

    public void MenuOnDisable()
    {
        Menu.SetActive(false);

        SwitchActionMap();
    }

    public void MenuOnEnable()
    {
        Menu.SetActive(true);

        SwitchActionMap();

        EventSystem.current.SetSelectedGameObject(Menu.transform.Find("Inventory").gameObject);

        _inventoryButton.interactable = true;
        _journalButton.interactable = false;
    }

    #endregion Public Methods
}