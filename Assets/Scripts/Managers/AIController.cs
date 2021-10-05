using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Interfaces;
using Enumerations;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<CharacterBase, CharacterBase> AITargetCharacterDictionary;
    private int enemyCounter;
    public static AIController Instance;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        enemyCounter = 0;

        AITargetCharacterDictionary = new Dictionary<CharacterBase, CharacterBase>();
        GameEvents.EnemiesSpawned += GameEvents_EnemiesSpawned;
        GameEvents.AlliesSpawned += GameEvents_AlliesSpawned;
        GameEvents.CharacterDied += GameEvents_CharacterDied;
        GameEvents.TurnChanged += GameEvents_TurnChanged;
        // GameEvents.AICharacterTurnStarted += GameEvents_AICharacterTurnStarted;
        GameEvents.CharacterTurnComplete += GameEvents_AICharacterTurnEnded;
    }
    private void GameEvents_AICharacterTurnStarted(ICombatCapable combater)
    {
        CharacterBase character = (CharacterBase)combater;
        MoveAICloserToTargetAndAttackIfAble(character, AITargetCharacterDictionary[character]);
    }
    private void GameEvents_AICharacterTurnEnded(IMoveCapable mover)
    {
        
        CharacterBase character = (CharacterBase)mover;

        if (character.GetPlayerType() == PlayerType.AI)
        {
            if (enemyCounter >= AITargetCharacterDictionary.Count)
            {
                GameEvents.FireAllAICharsAreFinishedActing();
            }
            else
            {
                GameEvents.FireAICharacterTurnStarted(AITargetCharacterDictionary.Keys.ElementAt(enemyCounter));
                MoveAICloserToTargetAndAttackIfAble(AITargetCharacterDictionary.Keys.ElementAt(enemyCounter), AITargetCharacterDictionary[character]);
                enemyCounter++;


            }
        }

    }



    private void GameEvents_TurnChanged(TurnType turn)
    {
        if (turn == TurnType.AITurn)
        {
            enemyCounter = 0;
            // GameEvents.FireAICharacterTurnStarted(AITargetCharacterDictionary.Keys.ElementAt(enemyCounter));
            CharacterBase character = AITargetCharacterDictionary.Keys.ElementAt(enemyCounter);
            MoveAICloserToTargetAndAttackIfAble(character, AITargetCharacterDictionary[character]);
            enemyCounter++;
        }
        else
        {
            
        }
    }

    private void GameEvents_AlliesSpawned()
    {

    }

    private void GameEvents_EnemiesSpawned()
    {
        if (UnitManager.Instance.PlayerCharacters.Count != 0)
        {
            AssignAITargets();
        }
        else
        {
            Debug.LogErrorFormat("No Player characters detected, failed to assign targets!");
        }
    }

    private void GameEvents_CharacterDied(ICombatCapable combater, Node node)
    {
        CharacterBase character = (CharacterBase) combater;
        if (character != null && character.GetPlayerType() == PlayerType.Player)
        {
            foreach (var keyValuePair in AITargetCharacterDictionary)
            {
                if (keyValuePair.Value == character)
                {
                    AssignTarget(keyValuePair.Key, UnitManager.Instance.PlayerCharacters[Random.Range(0, UnitManager.Instance.PlayerCharacters.Count)]);
                }
            }
        }
    }

    public void AssignAITargets()
    {
        foreach (var instanceAiCharacter in UnitManager.Instance.AICharacters)
        {
            AssignTarget(instanceAiCharacter, UnitManager.Instance.PlayerCharacters[Random.Range(0, UnitManager.Instance.PlayerCharacters.Count)]);
        }

       
    }

    public void AssignTarget(CharacterBase assignedCharacter, CharacterBase targetCharacter)
    {
        if (!AITargetCharacterDictionary.ContainsKey(assignedCharacter))
        {
            AITargetCharacterDictionary.Add(assignedCharacter, targetCharacter);
        }
        else
        {
            AITargetCharacterDictionary[assignedCharacter] = targetCharacter;
        }
    }

    public void MoveAICloserToTargetAndAttackIfAble(ICombatCapable aiCombater, ICombatCapable defender)
    {
        CharacterBase AIAttackerChar = (CharacterBase) aiCombater;
        CharacterBase DefendingChar = (CharacterBase) defender;
        Node attackerCurrentNode = AIAttackerChar.CurrentNode;
        Node defenderCurrentNode = DefendingChar.CurrentNode;
        GameField.Instance.ClearGridNodesForPathfinding();
        List<Node> traversedNodes =
        GameField.Instance.TraverseNeighboursAndSetNodeObjectHighlights(new List<Node> { attackerCurrentNode }, AIAttackerChar.MovementRange,true);
        Node closestNode = new Node(-1, -1, -2); // Dummy node;
        
        int min = Int32.MaxValue;
        bool hasCharacterActed = false;
        Node shortestNeighboringNodeForAttack = GameField._grid.GetNeighborWithLowestCostSingle(defenderCurrentNode.GetXCoord(), defenderCurrentNode.GetYCoord());

        foreach (var traversedNode in traversedNodes)
        {

            // if (UnitManager.Instance.CharactersByNodes.ContainsKey(traversedNode) &&
            //     UnitManager.Instance.CharactersByNodes[traversedNode] == DefendingChar)
            if (shortestNeighboringNodeForAttack != null && shortestNeighboringNodeForAttack == traversedNode)
            {
                List<Node> path = GameField.Instance.GetShortestPathToTargetNode(shortestNeighboringNodeForAttack);
                if (path.Count > 0)
                {
                    AIAttackerChar.MoveToNodeAndAttack(path, DefendingChar, attackerCurrentNode);
                    hasCharacterActed = true;
                    break;
                }
            }
            else
            {
                int distance = GameField._grid.GetDistance(defenderCurrentNode, traversedNode);
                if (distance < min)
                {
                    min = distance;
                    closestNode = traversedNode;
                }
            }
        }

        if (!hasCharacterActed)
        {
            List<Node> path = GameField.Instance.GetShortestPathToTargetNode(closestNode);
            AIAttackerChar.MoveToNode(path);
        }
        
        
        //I messed up here and happily converted it to reused code above
        
        
        
        
        // foreach (var traversedNode in traversedNodes)
        // {
        //     int distance = GameField._grid.GetDistance(defenderCurrentNode, traversedNode);
        //     if (distance < min)
        //     {
        //         min = distance;
        //         closestNode = traversedNode;
        //     }
        // }

        // if (closestNode.GetXCoord() != -1) // Check if closest node is assigned within the loop
        // {
        //     List<Node> neighbours = GameField._grid.GetNeighboringNodes(closestNode.GetXCoord(), closestNode.GetYCoord());
        //     bool hasCharActed = false;
        //     foreach (var neighbour in neighbours)
        //     {
        //         if (UnitManager.Instance.CharactersByNodes.ContainsKey(neighbour) &&
        //             UnitManager.Instance.CharactersByNodes[neighbour] == DefendingChar &&
        //             neighbour.DistanceFromSelectedNode < AIAttackerChar.MovementRange
        //         )
        //         {
        //             List<Node> path = GameField.Instance.GetShortestPathToTargetNode(neighbour);
        //
        //             AIAttackerChar.MoveToNodeAndAttack(path, DefendingChar, neighbour);
        //             hasCharActed = true;
        //         }
        //     }
        //
        //     if (!hasCharActed)
        //     {
        //         List<Node> path = GameField.Instance.GetShortestPathToTargetNode(closestNode);
        //         AIAttackerChar.MoveToNode(path);
        //     }
        //
        // }

    }

    
}
