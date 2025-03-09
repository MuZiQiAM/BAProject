using System.Collections.Generic;
using DefaultNamespace.Makeover;
using UnityEngine;

namespace Makeover
{
    [NodeWidth(Width)]
    public class DecisionStoryNode : StoryNode
    {
        private const int Width = 480;
        [HideInInspector] [Input(backingValue: ShowBackingValue.Never)]
        public IStoryNode Entry;

        [HideInInspector] [Output(dynamicPortList: true)]
        public List<IStoryNode> Exits;

        public override Camera CurrentSelectedCamera()
        {
            return GetEntryNodeOrNull()?.CurrentSelectedCamera();
        }

        public override void SetCurrentSelected(Camera camera)
        {
            GetEntryNodeOrNull()?.SetCurrentSelected(camera);
        }

        private IStoryNode GetEntryNodeOrNull()
        {
            var p = GetInputPort(nameof(Entry));
            var c = p?.Connection;
            if (c?.node is IStoryNode storyNode)
            {
                return ReferenceEquals(storyNode, this) ? null : storyNode;
            }

            return null;
        }
    }
}