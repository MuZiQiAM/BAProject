using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

[Serializable]
[NodeWidth(392)]
public class BoardNodeOld : BaseNode
{
    [HideInInspector] [Input] public int entry;
    [HideInInspector] [Output] public int exit;
    
    
    private bool isCurrentNode = false;
    private BoardNodeOld _previousNodeOld = null;

    private StartStoryNode1 _startNode1;


    private Camera previousCamera;
    
    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);
        Debug.Log("connection removed!");
        if (_startNode1 != null)
        {
            Debug.Log("method called, storynode is" + _startNode1);
            _startNode1.RemoveNodeFromAssigned(this);
        }

        if (port.IsInput && port.GetInputSum(1) == 1)
        {
       
        }
    }


    public BoardNodeOld GetNextBoardNode()
    {
        NodePort exitPort = GetOutputPort("exit");
        if (exitPort == null || exitPort.ConnectionCount == 0) return null;

        return exitPort.GetConnection(0).node as BoardNodeOld;
    } 

    public List<BoardNodeOld> FindPath(BoardNodeOld targetNode)
    {
        Queue<BoardNodeOld> queue = new Queue<BoardNodeOld>();
        Dictionary<BoardNodeOld, BoardNodeOld> cameFrom = new Dictionary<BoardNodeOld, BoardNodeOld>();
    
        queue.Enqueue(this);
        cameFrom[this] = null;

        while (queue.Count > 0)
        {
            BoardNodeOld current = queue.Dequeue();

            if (current == targetNode)
            {
                // Reconstruct the path
                List<BoardNodeOld> path = new List<BoardNodeOld>();
                while (current != null)
                {
                    path.Insert(0, current);
                    current = cameFrom[current];
                }
                return path;
            }

            foreach (BoardNodeOld neighbor in current.GetAllConnectedNodes<BoardNodeOld>("exit"))
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

    
    public void SetAsCurrentNode(BoardNodeOld prev)
    {
        isCurrentNode = true;
        _previousNodeOld = prev;
    }

    public void UnsetAsCurrentNode()
    {
        isCurrentNode = false;
        _previousNodeOld = null;
    }

    public bool IsCurrentNode()
    {
        return isCurrentNode;
    }

    public BoardNodeOld GetPreviousNode()
    {
        return _previousNodeOld;
    }

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "exit") return exit;
        if (port.fieldName == "entry") return entry;
        return null;
    }

    public override NodeTypes GetNodeType()
    {
        return NodeTypes.BOARD_NODE;
    }
}