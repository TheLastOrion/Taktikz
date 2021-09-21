
using System.Collections;

namespace Assets.Scripts.Interfaces
{
    public interface IMoveCapable
    {
        IEnumerator MoveToNode(Node node);
        IEnumerator MoveToNode(int x, int y);


    }
}
