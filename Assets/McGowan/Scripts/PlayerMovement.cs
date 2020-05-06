using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Update is called once per frame
    public float moveSpeed = 10.0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate((Vector3.forward * moveSpeed) * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate((-Vector3.forward * moveSpeed) * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate((Vector3.right * moveSpeed) * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate((-Vector3.right * moveSpeed) * Time.deltaTime);
        }
    }
}
