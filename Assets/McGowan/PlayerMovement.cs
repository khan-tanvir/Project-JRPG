using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Update is called once per frame
    public float moveSpeed = 5.0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate((Vector2.up * moveSpeed) * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate((-Vector2.up * moveSpeed) * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate((Vector2.right * moveSpeed) * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate((Vector2.left * moveSpeed) * Time.deltaTime);
        }
    }
}
