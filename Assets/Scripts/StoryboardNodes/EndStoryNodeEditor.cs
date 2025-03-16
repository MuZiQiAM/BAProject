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
        node.gizmo = EditorUIProvider.DrawGizmoReference(node.gizmo, LabelColor);

        if (node.gizmo != null)
        {
            node.gizmo.linkedNode = node;
        }

        node.storyboard = EditorUIProvider.DrawStoryboardReference(node.storyboard, LabelColor);
        Color prev = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        if (node.storyboard != null)
        {
            if (GUILayout.Button("Show Image"))
            {
                StoryboardManagerEditor.SetVisibility();
            }

            EditorUIProvider.DrawStoryboardFrame(node);
        }


        if (node.gizmo != null)
        {
            if (GUILayout.Button("Jump to Gizmo"))
            {
                if (Application.isPlaying)
                {
                    return;
                }

                var pathToGizmo = node.SearchShortestPathToGizmo();
                GizmoMovement.MoveGizmoThroughPath(node.CurrentSelectedCamera(), pathToGizmo);
                if (node.storyboard is not null)
                {
                    StoryboardManagerEditor.UpdateStoryboard(node.storyboard);
                }
            }

            if (GUILayout.Button("Jump to Gizmo Without Order"))
            {
                if (Application.isPlaying)
                {
                    return;
                }

                node.JumpToGizmo();

                if (node.storyboard is not null)
                {
                    StoryboardManagerEditor.UpdateStoryboard(node.storyboard);
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