using System;
using System.Collections.Generic;
using DefaultNamespace.Makeover;
using UnityEngine;
using XNode;

namespace Makeover
{
    public abstract class StoryNode : Node, IStoryNode
    {
        [HideInInspector] public Sprite Storyboard;
        [HideInInspector] public GizmoObject Gizmo;
        [HideInInspector] public string nodeName = "Enter Gizmo Number here";
        
        
        public Vector3 Position()
        {
            if (Gizmo is null)
            {
                throw new Exception(nameof(Gizmo));
            }

            return Gizmo.transform.position;
        }
 

        public Quaternion Rotation()
        {
            if (Gizmo is null)
            {
                throw new Exception(nameof(Gizmo));
            }

            return Gizmo.transform.rotation;
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
            if (Gizmo == null)
            {
                return false;
            }

            var cameraPosition = CurrentSelectedCamera().transform.position;
            return Gizmo.transform.position.Equals(cameraPosition);
        }

        public Texture2D StoryBoardTexture()
        {
            if (Storyboard is null)
            {
                throw new ApplicationException("Invalid state. Cannot call StoryBoardRect() if no StoryBoard was set");
            }

            return Storyboard.texture;
        }

        public Rect StoryBoardRect()
        {
            if (Storyboard is null)
            {
                throw new ApplicationException("Invalid state. Cannot call StoryBoardRect() if no StoryBoard was set");
            }

            return Storyboard.rect;
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
            return Gizmo.transform.rotation.eulerAngles.ToString();
        }

        public string GizmoPosition()
        {
            return Gizmo.transform.position.ToString();
        } 
        public void JumpToGizmo()
        {
            var camera = CurrentSelectedCamera();
            if (camera is null)
            {
                return;
            }

            if (Gizmo is null)
            {
                return;
            }

            var transform = Gizmo.transform;
            Gizmo.JumpToGizmo(camera, transform.position, transform.rotation);
        }

        public abstract Camera CurrentSelectedCamera();

        public abstract void SetCurrentSelected(Camera camera);
    }
}