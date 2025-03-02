using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using XNode;
using Microsoft.Unity.VisualStudio.Editor;


[CustomNodeEditor(typeof(StartStoryNode))]
public class StartStoryNodeDrawer : NodeEditor
{
    private StartStoryNode _startStoryNode;

    public Image storyboardUI;


    private Camera storyboardCamera;

    public override void OnBodyGUI()
    {
        if (_startStoryNode == null)
        {
            _startStoryNode = target as StartStoryNode;
        }

        serializedObject.Update();
        base.OnBodyGUI();
        
        GUIStyle entryLabelStyle = new GUIStyle(EditorStyles.label);
        entryLabelStyle.normal.textColor = Color.black; 
        entryLabelStyle.alignment = TextAnchor.MiddleRight;
        
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Push everything to the right
        GUILayout.Label("Exit", entryLabelStyle, GUILayout.Width(40)); 
        GUILayout.Space(-120);
        NodeEditorGUILayout.PortField(GUIContent.none, _startStoryNode.GetOutputPort("exit"), GUILayout.MinWidth(0));
        EditorGUILayout.EndHorizontal();
        

        //_boardNode.storyboard = (Sprite)EditorGUILayout.ObjectField("Storyboard", _boardNode.storyboard, typeof(Sprite), true);

        _startStoryNode.selectedCamera = (Camera) CreateCustomObjectField(
            ColorConverter.HexToColor("27657E"), 
            "Storyboard Camera", 
            _startStoryNode.selectedCamera, 
            typeof(Camera)
        );
        
        serializedObject.ApplyModifiedProperties();
    }
    
    public override Color GetTint()
    {
        return ColorConverter.HexToColor("E09595");
    }
    
    public Object CreateCustomObjectField(Color textColor, string labelText, Object objectReference, System.Type objectType)
    {
        // Custom label style
        GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = textColor },
        };

        GUILayoutOption[] layoutOptions = { GUILayout.MinWidth(100), GUILayout.MaxWidth(200) };

        EditorGUILayout.BeginHorizontal();

        // Custom label on the left
        EditorGUILayout.LabelField(labelText, labelStyle, GUILayout.MaxWidth(100));

        // Directly use the field instead of serialized property
        Object result = EditorGUILayout.ObjectField(objectReference, objectType, true, layoutOptions);

        EditorGUILayout.EndHorizontal();

        return result;
    }
    
}