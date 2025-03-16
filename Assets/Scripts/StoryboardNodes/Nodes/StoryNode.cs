using System;
using System.Collections.Generic;
using DefaultNamespace.Makeover;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

namespace Makeover
{
    public abstract class StoryNode : Node, IStoryNode
    {
        [FormerlySerializedAs("Storyboard")] [HideInInspector] public Sprite storyboard;
        [FormerlySerializedAs("Gizmo")] [HideInInspector] public GizmoObject gizmo;
        [HideInInspector] public string nodeName = "Enter Gizmo Number here";
        
        
        public Vector3 Position()
        {
            if (gizmo is null)
            {
                throw new Exception(nameof(gizmo));
            }

            return gizmo.transform.position;
        }
 

        public Quaternion Rotation()
        {
            if (gizmo is null)
            {
                throw new Exception(nameof(gizmo));
            }

            return gizmo.transform.rotation;
        }

        public ISet<StoryNode> GetAllNodes()
        {
            ISet<StoryNode> connectedNodes = new HashSet<StoryNode>();
            foreach (var nodePort in Ports)
            {
                if (nodePort.Connection?.node is StoryNode n)
                {
                    connectedNodes.Add(n);
                }
            }

            return connectedNodes;
        }

        public StoryNode GetNextNode()
        {
            foreach (var nodePort in Ports)
            {
                if (nodePort.Connection?.node is StoryNode nextNode)
                {
                    return nextNode;
                }
            }

            return null;
        }

        public IList<StoryNode> SearchShortestPathToGizmo()
        {
            StoryNode endNode = null;
            var distances = new Dictionary<StoryNode, int>();
            var previousNodes = new Dictionary<StoryNode, StoryNode>();
            var pq = new PriorityQueue<StoryNode, int>();

            distances[this] = 0;
            pq.Enqueue(this, 0);

            while (pq.Count > 0)
            {
                var currentNode = pq.Dequeue();

                if (currentNode.IsCameraOnGizmo())
                {
                    endNode = currentNode;
                    break;
                }

                foreach (var neighbor in currentNode.GetAllNodes())
                {
                    var newDistance = distances[currentNode] + 1;

                    if (distances.ContainsKey(neighbor) && newDistance >= distances[neighbor])
                        continue;

                    distances[neighbor] = newDistance;
                    previousNodes[neighbor] = currentNode;
                    pq.Enqueue(neighbor, newDistance);
                }
            }

            var path = new List<StoryNode>();
            for (var node = endNode; node != null; node = previousNodes.GetValueOrDefault(node))
            {
                path.Add(node);
            }

            if (path.Count < 1)
            {
                throw new Exception("Invalid state. At least " + nameof(endNode) + " must be part of the path.");
            }

            return path;
        }

        public bool IsCameraOnGizmo()
        {
            if (gizmo == null)
            {
                return false;
            }

            var cameraPosition = CurrentSelectedCamera().transform.position;
            return gizmo.transform.position.Equals(cameraPosition);
        }

        public Texture2D StoryBoardTexture()
        {
            if (storyboard is null)
            {
                throw new ApplicationException("Invalid state. Cannot call StoryBoardRect() if no StoryBoard was set");
            }

            return storyboard.texture;
        }

        public Rect StoryBoardRect()
        {
            if (storyboard is null)
            {
                throw new ApplicationException("Invalid state. Cannot call StoryBoardRect() if no StoryBoard was set");
            }

            return storyboard.rect;
        }

        public override object GetValue(NodePort port)
        {
            if (port.Connection?.node is StoryNode n)
            {
                return n;
            }

            return null;
        }


        public string GizmoRotationAngle()
        {
            return gizmo.transform.rotation.eulerAngles.ToString();
        }

        public string GizmoPosition()
        {
            return gizmo.transform.position.ToString();
        } 
        public void JumpToGizmo()
        {
            var camera = CurrentSelectedCamera();
            if (camera is null)
            {
                return;
            }

            if (gizmo is null)
            {
                return;
            }

            var transform = gizmo.transform;
            gizmo.JumpToGizmo(camera, transform.position, transform.rotation);
        }

        public abstract Camera CurrentSelectedCamera();

        public abstract void SetCurrentSelected(Camera camera);
    }
}