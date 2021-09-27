using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces
{
    public interface IMoveCapable
    {
        void MoveToNode(List<Node> path, bool rotate);
        //IEnumerator MoveToNodeCoroutine(List<Node> path, bool rotate = true);
    }
}
