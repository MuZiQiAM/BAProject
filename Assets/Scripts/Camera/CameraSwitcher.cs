using UnityEngine;
using UnityEditor;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera; // Player camera
    public Camera storyboardCamera; // Editor view camera

    private void Awake()
    {
        SwitchCamera(Application.isPlaying);
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
#endif
    }

    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            SwitchCamera(true);
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            SwitchCamera(false);
        }
    }

    private void SwitchCamera(bool inGame)
    {
        if (mainCamera == null || storyboardCamera == null)
        {
            Debug.LogWarning("Cameras are not assigned!");
            return;
        }

        mainCamera.gameObject.SetActive(inGame);
        storyboardCamera.gameObject.SetActive(!inGame);
    }
}