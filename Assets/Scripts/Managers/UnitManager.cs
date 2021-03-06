
using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
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
    public List<CharacterBase> PlayerCharacters = new List<CharacterBase>();
    public List<CharacterBase> AICharacters = new List<CharacterBase>();


    private int enemyCharacterCount;

    private int alliedCharacterCount;
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
        UIEvents.SkipUnitTurnButtonPressed += UIEvents_SkipUnitTurnButtonPressed;

        GameEvents.CharacterMoveStarted += GameEvents_CharacterMoveStarted;
        GameEvents.CharacterMoveCompleted += GameEvents_CharacterMoveCompleted;
        GameEvents.CharacterTurnComplete += GameEvents_CharacterTurnComplete;
        GameEvents.MoveCommandIssued += GameEvents_MoveCommandIssued;
        GameEvents.CharacterDied += GameEvents_CharacterDied;
        GameEvents.TurnChanged += GameEvents_TurnChanged;
        
    }

    private void GameEvents_TurnChanged(TurnType turn)
    {
        foreach (var playerCharacter in PlayerCharacters)
        {
            playerCharacter.HasActionLeft = true;
        }
        foreach (var aiCharacter in AICharacters)
        {
            aiCharacter.HasActionLeft = true;
        }
            
    }


    private void UIEvents_SkipUnitTurnButtonPressed()
    {
        if (GameField.Instance.CurrentSelectedNode != null &&
            CharactersByNodes.ContainsKey(GameField.Instance.CurrentSelectedNode))
            CharactersByNodes[GameField.Instance.CurrentSelectedNode].HasActionLeft = false;
        Debug.LogFormat("Skipped turn for {0}", CharactersByNodes[GameField.Instance.CurrentSelectedNode].gameObject.name);
    }
    private void GameEvents_CharacterDied(ICombatCapable combater, Node node)
    {
        CharacterBase character = (CharacterBase) combater;
        if (character != null)
        {
            if (character.PlayerType == PlayerType.Player)
            {
                alliedCharacterCount--;
            }
            else
            {
                enemyCharacterCount--;
            }

            if (character.GetPlayerType() == PlayerType.AI)
            {
                AICharacters.Remove(character);
            }
            else if (character.GetPlayerType() == PlayerType.Player)
            {
                PlayerCharacters.Remove(character);
            }
        }

        if (enemyCharacterCount == 0)
        {
            GameEvents.FireGameWon();
        }
        else if (alliedCharacterCount == 0)
        {
            GameEvents.FireGameLost();
        }
    }

    private void GameEvents_MoveCommandIssued(IMoveCapable moveCapable, Node startNode, Node endNode)
    {
        CharacterBase movingChar = (CharacterBase)moveCapable;
        if (GameField.Instance.GetNodeFromGrid(endNode.GetXCoord(), endNode.GetYCoord()).TileAvailability ==
            TileAvailabilityType.Blocked)
        {
            if (CharactersByNodes.ContainsKey(
                    GameField.Instance.GetNodeFromGrid(endNode.GetXCoord(), endNode.GetYCoord())) &&
                CharactersByNodes[GameField.Instance.GetNodeFromGrid(endNode.GetXCoord(), endNode.GetYCoord())]
                    .GetPlayerType() != movingChar.GetPlayerType())
            {
                
                Node shortestNeighboringNodeForAttack = GameField._grid.GetNeighborWithLowestCostSingle(endNode.GetXCoord(), endNode.GetYCoord());
                // CharacterBase moveChar = CharactersByNodes[startNode];
                CharacterBase blockingChar;
                if (CharactersByNodes.ContainsKey(endNode))
                {
                    blockingChar = CharactersByNodes[endNode];
                }
                else
                {
                    Debug.LogErrorFormat("Defending Character not found, likely a bug!");
                    return;
                }
                List<Node> path = GameField.Instance.GetShortestPathToTargetNode(shortestNeighboringNodeForAttack);
                if (path.Count > 0)
                {
                    //path.RemoveAt(path.Count-1);
                    //movingChar.MoveToNode(path);
                    //CharactersByNodes.Remove(startNode);
                    //movingChar.Attack(blockingChar);
                    movingChar.MoveToNodeAndAttack(path, blockingChar, endNode);
                }
            }
            else
            {
                Debug.LogError("The end node for this movement is blocked! Likely by a creature of the same type as mover");
                return;
            }
            
        }
        else
        {
            CharacterBase moveChar = CharactersByNodes[startNode];
            List<Node> path = GameField.Instance.GetShortestPathToTargetNode(endNode);
            moveChar.MoveToNode(path);
            //CharactersByNodes.Remove(startNode);

        }


    }

    private void GameEvents_CharacterMoveCompleted(IMoveCapable mover, Node startNode, Node endNode)
    {
        CharacterBase movingCharacter = (CharacterBase) mover;
        CharactersByNodes.Remove(startNode);
        CharactersByNodes[endNode] = movingCharacter;

        
    }
    
    private void GameEvents_CharacterTurnComplete(IMoveCapable mover)
    {
        CharacterBase movingCharacter = (CharacterBase) mover;

        if (movingCharacter.PlayerType == PlayerType.Player)
        {
            bool isPlayerTurnComplete = true;
            foreach (var playerCharacter in PlayerCharacters)
            {
                if (playerCharacter.HasActionLeft)
                {
                    isPlayerTurnComplete = false;
                }
                
            }

            if (isPlayerTurnComplete)
            {
                GameEvents.FireAllPlayerCharsAreFinishedActing();
            }
        }
        else
        {
            bool isAITurnComplete = true;
            foreach (var characterBase in AICharacters)
            {
                if (characterBase.HasActionLeft)
                {
                    isAITurnComplete = false;
                }
            }

            if (isAITurnComplete)
            {
                GameEvents.FireAllAICharsAreFinishedActing();
            }
        }
    }

    private void GameEvents_CharacterMoveStarted(IMoveCapable character, Node startNode, Node endNode)
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
        GameEvents.FireEnemiesSpawned();
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
        GameEvents.FireAlliesSpawned();

    }


    public CharacterBase SpawnEnemy(Node node, GameObject enemyObject)
    {
        node.TileAvailability = TileAvailabilityType.Blocked;
        GameObject go = GameObject.Instantiate(enemyObject, GameField.Instance.GetNodeObject(node).transform.position,
            Quaternion.identity, _unitsContainer);
        CharacterBase enemyChar = go.GetComponent<CharacterBase>();
        enemyChar.SetPlayerType(PlayerType.AI);
        enemyChar.CurrentNode = node;
        enemyCharacterCount++;
        AICharacters.Add(enemyChar);
        return enemyChar;

    }

    public CharacterBase SpawnAlly(Node node, GameObject alliedObject)
    {
        node.TileAvailability = TileAvailabilityType.Blocked;
        GameObject go = GameObject.Instantiate(alliedObject, GameField.Instance.GetNodeObject(node).transform.position,
            Quaternion.identity, _unitsContainer);
        CharacterBase alliedChar = go.GetComponent<CharacterBase>();
        alliedChar.SetPlayerType(PlayerType.Player);
        alliedChar.CurrentNode = node;
        alliedCharacterCount++;
        PlayerCharacters.Add(alliedChar);
        return alliedChar;
    }



}
