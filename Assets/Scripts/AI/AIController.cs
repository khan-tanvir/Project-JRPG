using UnityEngine;

public enum State
{
    PATROL,

    FOLLOW,

    IDLE
}

public class AIController : MonoBehaviour
{
    #region Private Fields

    private Animator _animator;

    private bool _moveAI;

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private string _npcName;

    private Rigidbody2D _rigidBody;

    [SerializeField]
    private State _state;

    [SerializeField]
    private Transform _target;

    private Vector2 direction;

    private float waitTime;

    #endregion Private Fields

    #region Public Fields

    [Header("Area of movement")]
    public float maxX;

    public float maxY;

    public float minX;

    public float minY;

    public Transform moveSpot;

    [Header("Time adjustment")]
    public float startWaitTime;

    #endregion Public Fields

    #region Public Properties

    public EscortObjective EscortObjective
    {
        get;
        set;
    }

    public string NPCName
    {
        get => _npcName;
    }

    public float Speed
    {
        get => _movementSpeed;
        set => _movementSpeed = value;
    }

    public State State
    {
        get => _state;
        set => _state = value;
    }

    public Transform Target
    {
        get => _target;
        set => _target = value;
    }

    #endregion Public Properties

    #region Private Methods

    private RaycastHit2D CheckRaycast(Vector2 direction)
    {
        Vector2 startingPositionX = new Vector2(_rigidBody.transform.position.x, _rigidBody.transform.position.y);

        LayerMask mask = LayerMask.GetMask("Building");

        Debug.DrawRay(startingPositionX, direction, Color.white);

        return Physics2D.CircleCast(startingPositionX, 1.0f, direction, 0.0f, mask);
    }

    private bool CheckRaycastUpdate()
    {
        Vector2 direction = new Vector2(0, 0);
        RaycastHit2D hit = CheckRaycast(direction);

        if (hit.collider)
        {
            Debug.Log("Hit " + hit.collider.name);
            moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {
        switch (State)
        {
            case State.PATROL:
                Patrol();
                break;

            case State.FOLLOW:
                Follow();
                break;

            case State.IDLE:
                Idle();
                break;
        }
    }

    private void Follow()
    {
        if (Vector2.Distance(_rigidBody.transform.position, Target.position) > 1.5f)
        {
            //_rigidBody.transform.position = Vector3.MoveTowards(_rigidBody.transform.position, Target.position, Speed * Time.deltaTime);
            _rigidBody.MovePosition(_rigidBody.position + (Time.fixedDeltaTime * Speed * direction));
            _moveAI = true;
        }
        else
        {
            _moveAI = false;
        }
    }

    private void Idle()
    {
        waitTime -= Time.deltaTime;
    }

    private void Patrol()
    {
        CheckRaycastUpdate();

        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, Speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                startWaitTime = Random.Range(1f, 6f);
                waitTime = startWaitTime;
                moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void SetupComponents()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        waitTime = startWaitTime;

        SetupComponents();
    }

    private void Update()
    {
        // Clean this up
        if (Target == null)
            return;

        direction = Target.position - _rigidBody.transform.position;
        direction.Normalize();

        _animator.SetFloat("Horizontal", direction.x);
        _animator.SetFloat("Vertical", direction.y);

        if (_moveAI && State != State.IDLE)
        {
            _animator.SetFloat("Speed", direction.sqrMagnitude);
        }
        else
        {
            ResetSpeed();
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void ResetSpeed()
    {
        _animator.SetFloat("Speed", 0.0f);
    }

    public void ToggleFollower()
    {
        if (_state == State.IDLE)
        {
            _state = State.FOLLOW;
            EscortObjective.IsFollowing = true;
        }
        else if (_state == State.FOLLOW)
        {
            _state = State.IDLE;
            EscortObjective.IsFollowing = false;
        }
    }

    #endregion Public Methods
}