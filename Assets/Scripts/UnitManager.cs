﻿
using System;
using System.Collections.Generic;
using Enumerations;
using NUnit.Framework.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Transform _unitsContainer;
    
    [SerializeField]private List<GameObject> EnemyTypes;
    [SerializeField]private List<GameObject> PlayerCharacterTypes;

    [SerializeField] private int MaxNumberOfEnemies;
    [SerializeField] private int MinNumberOfEnemies;
    [SerializeField] private int MaxNumberOfAllies;
    [SerializeField] private int MinNumberOfAllies;

    public static UnitManager Instance;
    public Dictionary<Node, CharacterBase> CharactersByNodes = new Dictionary<Node, CharacterBase>();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        GameEvents.GridInitialized += GameEvents_GridInitialized;
        UIEvents.SpawnEnemiesButtonPressed += UIEvents_SpawnEnemiesButtonPressed;
        UIEvents.SpawnAlliesButtonPressed += UIEvents_SpawnAlliesButtonPressed;
        GameEvents.CharacterMoveStarted += GameEvents_CharacterMoveStarted; ;
        GameEvents.CharacterMoveCompleted += GameEvents_CharacterMoveCompleted;
        GameEvents.MoveCommandIssued += GameEvents_MoveCommandIssued;
    }

    private void GameEvents_MoveCommandIssued(Node startNode, Node endNode)
    {
        if (GameField.Instance.GetNodeFromGrid(endNode.GetXCoord(), endNode.GetYCoord()).TileAvailability ==
            TileAvailabilityType.Blocked)
        {
            Debug.LogError("The end node for this movement is blocked!");
            return;
        }
        CharacterBase moveChar = CharactersByNodes[startNode];
        List<Node> path = GameField.Instance.GetShortestPathToTargetNode(endNode);
        moveChar.MoveToNode(path);
    }

    private void GameEvents_CharacterMoveCompleted(CharacterBase character, Node startNode, Node endNode)
    {
        CharactersByNodes[endNode] = character;
    }

    private void GameEvents_CharacterMoveStarted(CharacterBase character, Node startNode, Node endNode)
    {

    }

    private void GameEvents_GridInitialized(MyGrid myGrid)
    {
        InitializeCharactersByNodesDict(myGrid);
    }

    private void UIEvents_SpawnEnemiesButtonPressed()
    {
        SpawnEnemies(Random.Range(MinNumberOfEnemies, MaxNumberOfEnemies + 1), false);
    }

    private void UIEvents_SpawnAlliesButtonPressed()
    {
        SpawnAllies(Random.Range(MinNumberOfAllies, MaxNumberOfAllies + 1), false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeCharactersByNodesDict(MyGrid grid)
    {
        Node[,] gridNodes = grid.GetNodes();
        for (int i = 0; i < gridNodes.GetLength(0); i++)
        {
            for (int y = 0; y < gridNodes.GetLength(1); y++)
            {
                CharactersByNodes.Add(grid.GetNode(i, y), null);
            }
        }
    }
    public void SpawnEnemies(int numberOfEnemies, bool placeClose = true)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randomTryCount = 0;

            while (true)
            {
                int randomX = Random.Range(0, (int)GameField.Instance.GetGridSize().x);
                int randomY = Random.Range(0, (int)(GameField.Instance.GetGridSize().y / 2));

                if (GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.OccupiedByEnemies &&
                    GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.OccupiedByFriends &&
                    GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.Blocked)
                {
                    CharacterBase character = SpawnEnemy(GameField.Instance.GetNodeFromGrid(randomX, randomY), EnemyTypes[Random.Range(0, EnemyTypes.Count)]);
                    CharactersByNodes.Add(GameField.Instance.GetNodeFromGrid(randomX, randomY), character);
                    break;
                }
                randomTryCount++;
                if (randomTryCount > 100)
                {
                    Debug.LogErrorFormat("Too many attempts have been made to place units! Likely there are no possible places left.");
                    break;
                }
            }
        }
    }

    public void SpawnAllies(int numberOfAllies, bool placeClose = true)
    {
        for (int i = 0; i < numberOfAllies; i++)
        {
            int randomTryCount = 0;

            while (true)
            {
                int randomX = UnityEngine.Random.Range(0, (int)GameField.Instance.GetGridSize().x);
                int randomY = UnityEngine.Random.Range((int)(GameField.Instance.GetGridSize().y / 2), (int)(GameField.Instance.GetGridSize().y));

                if (GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.OccupiedByEnemies &&
                    GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.OccupiedByFriends &&
                    GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.Blocked)
                {
                    CharacterBase character = SpawnAlly(GameField.Instance.GetNodeFromGrid(randomX, randomY), PlayerCharacterTypes[Random.Range(0, PlayerCharacterTypes.Count)]);
                    CharactersByNodes.Add(GameField.Instance.GetNodeFromGrid(randomX, randomY), character);

                    break;
                }
                randomTryCount++;
                if (randomTryCount > 100)
                {
                    Debug.LogErrorFormat("Too many attempts have been made to place units! Likely there are no possible places left.");
                    break;
                }
            }
        }
    }


    public CharacterBase SpawnEnemy(Node node, GameObject enemyObject)
    {
        node.TileAvailability = TileAvailabilityType.Blocked;
        GameObject go = GameObject.Instantiate(enemyObject, GameField.Instance.GetNodeObject(node).transform.position,
            Quaternion.identity, _unitsContainer);
        CharacterBase enemyChar = go.GetComponent<CharacterBase>();
        enemyChar.SetPlayerType(PlayerType.AI);
        return enemyChar;

    }

    public CharacterBase SpawnAlly(Node node, GameObject alliedObject)
    {
        node.TileAvailability = TileAvailabilityType.Blocked;
        GameObject go = GameObject.Instantiate(alliedObject, GameField.Instance.GetNodeObject(node).transform.position,
            Quaternion.identity, _unitsContainer);
        CharacterBase alliedChar = go.GetComponent<CharacterBase>();
        alliedChar.SetPlayerType(PlayerType.Player);
        return alliedChar;
    }



}
