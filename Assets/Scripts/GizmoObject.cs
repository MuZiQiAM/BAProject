#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GizmoObject : MonoBehaviour
{
    public string gizmoName;

    public Quaternion cameraRotation;

    public Boolean visible;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1);
    }

    public string GetGizmoName()
    {
        return gizmoName;
    }

    public void JumpToGizmo(Camera storyboardCamera, Vector3 targetPosition, Quaternion targetRotation, float duration = 1f)
    {
        if (storyboardCamera == null)
        {
            Debug.LogError("Storyboard Camera is not assigned!");
            return;
        }

#if UNITY_EDITOR
        // Use Editor Coroutine for Smooth Transition in Edit Mode
        EditorApplication.update -= MoveCamera;
        elapsedTime = 0f;
        startPosition = storyboardCamera.transform.position;
        this.storyboardCamera = storyboardCamera;
        this.targetPosition = targetPosition;
        this.targetRotation = targetRotation;
        this.duration = duration;
        EditorApplication.update += MoveCamera;
#endif
    }

#if UNITY_EDITOR
    private Camera storyboardCamera;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float duration;
    private float elapsedTime = 0f;

    private void MoveCamera()
    {
        if (storyboardCamera == null) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        

        storyboardCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        storyboardCamera.transform.rotation = Quaternion.Slerp( startRotation, targetRotation, t);
        // Force Unity to refresh the Scene View
        SceneView.RepaintAll();

        if (t >= 1f)
        {
            EditorApplication.update -= MoveCamera;
        }
    }
#endif
}