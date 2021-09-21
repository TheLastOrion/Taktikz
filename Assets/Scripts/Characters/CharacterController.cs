using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Interfaces;
using UnityEngine;

public class CharacterController : MonoBehaviour, IMoveCapable, ICombatCapable, ISelectable
{
    // Start is called before the first frame update
    void Start()
    {
        
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
