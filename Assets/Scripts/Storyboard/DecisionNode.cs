using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using XNode;
[NodeWidth(450)]
public class DecisionNode : BaseNode {
	
	[HideInInspector][Input] public int entry;
	[HideInInspector][Output(dynamicPortList: true)]public List<BaseNode> exit;


	public override object GetValue(NodePort port) {
		return "Decisions"; // Replace this
	}
	
	public override NodeTypes GetNodeType()
	{
		return NodeTypes.DECISION_NODE;
	}
}