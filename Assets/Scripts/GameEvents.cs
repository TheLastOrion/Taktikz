using System;
using Assets.Scripts.Interfaces;
using UnityEngine;

public class GameEvents
{
    public static event Action<Node> NodeSelected;
    public static event Action<CharacterBase, Node, Node> CharacterMoveStarted;
    public static event Action<CharacterBase, Node, Node> CharacterMoveCompleted;
    public static event Action<IMoveCapable, Node, Node> MoveCommandIssued;
    public static event Action<ICombatCapable, ICombatCapable> AttackStarted;
    public static event Action<ICombatCapable, ICombatCapable> AttackCompleted;
    public static event Action<ICombatCapable> CharacterDied; 
    public static event Action<MyGrid> GridInitialized;
    public static event Action GameLost; 
    public static event Action GameWon; 
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

    public static void FireMoveCommandIssued(IMoveCapable moveCapable, Node startNode, Node endNode)
    {
        if (MoveCommandIssued != null)
        {
            MoveCommandIssued(moveCapable, startNode, endNode);
        }
    }

    public static void FireAttackStarted(ICombatCapable attacker, ICombatCapable defender)
    {
        if (AttackStarted != null)
        {
            AttackStarted(attacker, defender);
        }
    }

    public static void FireAttackCompleted(ICombatCapable attacker, ICombatCapable defender)
    {
        if (AttackCompleted != null)
        {
            AttackCompleted(attacker, defender);
        }
    }

    public static void FireCharacterDied(ICombatCapable character)
    {
        if (CharacterDied != null)
        {
            CharacterDied(character);
        }
    }

    public static void FireGridInitialized(MyGrid grid)
    {
        if (GridInitialized != null)
        {
            GridInitialized(grid);
        }
    }

    public static void FireGameWon()
    {
        if (GameWon != null)
        {
            GameWon();
        }
    }

    public static void FireGameLost()
    {
        if (GameLost != null)
        {
            GameLost();
        }
    }



}
