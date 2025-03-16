using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Makeover;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using XNode;

namespace XNode.NodeGroups {
	public enum GroupType {
		COMBAT,
		TRAVERSAL,
		EXPLORE
	}

	[CreateNodeMenu("node group")]
	public class NodeGroup : Node {
		public int width = 400;
		public int height = 400;
		public GroupType groupType = GroupType.COMBAT;
		private static readonly Color CombatOriginalColor = ColorConverter.HexToColor("B9375E");
		private static readonly Color CombatColor = new Color(CombatOriginalColor.r, CombatOriginalColor.g, CombatOriginalColor.b, 0.3f);
		private static readonly Color TravelOriginalColor = ColorConverter.HexToColor("C6BADE");
		private static readonly Color TravelColor = new Color(TravelOriginalColor.r, TravelOriginalColor.g, TravelOriginalColor.b, 0.3f);
		private static readonly Color ExploreOriginalColor = ColorConverter.HexToColor("CEDDBB");
		private static readonly Color ExploreColor = new Color(ExploreOriginalColor.r, ExploreOriginalColor.g, ExploreOriginalColor.b, 0.3f);

		

		public Color GetGroupColor() {
			switch (groupType) {
				case GroupType.COMBAT: return CombatColor;
				case GroupType.TRAVERSAL: return TravelColor;
				case GroupType.EXPLORE: return ExploreColor;
				default: return Color.gray;
			}
		}

		public override object GetValue(NodePort port) => null;

		public List<Node> GetNodes() {
			List<Node> result = new List<Node>();
			foreach (Node node in graph.nodes) {
				if (node == this || node == null) continue;
				if (IsNodeWithinBounds(node)) result.Add(node);
			}
			return result;
		}

		private bool IsNodeWithinBounds(Node node) {
			return node.position.x >= position.x &&
			       node.position.y >= position.y &&
			       node.position.x <= position.x + width &&
			       node.position.y <= position.y + height + 30;
		}
	}
}