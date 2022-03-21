using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    public Vector3 lookDirection;

    #region GIZMOS
    public bool shouldDrawDebugLine;
    private void OnDrawGizmos()
    {
        if(shouldDrawDebugLine)
            Debug.DrawLine(transform.position, transform.position + lookDirection);
    }
    #endregion
}
