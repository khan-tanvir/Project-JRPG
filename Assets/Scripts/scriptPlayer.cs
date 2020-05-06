using UnityEngine;

public class scriptPlayer : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;

    public float movementSpeed = 5.0f;

    public Animator animator;

    private Vector2 direction;

    private void Start()
    {
        // Blank because not needed
    }

    // Update is called once per frame
    private void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);

        string h = FindObjectOfType<scriptGameData>().PlayerName;
        Debug.Log("Your name is " + FindObjectOfType<scriptGameData>().PlayerName);
        
    }

    // Separate physics from update func
    private void FixedUpdate()
    {
        rigidBody2D.MovePosition(rigidBody2D.position + (Time.fixedDeltaTime * movementSpeed * direction));
    }
}