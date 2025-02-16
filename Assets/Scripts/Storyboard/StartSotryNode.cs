using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class StartSotryNode : BaseNode {

	[Output] public NodePort exit;

	public override string GetString()
	{
		return "Start";
	}
	
	
}