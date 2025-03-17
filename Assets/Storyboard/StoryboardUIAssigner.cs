#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StoryboardUIAssigner : MonoBehaviour
{
    public Image storyboardUIImage; // Assign in Inspector

    private void OnEnable()
    {
        if (storyboardUIImage != null)
        {
            StoryboardManagerEditor.SetStoryboardUI(storyboardUIImage);
        }
    }
}
#endif