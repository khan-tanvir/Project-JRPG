using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIcontroller : MonoBehaviour
{
    private enum State // This allows the AI to change between states
    {
        Patrol,
        Follow,
        Idle
    }

    private Animator animator;
    public Transform moveSpot;
    private Transform playerPosition;

    private State state;

    [SerializeField]
    private float Speed;

    [Header("Area of movement")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Time adjustment")]
    public float startWaitTime;
    private float waitTime;

    private void Awake()
    {
        state = State.Idle; // This is what happens when the game is launched
    }

    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        animator = GetComponent<Animator>();
        playerPosition = FindObjectOfType<PlayerMovement>().transform;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.Idle:
                waitTime -= Time.deltaTime;

                animator.SetBool("isFollow", false);
                animator.SetBool("isPatrol", false);

                if (waitTime <= 0)
                {
                    state = State.Patrol;
                }
                break;

            case State.Patrol:
                patrol();
                break;

            case State.Follow:
                animator.SetBool("isPatrol", false);
                followPlayer();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = State.Idle;
                }
                    break;
        }
    }

    public void followPlayer()
    {
            animator.SetBool("isFollow", true);
            animator.SetFloat("MoveX", (playerPosition.position.x - transform.position.x));
            animator.SetFloat("MoveY", (playerPosition.position.y - transform.position.y));

            if (Vector2.Distance(transform.position, playerPosition.position) > 2.0f)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPosition.transform.position, Speed * Time.deltaTime);
            }
    }


    public void patrol()
    {
        CheckRaycastUpdate();
        float targetRange = 2.0f;

        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, Speed * Time.deltaTime);

        animator.SetBool("isPatrol", true);
        animator.SetFloat("MoveX", (moveSpot.position.x - transform.position.x));
        animator.SetFloat("MoveY", (moveSpot.position.y - transform.position.y));

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
                animator.SetBool("isPatrol", false);
                waitTime -= Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Vector2.Distance(transform.position, playerPosition.position) < targetRange)
            {
                state = State.Follow;
            }
        }
    }
    

    private RaycastHit2D Checkraycast(Vector2 direction)
    {
        Vector2 startingPositionX = new Vector2(transform.position.x, transform.position.y);

        LayerMask mask = LayerMask.GetMask("Building");

        Debug.DrawRay(startingPositionX, direction, Color.white);

        return Physics2D.CircleCast(startingPositionX, 1.0f, direction, 0.0f, mask);
    }

    bool CheckRaycastUpdate()
    {
        Vector2 direction = new Vector2(0, 0);
        RaycastHit2D hit = Checkraycast(direction);

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

