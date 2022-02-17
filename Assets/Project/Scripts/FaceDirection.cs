using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    public Vector3 lookDirection;
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + lookDirection);
    }
}
