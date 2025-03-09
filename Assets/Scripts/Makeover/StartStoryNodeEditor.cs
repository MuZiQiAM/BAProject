using DefaultNamespace.Makeover;
using Makeover;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(StartStoryNode))]
public class StartStoryNodeEditor : NodeEditor
{

    private static readonly Color StartNodeColor = ColorConverter.HexToColor("E09595");
    private static readonly Color LabelColor = ColorConverter.HexToColor("630f0f");
    
    public override void OnBodyGUI()
    {
        if (target is not StartStoryNode node)
        {
            return;
        }

        serializedObject.Update();
        base.OnBodyGUI();
        
        EditorUIProvider.DrawExitPort(node.GetOutputPort(nameof(node.Exit)), Color.white);

        node.selectedCamera = EditorUIProvider.DrawCameraReference(node.selectedCamera, LabelColor);
        node.Gizmo = EditorUIProvider.DrawGizmoReference(node.Gizmo, LabelColor);
        node.Storyboard = EditorUIProvider.DrawStoryboardReference(node.Storyboard, LabelColor);

        if (node.Storyboard is not null)
        {
            EditorUIProvider.DrawStoryboardFrame(node);
        }

        Color prev = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;
        if (node.Gizmo is not null)
        {
            if (GUILayout.Button("Jump to Gizmo"))
            {
                if (Application.isPlaying)
                {
                    return;
                }

                if (node.selectedCamera is not null)
                {
                    node.JumpToGizmo();
                    StoryboardManagerEditor.UpdateStoryboard(node.Storyboard);
                }
                else
                {
                    Debug.Log("Please assign a camera first!");
                }
            }
            
            GUI.backgroundColor = prev;
            EditorUIProvider.DrawGizmoInfo(node.GizmoRotationAngle(), node.GizmoPosition());
        }

        GUI.backgroundColor = prev;
        serializedObject.ApplyModifiedProperties();
    }
    
    public override Color GetTint()
    {
        return StartNodeColor;
    }
}