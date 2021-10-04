using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Interfaces;
using Enumerations;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<CharacterBase, CharacterBase> AITargetCharacterDictionary;
    private int enemyCounter;
    public static AIController Instance;
    void Start()
    {
        enemyCounter = 0;
        if (Instance == null)
        {
            Instance = this;
        }
        AITargetCharacterDictionary = new Dictionary<CharacterBase, CharacterBase>();
        GameEvents.EnemiesSpawned += GameEvents_EnemiesSpawned;
        GameEvents.AlliesSpawned += GameEvents_AlliesSpawned;
        GameEvents.CharacterDied += GameEvents_CharacterDied;
        GameEvents.TurnChanged += GameEvents_TurnChanged;
        GameEvents.AICharacterTurnStarted += GameEvents_AICharacterTurnStarted;
        GameEvents.AICharacterTurnEnded += GameEvents_AICharacterTurnEnded;
    }
    private void GameEvents_AICharacterTurnStarted(ICombatCapable combater)
    {
        CharacterBase character = (CharacterBase)combater;
        MoveAICloserToTargetAndAttackIfAble(character, AITargetCharacterDictionary[character]);
    }
    private void GameEvents_AICharacterTurnEnded(ICombatCapable combater)
    {
        if (enemyCounter >= AITargetCharacterDictionary.Count)
        {
            GameEvents.FireAllAICharsAreFinishedActing();
        }
        else
        {
            CharacterBase character = (CharacterBase)combater;
            GameEvents.FireAICharacterTurnStarted(AITargetCharacterDictionary.Keys.ElementAt(enemyCounter));
            enemyCounter++;
        }
        
    }



    private void GameEvents_TurnChanged(TurnType turn)
    {
        if (turn == TurnType.AITurn)
        {
            GameEvents.FireAICharacterTurnStarted(AITargetCharacterDictionary.Keys.ElementAt(enemyCounter));
            enemyCounter++;
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

    }
}
