using UnityEngine;

public enum State
{
    PATROL,
    FOLLOW,
    IDLE
}

public class AIController : MonoBehaviour
{
    private Animator _animator;

    private Rigidbody2D _rigidBody;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private State _state;

    private bool _moveAI;

    [SerializeField]
    private string _npcName;

    private Vector2 direction;

    [SerializeField]
    private float _movementSpeed;

    public Transform moveSpot;

    public string NPCName
    {
        get { return _npcName; }
    }

    public Transform Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public State State
    {
        get { return _state; }
        set { _state = value; }
    }

    public float Speed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = value; }
    }

    [Header("Area of movement")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Time adjustment")]
    public float startWaitTime;
    private float waitTime;

    private void Start()
    {
        waitTime = startWaitTime;

        SetupComponents();
    }

    private void SetupComponents()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        direction = Target.position - _rigidBody.transform.position;
        direction.Normalize();

        _animator.SetFloat("Horizontal", direction.x);
        _animator.SetFloat("Vertical", direction.y);

        if (_moveAI)
        {
            _animator.SetFloat("Speed", direction.sqrMagnitude);
        }
        else
        {
            _animator.SetFloat("Speed", 0.0f);
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

    private void Patrol()
    {

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

    private RaycastHit2D CheckRaycast(Vector2 direction)
    {
        Vector2 startingPositionX = new Vector2(_rigidBody.transform.position.x, _rigidBody.transform.position.y);

        LayerMask mask = LayerMask.GetMask("Building");

        Debug.DrawRay(startingPositionX, direction, Color.white);

        return Physics2D.CircleCast(startingPositionX, 1.0f, direction, 0.0f, mask);
    }

    bool CheckRaycastUpdate()
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
}
