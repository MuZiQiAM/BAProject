using System.Collections.Generic;
using DefaultNamespace.Makeover;
using UnityEngine;

namespace Makeover
{
    [NodeWidth(Width)]
    public class StartStoryNode : StoryNode
    {
        private const int Width = 392;
        [HideInInspector] public Camera selectedCamera;
        [HideInInspector] [Output(backingValue: ShowBackingValue.Never)] public IStoryNode Exit;
        public List<Player> Players = new List<Player>();
        public List<EndStoryNode> endings = new List<EndStoryNode>();
 
        public override Camera CurrentSelectedCamera()
        {
            return selectedCamera;
        }

        public override void SetCurrentSelected(Camera camera)
        {
            selectedCamera = camera;
        }
        
    }
}