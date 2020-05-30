using UnityEngine;

public class Player : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private GameObject _inventory;

    [SerializeField]
    private GameObject _journal;

    [SerializeField]
    private float _movementSpeed;

    private PlayerInteraction _playerInteraction;

    #endregion Private Fields

    #region Public Properties

    public Animator Animator
    {
        get;
        internal set;
    }

    public UnityEngine.Vector2 Direction
    {
        get;
        internal set;
    }

    public UnityEngine.Vector2 FacingDirection
    {
        get;
        internal set;
    }

    public float MovementSpeed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = value; }
    }

    public PlayerInputActions PlayerInput
    {
        get;
        internal set;
    }

    public Rigidbody2D RigidBody
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Private Methods

    private void Awake()
    {
        LoadComponents();
        InitialiseInput();
    }

    private void FixedUpdate()
    {
        RigidBody.MovePosition(RigidBody.position + (Time.fixedDeltaTime * MovementSpeed * Direction));

        _playerInteraction.PlayerPosition = transform.position;
        _playerInteraction.Raycast();
    }

    private void InitialiseInput()
    {
        PlayerInput = new PlayerInputActions();
        PlayerInput.PlayerControls.Move.performed += ctx => Direction = ctx.ReadValue<UnityEngine.Vector2>();
        PlayerInput.PlayerControls.Inventory.performed += ctx => ToggleInventory();
        PlayerInput.PlayerControls.Journal.performed += ctx => ToggleJournal();
        PlayerInput.PlayerControls.Pause.performed += ctx => ToggleGame();
        PlayerInput.PlayerControls.Interact.performed += ctx => Interact();
        PlayerInput.PlayerControls.ToggleFollower.performed += ctx => ToggleFollower();
    }

    private void Interact()
    {
        if (_playerInteraction.InteractableObject)
            _playerInteraction.CallInteract();
    }

    private void LoadComponents()
    {
        RigidBody = gameObject.GetComponent<Rigidbody2D>();
        Animator = gameObject.GetComponentInChildren<Animator>();
        _playerInteraction = gameObject.GetComponent<PlayerInteraction>();
    }

    private void LoadPlayerPosition()
    {
        UnityEngine.Vector3 loadPos = new UnityEngine.Vector3(GameData.Instance.PlayerData.PlayerPosition[0], GameData.Instance.PlayerData.PlayerPosition[1], -1);

        transform.position = loadPos;
    }

    private void OnDisable()
    {
        if (PlayerInput != null)
            PlayerInput.Disable();
    }

    private void OnEnable()
    {
        if (PlayerInput != null)
            PlayerInput.Enable();
    }

    private void Start()
    {
        LoadPlayerPosition();

        if (MovementSpeed == 0)
        {
            Debug.LogError("Player Movement Speed is 0\nSet the speed in the inspector.");
        }
    }

    private void ToggleFollower()
    {
        EventsManager.Instance.ToggleFollower();
    }

    private void ToggleGame()
    {
        GameObject.Find("Canvas").GetComponent<PauseMenu>().Toggle();
    }

    private void ToggleInventory()
    {
        if (Time.timeScale != 0.0f && !_journal.activeInHierarchy)
        {
            _inventory.SetActive(!_inventory.activeInHierarchy);
        }
    }

    private void ToggleJournal()
    {
        if (Time.timeScale != 0.0f && !_inventory.activeInHierarchy)
        {
            _journal.SetActive(!_journal.activeInHierarchy);
        }
    }

    private void Update()
    {
        if (Direction != Vector2.zero)
        {
            FacingDirection = Direction;
        }

        Animator.SetFloat("Horizontal", FacingDirection.x);
        Animator.SetFloat("Vertical", FacingDirection.y);
        Animator.SetFloat("Speed", Direction.sqrMagnitude);
    }

    #endregion Private Methods
}