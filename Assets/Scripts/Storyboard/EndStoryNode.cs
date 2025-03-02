using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XNode;

public class EndStoryNode : BaseNode
{

	[Input] public int entry;
	
	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return "end"; 
	}
	
	public override NodeTypes GetNodeType()
	{
		return NodeTypes.END_NODE;
	}
}