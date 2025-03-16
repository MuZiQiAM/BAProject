using Makeover;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;


namespace DefaultNamespace.Makeover
{
    public static class EditorUIProvider
    {
        private static readonly Color LabelColor = ColorConverter.HexToColor("27657E");
        public static readonly Color EndNodeColor = ColorConverter.HexToColor("80b280");
        public static readonly Color DecisionNodeColor = ColorConverter.HexToColor("F4D667");
        public static readonly Color BoardNodeColor = ColorConverter.HexToColor("95CAE0");

        public static Camera DrawCameraReference(Camera camera, Color labelColor)
        {
            return CreateCustomObjectField<Camera>(
                labelColor,
                "Storyboard Camera",
                camera
            );
        }
        public static Camera DrawCameraReference(Camera camera)
        {
            return DrawCameraReference(camera, LabelColor);
        }

        public static GizmoObject DrawGizmoReference(GizmoObject gizmoObject, Color labelColor)
        {
            return CreateCustomObjectField<GizmoObject>(
                labelColor,
                "Gizmo Reference",
                gizmoObject
            );
        }
        public static GizmoObject DrawGizmoReference(GizmoObject gizmoObject)
        {
            return DrawGizmoReference(gizmoObject, LabelColor);
        }

        public static Sprite DrawStoryboardReference(Sprite storyboard, Color labelColor)
        {
            return CreateCustomObjectField<Sprite>(
                labelColor,
                "Storyboard",
                storyboard
            );
        }
        public static Sprite DrawStoryboardReference(Sprite storyboard)
        {
            return DrawStoryboardReference(storyboard, LabelColor);
        }
        
        public static void DrawStoryboardFrame(StoryNode storyNode)
        {
            
            Texture2D texture = storyNode.storyboard.texture;
            Rect spriteRect = storyNode.storyboard.rect;
            
            float spriteWidth = spriteRect.width;
            float spriteHeight = spriteRect.height;

            float maxWidth = 360;
            float maxHeight = 240;

            float widthScale = maxWidth / spriteWidth;
            float heightScale = maxHeight / spriteHeight;

            float displayWidth = spriteWidth * widthScale;
            float displayHeight = spriteHeight * heightScale;

            Rect texCoords = new Rect(
                spriteRect.x / texture.width,
                spriteRect.y / texture.height,
                spriteRect.width / texture.width,
                spriteRect.height / texture.height
            );

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Rect drawRect = GUILayoutUtility.GetRect(displayWidth, displayHeight, GUILayout.ExpandWidth(false));
            GUI.DrawTextureWithTexCoords(drawRect, texture, texCoords, true);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
        }
        public static void DrawEntryPort(NodePort port, Color color)
        {
            GUIStyle entryLabelStyle = new GUIStyle(EditorStyles.label);
            entryLabelStyle.normal.textColor = color;
            entryLabelStyle.fontStyle = FontStyle.Bold;
            entryLabelStyle.alignment = TextAnchor.MiddleRight;

            EditorGUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, port, GUILayout.Width(20));
            GUILayout.Space(-10);
            GUILayout.Label("Entry", entryLabelStyle, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
        }
        public static void DrawExitPort(NodePort port, Color color)
        {
            GUIStyle exitLabelStyle = new GUIStyle(EditorStyles.label);
            exitLabelStyle.normal.textColor = color;
            exitLabelStyle.fontStyle = FontStyle.Bold;
            exitLabelStyle.alignment = TextAnchor.MiddleRight;

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Exit", exitLabelStyle, GUILayout.Width(40));
            GUILayout.Space(-120);
            NodeEditorGUILayout.PortField(GUIContent.none, port, GUILayout.MinWidth(0));
            EditorGUILayout.EndHorizontal();
        }
        public static string CreateCustomTextField(
            Color textColor, 
            string labelText, 
            string value, 
            Node targetNode, 
            string propertyName)
        {
            var labelStyle = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = textColor },
                fontStyle = FontStyle.Bold
            };

            GUILayoutOption[] layoutOptions = { GUILayout.MinWidth(100), GUILayout.MaxWidth(200) };

            EditorGUILayout.BeginHorizontal();
    
            EditorGUILayout.LabelField(labelText, labelStyle, GUILayout.MaxWidth(150));
    
            EditorGUI.BeginChangeCheck();
            string newValue = EditorGUILayout.TextField(value, layoutOptions);
    
            if (EditorGUI.EndChangeCheck())
            {
                // Mark the node as changed and save the new value
                Undo.RecordObject(targetNode, "Edit Node Property");
                targetNode.GetType().GetField(propertyName).SetValue(targetNode, newValue);
                EditorUtility.SetDirty(targetNode);
            }

            EditorGUILayout.EndHorizontal();

            return newValue;
        }
        public static void DrawGizmoInfo(string rotation, string position)
        {
            //BaseNode baseNode
            GUIStyle headerStyle = new GUIStyle(EditorStyles.label);
            headerStyle.normal.textColor = Color.white;
            headerStyle.fontStyle = FontStyle.Bold;

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.normal.textColor = Color.white;

            EditorGUILayout.LabelField("", headerStyle);
            
           
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Gizmo Position", headerStyle, GUILayout.MaxWidth(150));
            EditorGUILayout.LabelField(position, labelStyle);
            EditorGUILayout.EndHorizontal();
            
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Gizmo Rotation", headerStyle, GUILayout.MaxWidth(150));
            EditorGUILayout.LabelField(rotation, labelStyle);
            EditorGUILayout.EndHorizontal();
        }

        private static T CreateCustomObjectField<T>(
            Color textColor,
            string labelText,
            Object objectReference
        ) where T : Object
        {
            var labelStyle = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = textColor },
                fontStyle = FontStyle.Bold
            };

            GUILayoutOption[] layoutOptions = { GUILayout.MinWidth(100), GUILayout.MaxWidth(200) };

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(labelText, labelStyle, GUILayout.MaxWidth(150));
            var result = (T)EditorGUILayout.ObjectField(objectReference, typeof(T), true, layoutOptions);

            EditorGUILayout.EndHorizontal();

            return result;
        }
    }
}