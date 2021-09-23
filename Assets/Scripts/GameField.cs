﻿using System.Collections.Generic;
using System.Text;
using Enumerations;
using UnityEngine;

public class GameField : MonoBehaviour
{
    
    [SerializeField] public Material StandardNodeMaterial;
    [SerializeField] public Material HighlightNodeMaterial;
    [SerializeField] public Material AttackableNodeMaterial;
    [SerializeField] public Material BlockedNodeMaterial;
    [SerializeField] public Material AvailableNodeMaterial;

    //TODO change to private again, made public for debugging purposes
    public static MyGrid _grid;
    [SerializeField]private GameObject _nodePrefab;
    [SerializeField]private int _width;
    [SerializeField]private int _height;
    [SerializeField]private int _cellSize;
    [SerializeField]private Transform _fieldTransform;
    [SerializeField]private Transform _nodesTransform;
    private const int NODE_TILE_LAYER = 8;
    private float SPACING_OFFSET;
    public static GameField Instance;

    [SerializeField] private int DebugRange;

    private Node _currentSelectedNode;

    public Node CurrentSelectedNode
    {
        get { return _currentSelectedNode; }
        set { _currentSelectedNode = value; }
    }
    public Dictionary<Vector2, GameObject> NodeObjectDictionary = new Dictionary<Vector2, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        SPACING_OFFSET = _cellSize * 0.5f;

        if (Instance == null)
        {
            Instance = this;
        }
        _grid = new MyGrid(_width, _height, _cellSize);
        AttachNodeObjectsToGrid();
        _fieldTransform.localScale = new Vector3(_width, 1, _height);
        UIEvents.GridVisibilityChanged += UIEvents_GridVisibilityChanged;
        UIEvents.CoordsVisibilityChanged += UIEvents_CoordsVisibilityChanged; 
        UIEvents.PathCostVisibilityChanged += UIEvents_PathCostVisibilityChanged;
        GameEvents.NodeSelected += GameEvents_NodeSelected;

    }

    

    void AttachNodeObjectsToGrid()
    {
        Node[,] gridNodes = _grid.GetNodes();
        for (int i = 0; i < gridNodes.GetLength(0); i++)
        {
            for (int y = 0; y < gridNodes.GetLength(1); y++)
            {
                Vector3 pos = transform.position + new Vector3(i, 0, y) * _cellSize - new Vector3(_width, 0, _height) * 0.5f * _cellSize +
                              new Vector3(1, 0, 1) * SPACING_OFFSET;
                GameObject go = GameObject.Instantiate(_nodePrefab, pos, Quaternion.identity, _nodesTransform);
                NodeObject nodeObject = go.GetComponent<NodeObject>();
                go.transform.localScale *= _cellSize;

                StringBuilder name = new StringBuilder();

                //Give names to nodes for debug purposes
                name.AppendFormat("Node X: {0} Y: {1}", i, y);
                go.name = name.ToString();
                nodeObject.SetNode(gridNodes[i,y]);
                NodeObjectDictionary[new Vector2(i,y)] = go;
            }
        }
    }

    private void UIEvents_GridVisibilityChanged(bool isVisible)
    {
        SetAllNodesVisibility(isVisible);
    }
    private void UIEvents_PathCostVisibilityChanged(bool isVisible)
    {
        foreach (var value in NodeObjectDictionary.Values)
        {
            value.GetComponent<NodeObject>().SetPathCosts(isVisible);
        }
    }

    private void UIEvents_CoordsVisibilityChanged(bool isVisible)
    {
        foreach (var value in NodeObjectDictionary.Values)
        {
            value.GetComponent<NodeObject>().SetCoords(isVisible);
        }
    }

    public void SetAllNodesVisibility(bool isVisible)
    {

        foreach (var o in NodeObjectDictionary.Values)
        {
            o.SetActive(isVisible);
        }
    }

    private void GameEvents_NodeSelected(Node node)
    {

    }

    public void SetNodeObjectHighlights(Node node, int range)
    {   
        Node[,] gridNodes = _grid.GetNodes();

        for (int i = 0; i < range; i++)
        {
            if (_grid.CheckNodeAvailabilityNW(node) && node.GetYCoord() - 1 > 0)
            {
                NodeObjectDictionary[new Vector2(node.GetXCoord() , node.GetYCoord() - 1)].GetComponent<NodeObject>().Highlight(HighlightTypes.Available);
            }
            if (_grid.CheckNodeAvailabilityNE(node) && node.GetXCoord() - 1 > 0)
            {
                NodeObjectDictionary[new Vector2(node.GetXCoord() -1, node.GetYCoord())].GetComponent<NodeObject>().Highlight(HighlightTypes.Available);
            }
            if (_grid.CheckNodeAvailabilitySW(node) && node.GetXCoord() + 1 < _width)
            {
                NodeObjectDictionary[new Vector2(node.GetXCoord() + 1, node.GetYCoord())].GetComponent<NodeObject>().Highlight(HighlightTypes.Available);
            }
            if (_grid.CheckNodeAvailabilitySE(node) && node.GetXCoord() - 1 > 0)
            {
                NodeObjectDictionary[new Vector2(node.GetXCoord() , node.GetYCoord() + 1)].GetComponent<NodeObject>().Highlight(HighlightTypes.Available);
            }

        }

        //for (int i = 0; i < gridNodes.GetLength(0); i++)
        //{
        //    for (int j = 0; j < gridNodes.GetLength(1); j++)
        //    {

        //    }
        //}
    }

    public void HighlightDebug()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                NodeObjectDictionary[new Vector2(i,j)].GetComponent<NodeObject>().Highlight((HighlightTypes)Random.Range(2, 5));
            }
        }
    }
    public void ResetNodeObjectHighlights()
    {
        foreach (var value in NodeObjectDictionary.Values)
        {
            value.gameObject.GetComponent<NodeObject>().Highlight(HighlightTypes.None);
        }
    }

    public Vector2 GetGridSize()
    {
        return new Vector2(_width, _height);
    }
    public Node GetNodeFromGrid(int x, int y)
    {
        return _grid.GetNode(x, y);
    }

    public Vector3 GetNodePosition(int x, int y)
    {
        return NodeObjectDictionary[new Vector2(x,y)].transform.position;
    }

    public GameObject GetNodeObject(Node node)
    {
        return NodeObjectDictionary[new Vector2(node.GetXCoord(), node.GetYCoord())];
    }
}
