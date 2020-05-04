using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class scriptPlayer : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;

    public float movementSpeed = 5.0f;

    public Animator animator;

    Vector2 direction;

    void Start()
    {
        // Blank because not needed
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
    }

    // Separate physics from update func
    void FixedUpdate()
    {
        rigidBody2D.MovePosition(rigidBody2D.position + (Time.fixedDeltaTime * movementSpeed * direction));
    }
}
