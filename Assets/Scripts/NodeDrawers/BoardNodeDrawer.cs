using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

[CustomNodeEditor(typeof(BoardNode))]
public class BoardNodeDrawer : NodeEditor
{
    private BoardNode _boardNode;


    public override void OnBodyGUI()
    {
        GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
        labelStyle.normal.textColor = ColorConverter.HexToColor("27657E");
        GUIStyle headerStyle = new GUIStyle(EditorStyles.label);
        headerStyle.normal.textColor = ColorConverter.HexToColor("27657E");
        headerStyle.fontStyle = FontStyle.Bold;

        if (_boardNode == null)
        {
            _boardNode = target as BoardNode;
        }

        serializedObject.Update();
        base.OnBodyGUI();


        //draw the ports
        GUIStyle entryLabelStyle = new GUIStyle(EditorStyles.label);
        entryLabelStyle.normal.textColor = Color.black;
        entryLabelStyle.alignment = TextAnchor.MiddleRight;


        Color originalColor = GUI.contentColor;

        EditorGUILayout.BeginHorizontal();
        NodeEditorGUILayout.PortField(GUIContent.none, _boardNode.GetInputPort("entry"),
            GUILayout.Width(20)); // Reduce port width
        GUILayout.Space(-10); // Move text closer to the port
        GUILayout.Label("Entry", entryLabelStyle, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Push everything to the right
        GUILayout.Label("Exit", entryLabelStyle, GUILayout.Width(40));
        GUILayout.Space(-120);
        NodeEditorGUILayout.PortField(GUIContent.none, _boardNode.GetOutputPort("exit"), GUILayout.MinWidth(0));
        EditorGUILayout.EndHorizontal();

        //_boardNode.storyboard = (Sprite)EditorGUILayout.ObjectField("Storyboard", _boardNode.storyboard, typeof(Sprite), true);


        _boardNode.storyboard = (Sprite)CreateCustomObjectField(
            ColorConverter.HexToColor("27657E"),
            "Storyboard",
            _boardNode.storyboard,
            typeof(Sprite)
        );
        _boardNode.gizmoReference = (GizmoObject)CreateCustomObjectField(
            ColorConverter.HexToColor("27657E"),
            "Gizmo Reference",
            _boardNode.gizmoReference,
            typeof(GizmoObject)
        );
        _boardNode.selectedCamera = (Camera)CreateCustomObjectField(
            ColorConverter.HexToColor("27657E"),
            "Storyboard Camera",
            _boardNode.selectedCamera,
            typeof(Camera)
        );


        //Gizmo Section
        if (_boardNode.gizmoReference != null)
        {
            EditorGUILayout.LabelField("", headerStyle);
            EditorGUILayout.LabelField("Gizmo Position", headerStyle);
            EditorGUILayout.LabelField(_boardNode.gizmoReference.transform.position.ToString(), labelStyle);
            EditorGUILayout.LabelField("Gizmo Rotation", headerStyle);
            EditorGUILayout.LabelField(_boardNode.gizmoReference.transform.rotation.eulerAngles.ToString(), labelStyle);

            GUI.contentColor = Color.white;
            GUI.backgroundColor = ColorConverter.HexToColor("7AA6B7");
            /*if (GUILayout.Button("Jump to Gizmo"))
            {
                if (Application.isPlaying)
                {
                    return;
                }

                if (_boardNode.selectedCamera != null)
                {
                    // Use the DOTween-based camera animation
                    StoryboardCameraAnimator animator = new StoryboardCameraAnimator();
                    animator.JumpToGizmo(
                        _boardNode.selectedCamera,
                        _boardNode.gizmoReference.transform.position,
                        _boardNode.gizmoReference.transform.rotation
                    );

                    // Update the storyboard after moving the camera
                    StoryboardManagerEditor.UpdateStoryboard(_boardNode.storyboard);
                }
                else
                {
                    Debug.Log("Please assign a camera first!");
                }
            }*/
            if (GUILayout.Button("Jump to Gizmo"))
            {
                if (Application.isPlaying)
                {
                    return;
                }

                if (_boardNode.selectedCamera != null)
                {
                    
                    _boardNode.gizmoReference.JumpToGizmo(_boardNode.selectedCamera,
                        _boardNode.gizmoReference.transform.position, _boardNode.gizmoReference.transform.rotation);
                    StoryboardManagerEditor.UpdateStoryboard(_boardNode.storyboard);
                }
                else
                {
                    Debug.Log("Please assign a camera first!");
                }
            }
            

            GUI.backgroundColor = originalColor;
        }

        GUI.contentColor = originalColor;


        //Storyboard section
        if (_boardNode.storyboard != null)
        {
            // Get the texture from the sprite

            Texture2D texture = _boardNode.storyboard.texture;

            // Get the native sprite size
            Rect spriteRect = _boardNode.storyboard.rect;
            float spriteWidth = spriteRect.width;
            float spriteHeight = spriteRect.height;

            // Calculate the maximum size the image can occupy in the node
            float maxWidth = 360;
            float maxHeight = 240;

            // Calculate the scaling factor to maintain the aspect ratio
            float widthScale = maxWidth / spriteWidth;
            float heightScale = maxHeight / spriteHeight;
            //float scale = Mathf.Min(widthScale, heightScale);

            // Calculate the final display size
            float displayWidth = spriteWidth * widthScale;
            float displayHeight = spriteHeight * heightScale;

            // Define cropping rect for the sprite
            Rect texCoords = new Rect(
                spriteRect.x / texture.width,
                spriteRect.y / texture.height,
                spriteRect.width / texture.width,
                spriteRect.height / texture.height
            );

            // Add some space before drawing
            GUILayout.Space(10);

            // Reserve a rect for drawing
            Rect drawRect = GUILayoutUtility.GetRect(displayWidth, displayHeight, GUILayout.ExpandWidth(false));

            // Draw the sprite
            GUI.DrawTextureWithTexCoords(drawRect, texture, texCoords, true);
        }

        if (!Application.isPlaying)
        {
            NodeEditorWindow.RepaintAll();
        }

        serializedObject.ApplyModifiedProperties();
    }

    public Object CreateCustomObjectField(Color textColor, string labelText, Object objectReference,
        System.Type objectType)
    {
        // Custom label style
        GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = textColor },
        };

        GUILayoutOption[] layoutOptions = { GUILayout.MinWidth(100), GUILayout.MaxWidth(200) };

        EditorGUILayout.BeginHorizontal();

        // Custom label on the left
        EditorGUILayout.LabelField(labelText, labelStyle, GUILayout.MaxWidth(150));

        // Directly use the field instead of serialized property
        Object result = EditorGUILayout.ObjectField(objectReference, objectType, true, layoutOptions);

        EditorGUILayout.EndHorizontal();

        return result;
    }


    public override Color GetTint()
    {
        return ColorConverter.HexToColor("95CAE0");
    }
}