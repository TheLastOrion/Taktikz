using System;
using UnityEngine;

public class GameEvents
{
    public static event Action<Node> NodeSelected;
    public static event Action<CharacterBase, Node, Node> CharacterMoveStarted;
    public static event Action<CharacterBase, Node, Node> CharacterMoveCompleted;
    public static event Action<Node, Node> MoveCommandIssued;
    public static event Action<MyGrid> GridInitialized; 

    public static void FireNodeSelected(Node node)
    {
        if (NodeSelected != null)
        {
            NodeSelected(node);
        }
    }

    public static void FireCharacterMoveStarted(CharacterBase character, Node startNode, Node endNode)
    {
        if (CharacterMoveStarted != null)
        {
            CharacterMoveStarted(character, startNode, endNode);
        }
    }

    public static void FireCharacterMoveCompleted(CharacterBase character, Node startNode, Node endNode)
    {
        if (CharacterMoveCompleted != null)
        {
            CharacterMoveCompleted(character, startNode, endNode);
        }
    }

    public static void FireMoveCommandIssued(Node startNode, Node endNode)
    {
        if (MoveCommandIssued != null)
        {
            MoveCommandIssued(startNode, endNode);
        }
    }

    public static void FireGridInitialized(MyGrid grid)
    {
        if (GridInitialized != null)
        {
            GridInitialized(grid);
        }
    }



}
