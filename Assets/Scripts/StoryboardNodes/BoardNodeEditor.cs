using System.ComponentModel;
using DefaultNamespace.Makeover;
using Makeover;
using UnityEditor;
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

        node.nodeName =
            EditorUIProvider.CreateCustomTextField(LabelColor, "Node Name:", node.nodeName, node, "nodeName");

        EditorUIProvider.DrawEntryPort(node.GetInputPort(nameof(node.Entry)), LabelColor);
        EditorUIProvider.DrawExitPort(node.GetOutputPort(nameof(node.Exit)), LabelColor);

        node.SetCurrentSelected(EditorUIProvider.DrawCameraReference(node.CurrentSelectedCamera()));
        node.Gizmo = EditorUIProvider.DrawGizmoReference(node.Gizmo);

        if (node.Gizmo != null)
        {
            node.Gizmo.linkedNode = node;
        }

        node.Storyboard = EditorUIProvider.DrawStoryboardReference(node.Storyboard);
        Color prev = GUI.backgroundColor;
        GUI.backgroundColor = Color.cyan;
        if (node.Storyboard != null)
        {
            if (GUILayout.Button("Show Image"))
            {
                StoryboardManagerEditor.SetVisibility();
            }

            EditorUIProvider.DrawStoryboardFrame(node);
        }


        if (node.Gizmo != null)
        {
            if (GUILayout.Button("Jump to Gizmo"))
            {
                if (Application.isPlaying)
                {
                    return;
                }

                var pathToGizmo = node.SearchShortestPathToGizmo();
                GizmoMovement.MoveGizmoThroughPath(node.CurrentSelectedCamera(), pathToGizmo);
                if (node.Storyboard is not null)
                {
                    StoryboardManagerEditor.UpdateStoryboard(node.Storyboard);
                }
            }

            if (GUILayout.Button("Jump to Gizmo without order"))
            {
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

            GUI.backgroundColor = prev;
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