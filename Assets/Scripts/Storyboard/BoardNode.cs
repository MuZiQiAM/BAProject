using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

[Serializable]
[NodeWidth(400)]
public class BoardNode : BaseNode
{
    [HideInInspector] [Input] public int entry;
    [HideInInspector] [Output] public int exit;

    [HideInInspector] public string scene;

    [HideInInspector] public Sprite storyboard;
    
    private bool isCurrentNode = false;
    private BoardNode previousNode = null;

    public Image storyboardUI;

    private StartStoryNode _startNode;

    [HideInInspector] public GizmoObject gizmoReference;


    [HideInInspector] public Camera selectedCamera;
    private Camera previousCamera;

   
    
    
    /*public override void OnCreateConnection(NodePort from, NodePort to)
    {
        base.OnCreateConnection(from, to);

        // If this node is connected to StartStoryNode, update camera
        if (from.node is StartStoryNode startNode)
        {
            Debug.Log("start node added to: " + to.node);
            _startNode = startNode;
            startNode.AssignCameraToBoardNodes();
        }

        if (from.node is BoardNode boardNode && boardNode._startNode != null)
        {
            Debug.Log("new board Node is connected");
            this._startNode = boardNode._startNode;
            Debug.Log("this boardnode has new story node: " + _startNode);
            _startNode.AssignCameraToBoardNodes();
        }
    }*/

    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);
        Debug.Log("connection removed!");
        if (_startNode != null)
        {
            Debug.Log("method called, storynode is" + _startNode);
            _startNode.RemoveNodeFromAssigned(this);
        }

        if (port.IsInput && port.GetInputSum(1) == 1)
        {
            ResetCamera();
            RemoveStartNode();
        }
    }


    public BoardNode GetNextBoardNode()
    {
        NodePort exitPort = GetOutputPort("exit");
        if (exitPort == null || exitPort.ConnectionCount == 0) return null;

        return exitPort.GetConnection(0).node as BoardNode;
    } 
    public void SetSelectedCamera(Camera camera)
    {
        selectedCamera = camera;
    }

    public void RemoveStartNode()
    {
        this._startNode = null;
    }

    public void ResetCamera()
    {
        selectedCamera = null;
    }
    
    public List<BoardNode> FindPath(BoardNode targetNode)
    {
        Queue<BoardNode> queue = new Queue<BoardNode>();
        Dictionary<BoardNode, BoardNode> cameFrom = new Dictionary<BoardNode, BoardNode>();
    
        queue.Enqueue(this);
        cameFrom[this] = null;

        while (queue.Count > 0)
        {
            BoardNode current = queue.Dequeue();

            if (current == targetNode)
            {
                // Reconstruct the path
                List<BoardNode> path = new List<BoardNode>();
                while (current != null)
                {
                    path.Insert(0, current);
                    current = cameFrom[current];
                }
                return path;
            }

            foreach (BoardNode neighbor in current.GetAllConnectedNodes<BoardNode>("exit"))
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return null; // No path found
    }

    
    public void SetAsCurrentNode(BoardNode prev)
    {
        isCurrentNode = true;
        previousNode = prev;
    }

    public void UnsetAsCurrentNode()
    {
        isCurrentNode = false;
        previousNode = null;
    }

    public bool IsCurrentNode()
    {
        return isCurrentNode;
    }

    public BoardNode GetPreviousNode()
    {
        return previousNode;
    }

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "exit") return exit;
        if (port.fieldName == "entry") return entry;
        return null;
    }

    private void SetScene(string newName)
    {
        scene = newName;
    }

    public override NodeTypes GetNodeType()
    {
        return NodeTypes.BOARD_NODE;
    }
}