using UnityEngine;

namespace DefaultNamespace.Makeover
{
    public interface IStoryNode
    {

        Camera CurrentSelectedCamera();

        void SetCurrentSelected(Camera camera);

    }
}