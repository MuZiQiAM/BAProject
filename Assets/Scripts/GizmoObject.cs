#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Makeover;
using UnityEngine;

public class GizmoObject : MonoBehaviour
{
    public string gizmoName;
    public StoryNode linkedNode;
    
    private void Start()
    {
        if (Application.isPlaying) 
        {
            this.gameObject.SetActive(false); 
        }
    }
    
    private void OnDrawGizmos()
    {
        if (linkedNode == null) return; // Now this should never be null!

        ISet<StoryNode> connectedNodes = linkedNode.GetAllNodes();

        foreach (var node in connectedNodes)
        {
            if (node.Gizmo != null)
            {
                DrawConnectionLine(node.Gizmo);
            }
        }
    }
    public void SetVisibility(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
    }
    public void DrawConnectionLine(GizmoObject targetGizmo)
    {
        if (targetGizmo == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, targetGizmo.transform.position);
    }

    public string GetGizmoName()
    {
        return gizmoName;
    }

    public void JumpToGizmo(Camera storyboardCamera, Vector3 targetPosition, Quaternion targetRotation,
        float duration = 400f)
    {
        if (storyboardCamera == null)
        {
            Debug.LogError("Storyboard Camera is not assigned!");
            return;
        }
#if UNITY_EDITOR
        Debug.Log("start");
        // Use Editor Coroutine for Smooth Transition in Edit Mode
        EditorApplication.update -= MoveCamera;
        elapsedTime = 0f;
        var transform1 = storyboardCamera.transform;
        startPosition = transform1.position;
        startRotation = transform1.rotation;
        this.storyboardCamera = storyboardCamera;
        this.targetPosition = targetPosition;
        this.targetRotation = targetRotation;
        this.duration = duration;
        startTime = Time.realtimeSinceStartup;
        EditorApplication.update += MoveCamera;
        Debug.Log("end");
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
    private float startTime;

    private void MoveCamera()
    {
        // Force Unity to refresh the Scene View
        // SceneView.RepaintAll();
        if (storyboardCamera == null) return;

        elapsedTime += Time.realtimeSinceStartup - startTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        storyboardCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        storyboardCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

        if (t >= 1f)
        {
            EditorApplication.update -= MoveCamera;
            Debug.Log("End");
        }
    }
#endif
}