
using System;
using System.Collections.Generic;
using Enumerations;
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

    
    // Start is called before the first frame update
    void Start()
    {
        UIEvents.SpawnEnemiesButtonPressed += UIEvents_SpawnEnemiesButtonPressed;
        UIEvents.SpawnAlliesButtonPressed += UIEvents_SpawnAlliesButtonPressed;
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

    public void SpawnEnemies(int numberOfEnemies, bool placeClose = true)
    {
        

        for (int i = 0; i < numberOfEnemies; i++)
        {
            while (true)
            {
                int randomX = UnityEngine.Random.Range(0, (int)GameField.Instance.GetGridSize().x);
                int randomY = UnityEngine.Random.Range(0, (int)(GameField.Instance.GetGridSize().y / 2));

                if (GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.OccupiedByEnemies ||
                    GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.OccupiedByFriends ||
                    GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.Blocked)
                {
                    SpawnEnemy(GameField.Instance.GetNodeFromGrid(randomX, randomY), EnemyTypes[Random.Range(0, EnemyTypes.Count)]);
                    break;
                }
            }
            


        }
    }

    public void SpawnAllies(int numberOfAllies, bool placeClose = true)
    {


        for (int i = 0; i < numberOfAllies; i++)
        {
            while (true)
            {
                int randomX = UnityEngine.Random.Range(0, (int)GameField.Instance.GetGridSize().x);
                int randomY = UnityEngine.Random.Range((int)(GameField.Instance.GetGridSize().y / 2), (int)(GameField.Instance.GetGridSize().y));

                if (GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.OccupiedByEnemies ||
                    GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.OccupiedByFriends ||
                    GameField.Instance.GetNodeFromGrid(randomX, randomY).TileAvailability != TileAvailabilityType.Blocked)
                {
                    SpawnEnemy(GameField.Instance.GetNodeFromGrid(randomX, randomY), PlayerCharacterTypes[Random.Range(0, EnemyTypes.Count)]);
                    break;
                }
            }



        }
    }


    public void SpawnEnemy(Node node, GameObject enemyObject)
    {
        node.TileAvailability = TileAvailabilityType.Blocked;
        GameObject go = GameObject.Instantiate(enemyObject, GameField.Instance.GetNodeObject(node).transform.position,
            Quaternion.identity, _unitsContainer);
        CharacterBase enemyChar = go.GetComponent<CharacterBase>();
        enemyChar.CurrentNode = node;
        enemyChar.SetPlayerType(PlayerType.AI);

    }

    public void SpawnAlly(Node node, GameObject alliedObject)
    {
        node.TileAvailability = TileAvailabilityType.Blocked;
        GameObject go = GameObject.Instantiate(alliedObject, GameField.Instance.GetNodeObject(node).transform.position,
            Quaternion.identity, _unitsContainer);
        CharacterBase alliedChar = go.GetComponent<CharacterBase>();
        alliedChar.CurrentNode = node;
        alliedChar.SetPlayerType(PlayerType.Player);
    }

}
