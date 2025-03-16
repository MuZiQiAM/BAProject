using DefaultNamespace.Makeover;
using Makeover;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(StartStoryNode))]
public class StartStoryNodeEditor : NodeEditor
{
    private static readonly Color StartNodeColor = ColorConverter.HexToColor("E09595");
    private static readonly Color LabelColor = ColorConverter.HexToColor("630f0f");
    public static readonly Color ListColor = ColorConverter.HexToColor("E2062B");
    private SerializedProperty endingsProp;
    private StartStoryNode node;

    public override void OnBodyGUI()
    {
        if (target is not StartStoryNode node)
        {
            return;
        }

        serializedObject.Update();
        
        
        
        node.nodeName = EditorUIProvider.CreateCustomTextField(LabelColor, "Node Name:", node.nodeName, node, "nodeName");

        EditorUIProvider.DrawExitPort(node.GetOutputPort(nameof(node.Exit)), Color.white);

        node.selectedCamera = EditorUIProvider.DrawCameraReference(node.selectedCamera, LabelColor);
        node.Gizmo = EditorUIProvider.DrawGizmoReference(node.Gizmo, LabelColor);
        node.Storyboard = EditorUIProvider.DrawStoryboardReference(node.Storyboard, LabelColor);
        Color prev = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;
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
            node.Gizmo.linkedNode = node;
        }

        
        if (node.Gizmo != null)
        {
            if (GUILayout.Button("Jump to Gizmo"))
            {
                if (Application.isPlaying)
                {
                    return;
                }

                if (node.selectedCamera == null)
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
        GUI.backgroundColor = new Color(Color.red.r,Color.red.b, Color.red.g ,3) ;
        base.OnBodyGUI();
        GUI.backgroundColor = prev;
        

        serializedObject.ApplyModifiedProperties();
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
        return StartNodeColor;
    }
}