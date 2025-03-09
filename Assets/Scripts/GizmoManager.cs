using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class GizmoManager : MonoBehaviour
{
    public static GizmoManager Instance;

    private Dictionary<BoardNodeOld, GizmoObject> nodeToGizmoMap = new Dictionary<BoardNodeOld, GizmoObject>();

 
    
    public void Register(BoardNodeOld nodeOld, GizmoObject gizmo)
    {
        if (!nodeToGizmoMap.ContainsKey(nodeOld))
        {
            nodeToGizmoMap[nodeOld] = gizmo;
        }
    }
    
    public GizmoObject GetGizmo(BoardNodeOld nodeOld)
    {
        if (nodeToGizmoMap.TryGetValue(nodeOld, out var gizmo))
        {
            return gizmo;
        }
        return null;
    }

    public void DrawConnections(NodeGraph graph)
    {
        
    }
    
}
