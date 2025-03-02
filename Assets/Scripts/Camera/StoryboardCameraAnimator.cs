#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using DG.Tweening;
using Unity.EditorCoroutines.Editor;

public class StoryboardCameraAnimator
{
    public void JumpToGizmo(Camera storyboardCamera, Vector3 targetPosition, Quaternion targetRotation,
        float duration = 1f)
    {
        if (storyboardCamera == null)
        {
            Debug.LogError("Storyboard Camera is not assigned!");
            return;
        }

        // Ensure DOTween is initialized
        DOTween.Init();

        // Kill any existing tweens on the camera to prevent conflicts
        storyboardCamera.transform.DOKill();

        // Tween Position
        storyboardCamera.transform.DOMove(targetPosition, duration)
            .SetEase(Ease.InOutSine) // Smooth easing function
            .OnUpdate(() => SceneView.RepaintAll()); // Ensure Scene View updates smoothly

        // Tween Rotation
        storyboardCamera.transform.DORotateQuaternion(targetRotation, duration)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() => SceneView.RepaintAll()); // Ensure Scene View updates smoothly
    }

#endif

    private System.Collections.IEnumerator AnimateCamera(Camera storyboardCamera, Vector3 targetPosition,
        Quaternion targetRotation, float duration)
    {
        float elapsedTime = 0f;

        // Tween Position
        Tween moveTween = storyboardCamera.transform.DOMove(targetPosition, duration)
            .SetEase(Ease.InOutSine) // You can change the easing type
            .OnUpdate(() => SceneView.RepaintAll()); // Force Scene View to update

        // Tween Rotation
        Tween rotateTween = storyboardCamera.transform.DORotateQuaternion(targetRotation, duration)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() => SceneView.RepaintAll());

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position & rotation are set correctly
        moveTween.Kill();
        rotateTween.Kill();
        storyboardCamera.transform.position = targetPosition;
        storyboardCamera.transform.rotation = targetRotation;
        SceneView.RepaintAll();
    }
}

