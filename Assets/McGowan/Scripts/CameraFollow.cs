using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    public float SmoothSpeed = 0.125f;
    public Vector3 Offset;

     void LateUpdate()
    {
        transform.position = Target.position + Offset;
    }
}
