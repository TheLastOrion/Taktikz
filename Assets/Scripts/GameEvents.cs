using System;

public class GameEvents
{
    public static event Action<Node> NodeSelected;


    public static void FireNodeSelected(Node node)
    {
        if (NodeSelected != null)
        {
            NodeSelected(node);
        }
    }



}
