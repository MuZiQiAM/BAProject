using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class StoryboardGraph : NodeGraph
{

	public BaseNode startNode;
	public BaseNode currentNode;

	private List<BaseNode> parsedNodes = new List<BaseNode>(); // Store parsed nodes



}