using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces
{
    public interface IMoveCapable
    {
        IEnumerator MoveToNode(List<Node> path);
    }
}
