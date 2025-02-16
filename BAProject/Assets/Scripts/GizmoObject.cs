using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoObject : MonoBehaviour
{
    public string gizmoName;

    public Vector3 cameraRotation;

    public Boolean visible;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1);
    }

    public string getGizmoName()
    {
        return gizmoName;
    }

    
}