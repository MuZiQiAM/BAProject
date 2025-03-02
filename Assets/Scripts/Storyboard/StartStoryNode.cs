using System.Collections.Generic;
using UnityEngine;
using XNode;

public interface IBoardNodeObserver
{
    void UpdateCamera(Camera camera);
}

[NodeWidth(350)]
public class StartStoryNode : BaseNode
{
    [HideInInspector] [Output] public NodePort exit;

    [HideInInspector] public Camera selectedCamera;
    
    private HashSet<BoardNode> assignedNodes = new HashSet<BoardNode>();


    public override string GetString()
    {
        return "Start";
    }
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "exit") return exit;
        return null;
    }

    /*public void AssignCameraToBoardNodes()
    {
        return;
        if (selectedCamera == null) return;

        assignedNodes.Clear(); // Clear old assignments before re-assigning
        AssignCameraRecursive(GetConnectedNode<BoardNode>("exit"));
    }
    private void AssignCameraRecursive(BoardNode node)
    {
        if (node == null || assignedNodes.Contains(node)) return;

        node.SetSelectedCamera(selectedCamera);
        assignedNodes.Add(node);

        List<BoardNode> connectedNodes = node.GetAllConnectedNodes<BoardNode>("exit");
        foreach (var connectedNode in connectedNodes)
        {
            AssignCameraRecursive(connectedNode);
        }
    }*/
    public void RemoveNodeFromAssigned(BoardNode node)
    {
        if (node != null && assignedNodes.Contains(node))
        {
            assignedNodes.Remove(node);
        }
    }

    
    public override NodeTypes GetNodeType()
    {
        return NodeTypes.START_NODE;
    }

}