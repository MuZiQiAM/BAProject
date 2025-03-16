using System;
using DefaultNamespace.Makeover;
using Makeover;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;


[CustomNodeEditor(typeof(DecisionStoryNode))]
public class DecisionNodeEditor : NodeEditor
{
    private static readonly Color LabelColor = ColorConverter.HexToColor("bb6611");
    public static readonly Color ListColor = ColorConverter.HexToColor("f4d667");

    public override void OnBodyGUI()
    {
        if (target is not DecisionStoryNode node)
        {
            return;
        }

        serializedObject.Update();
        base.OnBodyGUI();

        node.nodeName = EditorUIProvider.CreateCustomTextField(LabelColor, "Node Name:", node.nodeName, node, "nodeName");

        EditorUIProvider.DrawEntryPort(node.GetInputPort(nameof(node.Entry)), LabelColor);

        Color prev = GUI.backgroundColor;
        GUI.backgroundColor = ListColor;
        DrawExitPortsWithBackground();
        GUI.backgroundColor = prev;

        node.SetCurrentSelected(EditorUIProvider.DrawCameraReference(node.CurrentSelectedCamera(), LabelColor));
        node.Gizmo = EditorUIProvider.DrawGizmoReference(node.Gizmo, LabelColor);

        if (node.Gizmo is not null)
        {
            node.Gizmo.linkedNode = node;
        }

        node.Storyboard = EditorUIProvider.DrawStoryboardReference(node.Storyboard, LabelColor);
        prev = GUI.backgroundColor;
        GUI.backgroundColor = LabelColor;
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

                GUI.backgroundColor = prev;
                EditorUIProvider.DrawGizmoInfo(node.GizmoRotationAngle(), node.GizmoPosition());
            }

            if (GUILayout.Button("Jump to Gizmo Without Order"))
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
            EditorUIProvider.DrawGizmoInfo(node.GizmoRotationAngle(), node.GizmoPosition());
        }
        

        GUI.backgroundColor = prev;
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawExitPortsWithBackground()
    {
        GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
        backgroundStyle.normal.background = MakeTex(2, 2, ListColor);

        EditorGUILayout.BeginVertical(backgroundStyle);

        GUIStyle exitLabelStyle = new GUIStyle(EditorStyles.boldLabel);
        exitLabelStyle.normal.textColor = LabelColor;
        EditorGUILayout.LabelField("Decisions", exitLabelStyle);

        NodeEditorGUILayout.DynamicPortList(
            "Exits",
            typeof(StoryNode),
            serializedObject,
            NodePort.IO.Output,
            Node.ConnectionType.Override,
            Node.TypeConstraint.None
        );

        EditorGUILayout.EndVertical();
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    public override Color GetTint()
    {
        return EditorUIProvider.DecisionNodeColor;
    }
}