using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(Width)]
public class StartStoryNode1 : BaseNode
{
    private const int Width = 392;
    
    [HideInInspector] [Output] public NodePort exit;
  
    private HashSet<BoardNodeOld> assignedNodes = new HashSet<BoardNodeOld>();

    public List<Player> Players = new List<Player>();


    public override string GetString()
    {
        return "Start";
    }
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "exit") return exit;
        return null;
    }
    
    public void RemoveNodeFromAssigned(BoardNodeOld nodeOld)
    {
        if (nodeOld != null && assignedNodes.Contains(nodeOld))
        {
            assignedNodes.Remove(nodeOld);
        }
    }

    
    public override NodeTypes GetNodeType()
    {
        return NodeTypes.START_NODE;
    }

}