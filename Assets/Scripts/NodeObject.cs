using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Interfaces;
using Enumerations;
using Interfaces;
using TMPro;
using UnityEngine;

public class NodeObject : MonoBehaviour, ISelectable, IHighlightable
{
    private Node _node;
    [SerializeField]private TextMeshPro _coordsText;
    [SerializeField]private TextMeshPro _moveCostText;
    
    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();

        StringBuilder s = new StringBuilder();
        s.Append("X: ");
        s.Append(_node.GetNodeCoords().x);
        s.Append("\n Y: ");
        s.Append(_node.GetNodeCoords().y);
        _coordsText.text = s.ToString();
        s.Clear();


    }

    private void Update()
    {

    }
    public Vector3 GetNodePosition()
    {
        return transform.position;
    }
    private MeshRenderer _renderer;

    public void SetVisible(bool active)
    {
        _renderer.enabled = active;
    }

    public void SetCoords(bool isOn)
    {
        _coordsText.gameObject.GetComponent<MeshRenderer>().enabled = isOn;
    }

    public void SetPathCosts(bool isOn)
    {
        _moveCostText.gameObject.GetComponent<MeshRenderer>().enabled = isOn;
    }
    public void Highlight(HighlightTypes highlightType)
    {
        _renderer.material = GameField.Instance.HighlightNodeMaterial;
    }

    public ISelectable Select()
    {
        return this;
    }

    public void Deselect()
    {

    }
    

    public void SetNode(Node node)
    {
        _node = node;
    }

    public Node GetNode()
    {
        return _node;
    }

    
}
