using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using XNode;

public class DecisionNode : BaseNode {
	
	[Input] public int entry;
	[Output(dynamicPortList: true)]public List<BaseNode> exit;
	
	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return "Decisions"; // Replace this
	}
	
	public override NodeTypes GetNodeType()
	{
		return NodeTypes.DECISION_NODE;
	}
}