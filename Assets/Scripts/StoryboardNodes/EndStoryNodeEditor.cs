using DefaultNamespace.Makeover;
using Makeover;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(EndStoryNode))]
public class EndStoryNodeEditor : NodeEditor
{
    private static readonly Color LabelColor = ColorConverter.HexToColor("006400");

    public override void OnBodyGUI()
    {
        if (target is not EndStoryNode node)
        {
            return;
        }

        serializedObject.Update();

        node.nodeName =
            EditorUIProvider.CreateCustomTextField(LabelColor, "Node Name:", node.nodeName, node, "nodeName");

        EditorUIProvider.DrawEntryPort(node.GetInputPort(nameof(node.Entry)), LabelColor);

        node.SetCurrentSelected(EditorUIProvider.DrawCameraReference(node.CurrentSelectedCamera(), LabelColor));
        node.Gizmo = EditorUIProvider.DrawGizmoReference(node.Gizmo, LabelColor);

        if (node.Gizmo != null)
        {
            node.Gizmo.linkedNode = node;
        }

        node.Storyboard = EditorUIProvider.DrawStoryboardReference(node.Storyboard, LabelColor);
        Color prev = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
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

        serializedObject.ApplyModifiedProperties();
    }

    public override Color GetTint()
    {
        return EditorUIProvider.EndNodeColor;
    }
}