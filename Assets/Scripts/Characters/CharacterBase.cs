using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Enumerations;
using TMPro;
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
    private Quaternion _previousRotation;
    private GameObject _damageTextObject;
    private TextMeshPro _damageText;
    private bool _hasActionLeft;

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

    public bool HasActionLeft
    {
        get { return _hasActionLeft; }
        set { _hasActionLeft = value; }
    }
    public int MovementRange
    {
        get { return _movementRange; }
    }
    private Coroutine _currentCoroutine;
    private Node _currentNode;

    public Node CurrentNode
    {
        get { return _currentNode; }
        set { _currentNode = value; }
    }
    #endregion

    private Animator _animatorController;

    // Start is called before the first frame update
    void Start()
    {
        _hasActionLeft = true;
        _animatorController = GetComponent<Animator>();
        _damageTextObject = GameObject.Instantiate(GameField.Instance.DamageTextPrefab, this.gameObject.transform.position, Quaternion.Euler(0,-135,0), this.gameObject.transform);
        _damageTextObject.transform.localPosition = Vector3.one * 1.5f;
        _damageText = _damageTextObject.GetComponent<TextMeshPro>();
        _previousRotation = transform.rotation;
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

    public IEnumerator MoveToNodeCoroutine(List<Node> path, bool rotate = true, bool isMoveAndAttack = false)
    {
        GameEvents.FireCharacterMoveStarted(this, path[path.Count - 1], path[0]);

        _animatorController.SetBool("Run", true);
        for (int i = path.Count - 1; i >= 0;)
        {
            Vector3 targetPos =
                GameField.Instance.GetNodePosition(path[i].GetXCoord(), path[i].GetYCoord());

            if (rotate && _previousRotation.Equals(transform.rotation) )
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
        _currentNode = path[0];

        GameEvents.FireCharacterMoveCompleted(this, path[path.Count - 1], path[0]);
        
        if (!isMoveAndAttack)
        {
            _hasActionLeft = false;
            GameEvents.FireCharacterTurnComplete(this);
        }

    }

    public IEnumerator MoveToNodeAndAttackCoroutine(List<Node> path, ICombatCapable defender, Node defenderNode, bool rotate = true )
    {
        yield return MoveToNodeCoroutine(path, rotate, true);
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
        _previousRotation = rotation;

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
        yield return new WaitForSeconds(2);
        defendingChar.TakeDamage(_baseAttackDamage);
        Debug.LogFormat("{0} attacks {1} for {2} damage! {1} has {3} hitpoints left!",this.gameObject.name, defendingChar.gameObject.name, _baseAttackDamage, _hitPoints - _baseAttackDamage);
        _hasActionLeft = false;
        GameEvents.FireAttackCompleted(this, defender);
        GameEvents.FireCharacterTurnComplete(this);


    }

    public IEnumerator DieCoroutine()
    {
        _animatorController.SetTrigger("Die");
        yield return new WaitForSeconds(1);
        _animatorController.ResetTrigger("Die");
        GameEvents.FireCharacterDied(this, _currentNode);
        Destroy(this.gameObject);
    }

    public IEnumerator TakeDamageCoroutine(int damage)
    {
        _damageText.text = damage.ToString();
        while (_damageTextObject.transform.localPosition.y < GeneralConstants.DAMAGE_TEXT_HEIGHT_BEFORE_DISAPPEAR)
        {
            yield return new WaitForFixedUpdate();
            _damageTextObject.transform.Translate(Vector3.up * _moveAnimationSpeed * Time.fixedDeltaTime);
        }

        _damageText.text = "";
        _damageTextObject.transform.localPosition = GeneralConstants.DAMAGE_TEXT_DEFAULT_POSITION_VECTOR;
    }

    public void Die()
    {
        _currentCoroutine = StartCoroutine(DieCoroutine());
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(TakeDamageCoroutine(damage));
        _hitPoints -= damage;
        if (_hitPoints <= 0)
        {
            Die();
        }
    }
}
