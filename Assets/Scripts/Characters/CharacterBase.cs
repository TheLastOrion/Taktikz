using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Enumerations;
using UnityEngine;

public class CharacterBase : MonoBehaviour, IMoveCapable, ICombatCapable
{
    #region Character Variables

    [SerializeField]private int _hitPoints;
    [SerializeField]private int _movementRange;
    [SerializeField]private float _moveAnimationSpeed;
    [SerializeField]private int _baseAttackDamage;
    [SerializeField] private PlayerType _playerType;
    private Node _currentNode;

    public Node CurrentNode
    {
        get { return _currentNode; }
        set { _currentNode = value; }
    }
    #endregion

    private Animator _animatorController;

    private Node _currentResidingNode;
    // Start is called before the first frame update
    void Start()
    {
        _animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MoveToNode(List<Node> path, bool rotate = true)
    {
        _animatorController.SetBool("Run", true);
        for (int i = 0; i < path.Count;)
        {
            Vector3 targetPos =
                GameField.Instance.GetNodePosition((int)path[i].GetNodeCoords().x, (int)path[i].GetNodeCoords().y);

            if (rotate)
            {
                RotateTowards(path[i]);
            }
            if (Vector3.Distance(GameField.Instance.GetNodePosition((int)path[i].GetNodeCoords().x, (int)path[i].GetNodeCoords().y), transform.position) >=
                GeneralConstants.NODE_CENTER_DISTANCE_COMPARISON_EPSILON)
            {
                //transform.Translate(path[i].GetNodePosition().normalized * Time.deltaTime * _moveAnimationSpeed);
                transform.position = Vector3.MoveTowards(transform.position, targetPos,
                    Time.fixedDeltaTime * _moveAnimationSpeed);

                

                yield return new WaitForFixedUpdate();
            }
            else
            {
                i++;
                yield return new WaitForFixedUpdate();
            }
        }
        _animatorController.SetBool("Run", false);
    }

    public void RotateTowards(Node nextNode)
    {
        Vector3 targetPos =
            GameField.Instance.GetNodePosition((int)nextNode.GetNodeCoords().x, (int)nextNode.GetNodeCoords().y);

        Vector3 direction = targetPos - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
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
         moveCoroutine = StartCoroutine(MoveToNode(DebugList));

    }


    public void Attack(CharacterBase defendingChar)
    {
        if (defendingChar._playerType == this._playerType)
        {
            Debug.LogError("Error, can't attack the same player's character!");
            return;
        }
        else
        {
            
        }

    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }
}
