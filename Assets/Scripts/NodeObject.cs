using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Interfaces;
using Enumerations;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class NodeObject : MonoBehaviour, ISelectable, IHighlightable, IPointerClickHandler
{
    private Node _node;
    [SerializeField]private GameObject _outlineObject;
    [SerializeField]private GameObject _highlightObject;
    [SerializeField]private TextMeshPro _coordsText;
    [SerializeField]private TextMeshPro _moveCostText;
    private HighlightType _currentHighlight;
    private MeshRenderer _outlineRenderer;
    private MeshRenderer _highlightRenderer;
    
    private void Start()
    {
        _outlineRenderer = _outlineObject.GetComponent<MeshRenderer>();
        _highlightRenderer = _highlightObject.GetComponent<MeshRenderer>();
        StringBuilder s = new StringBuilder();
        s.Append("X: ");
        s.Append(_node.GetXCoord());
        s.Append("\n Y: ");
        s.Append(_node.GetYCoord());
        _coordsText.text = s.ToString();
        s.Clear();
        this.GetComponent<BoxCollider>().size = new Vector3(1, _outlineObject.transform.localScale.y, 1);
        _currentHighlight = HighlightType.None;
    }



    void OnMouseOver()
    {
        //_outlineRenderer.material = GameField.Instance.HighlightNodeMaterial;
        Highlight(HighlightType.Hover);
    }

    void OnMouseExit()
    {
        //_outlineRenderer.material = GameField.Instance.StandardNodeMaterial;
        UnHighlight();
    }
    
    private void Update()
    {
        //if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        //{
        //    Select();
        //}
    }

    
    public Vector3 GetNodePosition()
    {
        return transform.position;
    }

    public void SetCoords(bool isOn)
    {
        _coordsText.gameObject.GetComponent<MeshRenderer>().enabled = isOn;
    }

    public void SetPathCostsVisual(bool isOn)
    {
        _moveCostText.gameObject.GetComponent<MeshRenderer>().enabled = isOn;
    }

    public void AssignPathCostText(string cost)
    {
        _moveCostText.text = cost;
    }
    public void Highlight(HighlightType highlightType)
    {
        switch (highlightType)
        {
            case HighlightType.None:
                _outlineRenderer.material = GameField.Instance.StandardNodeMaterial;
                break;
            case HighlightType.Hover:
                _highlightRenderer.enabled = true;
                break;
            case HighlightType.Attackable:
                _outlineRenderer.material = GameField.Instance.AttackableNodeMaterial;
                break;
            case HighlightType.Available:
                _outlineRenderer.material = GameField.Instance.AvailableNodeMaterial;
                break;
            case HighlightType.Blocked:
                _outlineRenderer.material = GameField.Instance.BlockedNodeMaterial;
                break;

        }
    }

    public void UnHighlight()
    {
        _highlightRenderer.enabled = false;
    }

    public ISelectable Select()
    {

        GameEvents.FireNodeSelected(_node);
        //Debug.LogFormat("Node Selected  X:{0}  Y:{1} ", _node.GetXCoord(), _node.GetYCoord());

        return this;
    }

    public void Deselect()
    {
        GameField.Instance.CurrentSelectedNode = null;
    }
    
    public void SetNode(Node node)
    {
        _node = node;
    }

    public Node GetNode()
    {
        return _node;
    }

    public CharacterBase GetCharType()
    {
        return null;
    }

    public HighlightType GetHighlightType()
    {
        return _currentHighlight;
    }

    public void SetHighlightType(HighlightType highlightType)
    {
        _currentHighlight = highlightType;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Select();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameEvents.FireMoveCommandIssued(UnitManager.Instance.CharactersByNodes[GameField.Instance.CurrentSelectedNode],GameField.Instance.CurrentSelectedNode, _node);
        }
    }
}
