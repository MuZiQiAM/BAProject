using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Makeover
{
#if UNITY_EDITOR

    public class GizmoMovement
    {
        private const float Duration = 200f;

        [NotNull] private readonly Camera _storyboardCamera;
        private readonly GizmoMovement _nextMovement;

        private readonly StoryNode _from;
        private readonly StoryNode _to;

        private float _elapsedTime;
        private float _startTime;


        private GizmoMovement(
            [NotNull] Camera storyboardCamera,
            StoryNode from,
            StoryNode to,
            GizmoMovement nextMovement
        )
        {
            _storyboardCamera = storyboardCamera ? storyboardCamera : throw new System.ArgumentNullException(nameof(storyboardCamera));
            _nextMovement = nextMovement;
            _from = from;
            _to = to;

            _startTime = 0f;
            _elapsedTime = 0f;
        }

        public static void MoveGizmoThroughPath(Camera camera, IList<StoryNode> path)
        {
            if (path.Count < 1)
            {
                throw new Exception("Invalid state. At least the end must be part of the path.");    
            }
            
            if (path.Count == 1)
            {
                return;
            }
            
            GizmoMovement nextMovement = null;
            for (var i = path.Count - 1; i >= 1; i--)
            {
                var from = path[i - 1];
                var to = path[i];
                
                var movement = new GizmoMovement(
                    camera,
                    from,
                    to,
                    nextMovement);
                nextMovement = movement;
            }

            if (nextMovement is null)
            {
                throw new Exception("Invalid state. " + nameof(nextMovement) + " cannot be null.");    
            }
            
            nextMovement.StartMovement();
        }

        private void StartMovement()
        {
            _startTime = Time.realtimeSinceStartup;
            EditorApplication.update += MoveCamera;
        }
        
        private void MoveCamera()
        {
            // Force Unity to refresh the Scene View
            // SceneView.RepaintAll();
            if (_storyboardCamera == null) return;

            _elapsedTime += Time.realtimeSinceStartup - _startTime;
            var t = Mathf.Clamp01(_elapsedTime / Duration);

            _storyboardCamera.transform.rotation = Quaternion.Slerp(_from.Rotation(),_to.Rotation(), t);
            _storyboardCamera.transform.position = Vector3.Lerp(_from.Position(), _to.Position(), t);

            if (t >= 1f)
            {
                EditorApplication.update -= MoveCamera;
                _nextMovement?.StartMovement();
            }
        }
    }
#endif
}