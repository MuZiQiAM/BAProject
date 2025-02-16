using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using XNode;
using System.Linq;
using Unity.VisualScripting;

[CustomNodeEditor(typeof(BoardNode))]
public class NodeDrawer : NodeEditor
{
    private BoardNode _boardNode;

    public override void OnBodyGUI()
    {
        if (_boardNode == null)
        {
            _boardNode = target as BoardNode;
        }
        
        serializedObject.Update();
        base.OnBodyGUI();
        
        
        _boardNode.storyboard =
          (Sprite)EditorGUILayout.ObjectField("storyboard", _boardNode.storyboard, typeof(Sprite), false);
        
        _boardNode.gizmoReference =
            (GizmoObject)EditorGUILayout.ObjectField("Gizmo Reference", _boardNode.gizmoReference, typeof(GizmoObject), true);

        //Gizmo Section
        if (_boardNode.gizmoReference != null)
        {
            EditorGUILayout.LabelField("Gizmo Position", _boardNode.gizmoReference.transform.position.ToString());
            EditorGUILayout.LabelField("Gizmo Rotation", _boardNode.gizmoReference.transform.rotation.eulerAngles.ToString());

            if (GUILayout.Button("Jump to Gizmo"))
            {
                JumpToGizmo(
                    _boardNode.gizmoReference.transform.position,
                    
                    Quaternion.Euler(_boardNode.gizmoReference.cameraRotation)
                );
            }
        }

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
            float scale = Mathf.Min(widthScale, heightScale);

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

        serializedObject.ApplyModifiedProperties();
    }
    
    private void JumpToGizmo(Vector3 position, Quaternion rotation)
    {
        if (SceneView.lastActiveSceneView != null)
        {
            SceneView.lastActiveSceneView.pivot = position;
            SceneView.lastActiveSceneView.rotation = rotation;
            SceneView.lastActiveSceneView.size = 1; 
            SceneView.lastActiveSceneView.Repaint();
        }
    }
}