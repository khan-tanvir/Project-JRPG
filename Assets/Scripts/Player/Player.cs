using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private GameObject _deathMenu;

    [SerializeField]
    private GameObject _inventory;

    [SerializeField]
    private GameObject _journal;

    [SerializeField]
    private float _movementSpeed;

    private PlayerInteraction _playerInteraction;

    public bool UseGamePad
    {
        get;
        internal set;
    }

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

        //SwitchInput();

        PlayerInput.PlayerControls.Move.performed += ctx => Direction = ctx.ReadValue<UnityEngine.Vector2>();
        PlayerInput.PlayerControls.Inventory.performed += ctx => ToggleInventory();
        PlayerInput.PlayerControls.Journal.performed += ctx => ToggleJournal();
        PlayerInput.PlayerControls.Pause.performed += ctx => ToggleGame();
        PlayerInput.PlayerControls.Interact.performed += ctx => Interact();
        PlayerInput.PlayerControls.ToggleFollower.performed += ctx => ToggleFollower();

#if DEBUG
        PlayerInput.PlayerControls.KillPlayer.performed += ctx => KillPlayer();

#endif
    }

    private void Interact()
    {
        if (_playerInteraction.InteractableObject)
            _playerInteraction.CallInteract();
    }

    private void KillPlayer()
    {
        Time.timeScale = 0.0f;
        _deathMenu.SetActive(true);
        PlayerInput.Disable();
    }

    private void LoadComponents()
    {
        RigidBody = gameObject.GetComponent<Rigidbody2D>();
        Animator = gameObject.GetComponentInChildren<Animator>();
        _playerInteraction = gameObject.GetComponent<PlayerInteraction>();
    }

    private void LoadPlayerPosition()
    {
        transform.position = RespawnManager.Instance.CurrentCheckpoint;
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

    public void SwitchInput()
    {
        if (UseGamePad)
        {
            var binding = PlayerInput.controlSchemes.First(a => a.name == "Keyboard").bindingGroup;
            PlayerInput.bindingMask = InputBinding.MaskByGroup(binding);
            UseGamePad = false;
        }
        else
        {
            var binding = PlayerInput.controlSchemes.First(a => a.name == "Gamepad").bindingGroup;
            PlayerInput.bindingMask = InputBinding.MaskByGroup(binding);
            UseGamePad = true;
        }
    }

    #endregion Private Methods
}