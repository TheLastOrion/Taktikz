using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Enumerations;
using NUnit.Framework.Api;
using UnityEngine;

public class CharacterBase : MonoBehaviour, IMoveCapable, ICombatCapable
{
    #region Character Variables

    [SerializeField]private int _hitPoints;
    [SerializeField]private int _movementRange;
    [SerializeField]private float _moveAnimationSpeed;
    [SerializeField]private int _baseAttackDamage;
    [SerializeField]private PlayerType _playerType;
    private string _characterName;
    public string CharacterName
    {
        get { return _characterName;}
        set { _characterName = value; }
    }

    public PlayerType PlayerType
    {
        get
        {
            return _playerType; 
            
        }
        set { _playerType = value; }
    }

    public int MovementRange
    {
        get { return _movementRange; }
    }
    private Coroutine _currentCoroutine;
    //private Node _currentNode;

    //public Node CurrentNode
    //{
    //    get { return _currentNode; }
    //    set { _currentNode = value; }
    //}
    #endregion

    private Animator _animatorController;

    // Start is called before the first frame update
    void Start()
    {
        _animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    public PlayerType GetPlayerType()
    {
        return _playerType;
    }

    public void SetPlayerType(PlayerType playerType)
    {
        _playerType = playerType;
    }

    public void DebugMove()
    {
        Coroutine moveCoroutine;
         List<Node> DebugList = new List<Node>
        {
            GameField.Instance.GetNodeFromGrid(0, 0),
            GameField.Instance.GetNodeFromGrid(0, 1),
            GameField.Instance.GetNodeFromGrid(0, 2),
            GameField.Instance.GetNodeFromGrid(0, 3),
            GameField.Instance.GetNodeFromGrid(0, 4),
        };
         moveCoroutine = StartCoroutine(MoveToNodeCoroutine(DebugList));

    }
    public void MoveToNodeAndAttack(List<Node> path, ICombatCapable defender, Node defenderNode, bool rotate = true)
    {
        _currentCoroutine = StartCoroutine(MoveToNodeAndAttackCoroutine(path, defender, defenderNode, rotate));
    }

    public void MoveToNode(List<Node> path, bool rotate = true)
    {
        _currentCoroutine = StartCoroutine(MoveToNodeCoroutine(path, rotate));
    }
    public IEnumerator MoveToNodeCoroutine(List<Node> path, bool rotate = true)
    {
        GameEvents.FireCharacterMoveStarted(this, path[path.Count - 1], path[0]);

        _animatorController.SetBool("Run", true);
        for (int i = path.Count - 1; i >= 0;)
        {
            Vector3 targetPos =
                GameField.Instance.GetNodePosition(path[i].GetXCoord(), path[i].GetYCoord());

            if (rotate )
            {
                RotateTowards(path[i]);
            }
            if (Vector3.Distance(GameField.Instance.GetNodePosition(path[i].GetXCoord(), path[i].GetYCoord()), transform.position) >=
                GeneralConstants.NODE_CENTER_DISTANCE_COMPARISON_EPSILON)
            {
                //transform.Translate(path[i].GetNodePosition().normalized * Time.deltaTime * _moveAnimationSpeed);
                transform.position = Vector3.MoveTowards(transform.position, targetPos,
                    Time.fixedDeltaTime * _moveAnimationSpeed);



                yield return new WaitForFixedUpdate();
            }
            else
            {
                i--;
                yield return new WaitForFixedUpdate();
            }
        }
        _animatorController.SetBool("Run", false);
        GameEvents.FireCharacterMoveCompleted(this, path[path.Count - 1], path[0]);

    }

    public IEnumerator MoveToNodeAndAttackCoroutine(List<Node> path, ICombatCapable defender, Node defenderNode, bool rotate = true)
    {
        yield return MoveToNodeCoroutine(path, rotate);
        if (rotate)
        {
            RotateTowards(defenderNode);
        }
        yield return AttackCoroutine(defender);
    }
    


    public void RotateTowards(Node nextNode)
    {
        Vector3 targetPos =
            GameField.Instance.GetNodePosition(nextNode.GetXCoord(), nextNode.GetYCoord());

        Vector3 direction = targetPos - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }



    public void Attack(ICombatCapable defender)
    {
        CharacterBase defendingChar = (CharacterBase) defender;

        if (defendingChar.GetPlayerType() == this.GetPlayerType())
        {
            Debug.LogError("Error, can't attack the same player's character!");
            return;
        }
        else
        {
            _currentCoroutine = StartCoroutine(AttackCoroutine(defendingChar));
        }

    }

    public IEnumerator AttackCoroutine(ICombatCapable defender)
    {
        GameEvents.FireAttackStarted(this, defender);
        CharacterBase defendingChar = (CharacterBase)defender;

        _animatorController.SetTrigger("Melee Right Attack 01");
        yield return new WaitForSeconds(1);
        defendingChar.TakeDamage(_baseAttackDamage);
        Debug.LogFormat("{0} attacks {1} for {2} damage! {1} has {3} hitpoints left!",this.gameObject.name, defendingChar.gameObject.name, _baseAttackDamage, _hitPoints - _baseAttackDamage);
        GameEvents.FireAttackCompleted(this, defender);

    }

    public IEnumerator DieCoroutine()
    {
        _animatorController.SetTrigger("Die");
        yield return new WaitForSeconds(1);
        _animatorController.ResetTrigger("Die");
        GameEvents.FireCharacterDied(this);
        Destroy(this.gameObject);
    }

    public void Die()
    {
        _currentCoroutine = StartCoroutine(DieCoroutine());
    }

    public void TakeDamage(int damage)
    {
        _hitPoints -= damage;
        if (_hitPoints <= 0)
        {
            Die();
        }
    }
}
