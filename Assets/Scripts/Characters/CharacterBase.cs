using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Interfaces;
using UnityEngine;

public class CharacterBase : MonoBehaviour, IMoveCapable, ICombatCapable, ISelectable
{
    #region Character Variables

    [SerializeField]private int _hitPoints;
    [SerializeField]private int _movementRange;
    [SerializeField]private float _moveAnimationSpeed;
    [SerializeField] private int _baseAttackDamage;

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

    public IEnumerator MoveToNode(Node node)
    {
        yield return null;
    }

    public IEnumerator MoveToNode(int x, int y)
    {
        yield return null;
    }

    public ISelectable Select()
    {
        return this;
    }

    public void Deselect()
    {

    }
}
