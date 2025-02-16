using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class GizmoManager : MonoBehaviour
{
    public static GizmoManager Instance;

    private Dictionary<BoardNode, GizmoObject> nodeToGizmoMap = new Dictionary<BoardNode, GizmoObject>();

 
    
    public void Register(BoardNode node, GizmoObject gizmo)
    {
        if (!nodeToGizmoMap.ContainsKey(node))
        {
            nodeToGizmoMap[node] = gizmo;
        }
    }
    
    public GizmoObject GetGizmo(BoardNode node)
    {
        if (nodeToGizmoMap.TryGetValue(node, out var gizmo))
        {
            return gizmo;
        }
        return null;
    }

    public void DrawConnections(NodeGraph graph)
    {
        
    }
    
}
