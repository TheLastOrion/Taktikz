﻿
using System;
using System.Collections.Generic;
using Enumerations;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Transform _unitsContainer;
    [SerializeField]private List<GameObject> EnemyTypes;
    // Start is called before the first frame update
    void Start()
    {
        UIEvents.SpawnEnemiesButtonPressed += UIEvents_SpawnEnemiesButtonPressed;
    }

    private void UIEvents_SpawnEnemiesButtonPressed()
    {
        SpawnEnemies(8, false);
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

                if (!GameField.Instance.GetNodeFromGrid(randomX, randomY).Blocked)
                {
                    SpawnEnemy(GameField.Instance.GetNodeFromGrid(randomX, randomY), EnemyTypes[0]);
                    break;
                }
            }
            


        }
    }

    public void SpawnEnemy(Node node, GameObject enemyObject)
    {
        node.Blocked = true;
        GameObject go = GameObject.Instantiate(enemyObject, GameField.Instance.GetNodeObject(node).transform.position,
            Quaternion.identity, _unitsContainer);
        CharacterBase enemyChar = go.GetComponent<CharacterBase>();
        enemyChar.CurrentNode = node;
        enemyChar.SetPlayerType(PlayerType.AI);

    }

}