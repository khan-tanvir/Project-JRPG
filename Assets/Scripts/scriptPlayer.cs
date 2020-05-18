using System;
using System.Data;
using System.Numerics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class scriptPlayer : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;

    public float movementSpeed = 5.0f;

    public Animator animator;

    private UnityEngine.Vector2 direction;

    private PlayerInputActions _inputAction;

    private bool _isGamePaused;

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
        get { return _inputAction; }
    }

    private void Awake()
    {
        _inputAction = new PlayerInputActions();
        _inputAction.PlayerControls.Move.performed += ctx => direction = ctx.ReadValue<UnityEngine.Vector2>();
        _inputAction.PlayerControls.Inventory.performed += ctx => ToggleInventory();
        _inputAction.PlayerControls.Journal.performed += ctx => ToggleJournal();
        _inputAction.PlayerControls.Pause.performed += _ => ToggleGame();

        _isGamePaused = false;
    }

    private void Start()
    {
        try
        {
            if (scriptGameData.GameDataManager.PlayerPosition[0] != 999.0f && scriptGameData.GameDataManager.PlayerPosition[1] != 999.0f)
            {
                UnityEngine.Vector3 loadPos = new UnityEngine.Vector3(scriptGameData.GameDataManager.PlayerPosition[0], scriptGameData.GameDataManager.PlayerPosition[1], -1);

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
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rigidBody2D.MovePosition(rigidBody2D.position + (Time.fixedDeltaTime * movementSpeed * direction));
    }

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
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
        GameObject.Find("Canvas").GetComponent<scriptPauseMenu>().Toggle();
    }
}