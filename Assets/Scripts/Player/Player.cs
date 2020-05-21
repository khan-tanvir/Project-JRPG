using System;
using System.Data;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed;

    private PlayerInteraction _playerInteraction;
    
    public Rigidbody2D RigidBody
    {
        get;
        internal set;
    }

    public float MovementSpeed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = value; }
    }

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

    public bool InventoryOpened
    {
        get;
        internal set;
    }

    public bool JournalOpened
    {
        get;
        internal set;
    }

    public PlayerInputActions PlayerInput
    {
        get;
        internal set;
    }

    private void Awake()
    {
        LoadComponents();
        InitialiseInput();
    }

    private void Start()
    {
        LoadPlayerPosition();

        if (MovementSpeed == 0)
        {
            Debug.LogError("Player Movement Speed is 0\nSet the speed in the inspector.");
        }
    }

    private void LoadComponents()
    {
        RigidBody = gameObject.GetComponent<Rigidbody2D>();
        Animator = gameObject.GetComponent<Animator>();
        _playerInteraction = gameObject.GetComponent<PlayerInteraction>();
    }

    private void InitialiseInput()
    {
        PlayerInput = new PlayerInputActions();
        PlayerInput.PlayerControls.Move.performed += ctx => Direction = ctx.ReadValue<UnityEngine.Vector2>();
        PlayerInput.PlayerControls.Inventory.performed += ctx => ToggleInventory();
        PlayerInput.PlayerControls.Journal.performed += ctx => ToggleJournal();
        PlayerInput.PlayerControls.Pause.performed += ctx => ToggleGame();
        PlayerInput.PlayerControls.Interact.performed += ctx => Interact();
    }

    private void LoadPlayerPosition()
    {
        try
        {
            if (GameData.Instance.PlayerData.PlayerPosition[0] >= 800.0f && GameData.Instance.PlayerData.PlayerPosition[1] >= 800.0f)
            {
                UnityEngine.Vector3 loadPos = new UnityEngine.Vector3(GameData.Instance.PlayerData.PlayerPosition[0], GameData.Instance.PlayerData.PlayerPosition[1], -1);

                transform.position = loadPos;
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Load from the menu scene to avoid errors.\n" + e.Message);
        }
    }

    private void Update()
    {
        Animator.SetFloat("Horizontal", Direction.x);
        Animator.SetFloat("Vertical", Direction.y);
        Animator.SetFloat("Speed", Direction.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        RigidBody.MovePosition(RigidBody.position + (Time.fixedDeltaTime * MovementSpeed * Direction));

        _playerInteraction.PlayerPosition = transform.position;
        _playerInteraction.Raycast();
    }

    private void OnEnable()
    {
        if (PlayerInput != null)
            PlayerInput.Enable();
    }

    private void OnDisable()
    {
        if (PlayerInput != null)
            PlayerInput.Disable();
    }

    private void ToggleInventory()
    {
        if (!JournalOpened && Time.timeScale != 0.0f)
        {
            InventoryOpened = GameObject.Find("Canvas").transform.Find("Inventory").GetComponent<ToggleGameObject>().ToggleObject();
        }
    }

    private void ToggleJournal()
    {
        if (!InventoryOpened && Time.timeScale != 0.0f)
        {
            JournalOpened = GameObject.Find("Canvas").transform.Find("Journal").GetComponent<ToggleGameObject>().ToggleObject();
        }
    }

    private void ToggleGame()
    {
        GameObject.Find("Canvas").GetComponent<PauseMenu>().Toggle();
    }

    private void Interact()
    {
        if (_playerInteraction.InteractableObject)
            _playerInteraction.CallInteract();
    }
}