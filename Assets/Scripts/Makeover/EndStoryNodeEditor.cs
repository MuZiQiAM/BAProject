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
        base.OnBodyGUI();

        EditorUIProvider.DrawEntryPort(node.GetInputPort(nameof(node.Entry)), LabelColor);

        node.SetCurrentSelected(EditorUIProvider.DrawCameraReference(node.CurrentSelectedCamera()));
        node.Gizmo = EditorUIProvider.DrawGizmoReference(node.Gizmo, LabelColor);
        node.Storyboard = EditorUIProvider.DrawStoryboardReference(node.Storyboard, LabelColor);



        if (node.Storyboard is not null)
        {
            EditorUIProvider.DrawStoryboardFrame(node);
        }

        if (node.Gizmo is not null)
        {
            EditorUIProvider.DrawGizmoInfo(node.GizmoRotationAngle(), node.GizmoPosition());
        }

        serializedObject.ApplyModifiedProperties();
    }

    public override Color GetTint()
    {
        return EditorUIProvider.EndNodeColor;
    }
}