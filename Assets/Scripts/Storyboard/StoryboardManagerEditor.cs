#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[InitializeOnLoad]
public static class StoryboardManagerEditor
{
    private static Image storyboardUI; // Reference to the UI Image
    private static Sprite currentStoryboard; // Track current sprite to avoid redundant updates

    static StoryboardManagerEditor()
    {
        EditorApplication.update += UpdateStoryboardUI;
    }

    public static void SetStoryboardUI(Image uiElement)
    {
        storyboardUI = uiElement;
    }

    public static void UpdateStoryboard(Sprite newStoryboard)
    {
        if (storyboardUI == null)
        {
            Debug.LogError("Storyboard UI Image is not assigned in StoryboardManagerEditor!");
            return;
        }

        if (currentStoryboard == newStoryboard) return; // Avoid unnecessary updates

        currentStoryboard = newStoryboard;
        storyboardUI.sprite = newStoryboard;
        storyboardUI.enabled = newStoryboard != null; // Hide UI if no sprite
        SceneView.RepaintAll(); // Refresh scene view
    }

    private static void UpdateStoryboardUI()
    {
        if (storyboardUI != null && currentStoryboard != null)
        {
            storyboardUI.sprite = currentStoryboard;
        }
    }
}
#endif