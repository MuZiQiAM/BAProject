using System.ComponentModel;
using DefaultNamespace.Makeover;
using Makeover;
using UnityEngine;
using XNodeEditor;


[CustomNodeEditor(typeof(BoardStoryNode))]
public class BoardNodeEditor : NodeEditor
{
    private static readonly Color LabelColor = ColorConverter.HexToColor("27657E");
    
    public override void OnBodyGUI()
    {
        if (target is not BoardStoryNode node)
        {
            return;
        }

        serializedObject.Update();
        base.OnBodyGUI();
        EditorUIProvider.DrawEntryPort(node.GetInputPort(nameof(node.Entry)), LabelColor);
        EditorUIProvider.DrawExitPort(node.GetOutputPort(nameof(node.Exit)), LabelColor);

        node.SetCurrentSelected(EditorUIProvider.DrawCameraReference(node.CurrentSelectedCamera()));
        node.Gizmo = EditorUIProvider.DrawGizmoReference(node.Gizmo);
        if (node.Gizmo is not null)
        {
            node.Gizmo.linkedNode = node;
        }
        node.Storyboard = EditorUIProvider.DrawStoryboardReference(node.Storyboard);

        if (node.Storyboard is not null)
        {
            EditorUIProvider.DrawStoryboardFrame(node);
        }

        if (GUILayout.Button("Is camera on gizmo"))
        {
            Debug.Log(node.GetAllNodes().ToString());
        }
        EditorUIProvider.DrawStoryboardFrame(node);
        Color prev = GUI.backgroundColor;
        GUI.backgroundColor = Color.cyan;
        if (node.Gizmo is not null)
        {
            if (GUILayout.Button("Jump to Gizmo"))
            {
                // todo remove button if in application is playing state
                if (Application.isPlaying)
                {
                    return;
                }
                
                node.JumpToGizmo();
                if (node.Storyboard is not null)
                {
                    StoryboardManagerEditor.UpdateStoryboard(node.Storyboard);
                }
            }

            
            EditorUIProvider.DrawGizmoInfo(node.GizmoRotationAngle(), node.GizmoPosition());
        }

        GUI.backgroundColor = prev;
        serializedObject.ApplyModifiedProperties();
    }
    
    public override Color GetTint()
    {
        return EditorUIProvider.BoardNodeColor;
    }
}