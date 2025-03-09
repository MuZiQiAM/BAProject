using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class BaseNode : Node
{
    [HideInInspector] public int id;
    [HideInInspector] public Sprite storyboard;
    [HideInInspector] public GizmoObject gizmoReference;
    [HideInInspector] public Camera selectedCamera;

    public virtual string GetString()
    {
        return null;
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }


    public BaseNode GetNextNode()
    {
        NodePort exitPort = GetOutputPort("exit");
        if (exitPort == null || exitPort.ConnectionCount == 0) return null;

        return exitPort.GetConnection(0).node as BaseNode;
    }

    public List<T> GetAllConnectedNodes<T>(string basePortName) where T : Node
    {
        var connectedNodes = new List<T>();
        foreach (var port in Ports)
            if (port.fieldName.StartsWith(basePortName))
                if (port.IsConnected)
                    foreach (var connection in port.GetConnections())
                    {
                        var connectedNode = connection.node as T;
                        if (connectedNode != null) connectedNodes.Add(connectedNode);
                    }

        return connectedNodes;
    }

    public T GetConnectedNode<T>(string fieldName) where T : Node
    {
        var port = GetInputPort(fieldName);

        if (port != null && port.IsConnected)
            return port.Connection.node as T;

        return null;
    }

    public virtual NodeTypes GetNodeType()
    {
        return NodeTypes.WRONG_NODE;
    }
}