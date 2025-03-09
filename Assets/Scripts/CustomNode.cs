using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    using XNode;

    public class CustomNode : Node {
        
       [Input(backingValue: ShowBackingValue.Never)] public CustomNode inputNode;   // Direct reference to another node
       [Output(dynamicPortList: true)] public List<CustomNode> outputNodes = new List<CustomNode>(); // Multiple outputs

       public string nodeName; // Example node data

       // Retrieves all connected nodes from the output port
       public List<CustomNode> GetConnectedNodes() {
           List<CustomNode> connectedNodes = new List<CustomNode>();
           NodePort port = GetOutputPort("outputNodes");
        
           if (port == null) return connectedNodes;

           foreach (NodePort connection in port.GetConnections()) {
               if (connection.node is CustomNode node) {
                   connectedNodes.Add(node);
               }
           }
           return connectedNodes;
       }
       
    }

}