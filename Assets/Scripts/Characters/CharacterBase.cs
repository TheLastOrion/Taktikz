using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

public class CharacterBase : MonoBehaviour, IMoveCapable, ICombatCapable
{
    #region Character Variables

    [SerializeField]private int _hitPoints;
    [SerializeField]private int _movementRange;
    [SerializeField]private float _moveAnimationSpeed;
    [SerializeField]private int _baseAttackDamage;

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

    

    public IEnumerator MoveToNode(List<Node> path)
    {
        for (int i = 0; i < path.Count;)
        {

            if (Vector3.Distance(path[i].GetNodePosition(), transform.position) >=
                GeneralConstants.NODE_CENTER_DISTANCE_COMPARISON_EPSILON)
            {
                //transform.Translate(path[i].GetNodePosition().normalized * Time.deltaTime * _moveAnimationSpeed);
                transform.position = Vector3.MoveTowards(transform.position, path[i].GetNodePosition(),
                    Time.fixedDeltaTime * _moveAnimationSpeed);
                yield return new WaitForFixedUpdate();
            }
            else
            {
                i++;
                yield return new WaitForFixedUpdate();

            }
        }
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
    
    
}
