using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class scriptPlayer : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;

    public float movementSpeed = 5.0f;

    public Animator animator;

    Vector2 position;

    void Start()
    {
        // Blank because not needed
    }

    // Update is called once per frame
    void Update()
    {
        position.x = Input.GetAxisRaw("Horizontal");
        position.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", position.x);
        animator.SetFloat("Vertical", position.y);
        animator.SetFloat("Speed", position.sqrMagnitude);
    }

    // Separate physics from update func
    void FixedUpdate()
    {
        
        rigidBody2D.MovePosition(rigidBody2D.position + (Time.fixedDeltaTime * movementSpeed * position));
    }
}
