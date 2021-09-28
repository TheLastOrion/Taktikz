using System.Collections.Generic;
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
    [SerializeField] public GameObject DamageTextPrefab;


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
    private int _dfsCount = 0;

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
        GameEvents.CharacterMoveCompleted += GameEvents_CharacterMoveCompleted;
        GameEvents.CharacterMoveStarted += GameEvents_CharacterMoveStarted; 
        GameEvents.FireGridInitialized(_grid);

    }

    private void GameEvents_CharacterMoveStarted(CharacterBase arg1, Node arg2, Node arg3)
    {
        ClearNodeObjectHighlights();
        ClearAllNodeObjectPathCosts();
    }

    private void GameEvents_CharacterMoveCompleted(CharacterBase arg1, Node startNode, Node endNode)
    {
        startNode.TileAvailability = TileAvailabilityType.AvailableForMovement;
        endNode.TileAvailability = TileAvailabilityType.Blocked;
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
            value.GetComponent<NodeObject>().SetPathCostsVisual(isVisible);
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
        if (UnitManager.Instance.CharactersByNodes.ContainsKey(node) &&
            UnitManager.Instance.CharactersByNodes[node].GetPlayerType() == PlayerType.Player)
        {
            Debug.LogFormat("Node Selected: X:{0}  Y:{1}", node.GetXCoord(), node.GetYCoord());
            _grid.ResetAllNodesForPathfinding();
            ClearAllNodeObjectPathCosts();
            ClearNodeObjectHighlights();
            _dfsCount = 0;
            GameField.Instance.CurrentSelectedNode = node;

            TraverseNeighboursAndSetNodeObjectHighlights(new List<Node> { node }, UnitManager.Instance.CharactersByNodes[node].MovementRange, true);
        }
    }

    private void ClearAllNodeObjectPathCosts()
    {
        foreach (var o in NodeObjectDictionary)
        {
            o.Value.GetComponent<NodeObject>().AssignPathCostText("");
        }
    }

    public void TraverseNeighboursAndSetNodeObjectHighlights(List<Node> nodes, int range, bool highlightTiles = false)
    {

        if (_dfsCount >= range)
            return;
        foreach (var node in nodes)
        {
            /// Check for dfs_count to negate blocking on home tile
            if ((!node.IsTraversedDuringPathfinding && node.TileAvailability != TileAvailabilityType.Blocked )|| _dfsCount == 0)
            {
                node.IsTraversedDuringPathfinding = true;
                node.DistanceFromSelectedNode = _dfsCount;
                NodeObjectDictionary[new Vector2(node.GetXCoord(), node.GetYCoord())].GetComponent<NodeObject>().AssignPathCostText((_dfsCount).ToString());
            }
        }
        
        List<Node> currentLevelNeighbors = new List<Node>();

        foreach (var node in nodes)
        {
            var copyList = _grid.GetNeighboringNodes(node.GetXCoord(), node.GetYCoord());
            foreach (var node1 in copyList)
            {
                if (node1.TileAvailability != TileAvailabilityType.Blocked)
                {
                    currentLevelNeighbors.Add(node1);
                }

                else if (node1.TileAvailability == TileAvailabilityType.Blocked)
                {
                    NodeObjectDictionary[new Vector2(node1.GetXCoord(), node1.GetYCoord())].GetComponent<NodeObject>().Highlight(HighlightType.Blocked);

                }
            }
        }

        foreach (var currentLevelNeighbor in currentLevelNeighbors)
        {
            if (!currentLevelNeighbor.IsTraversedDuringPathfinding && 
                (currentLevelNeighbor.TileAvailability != TileAvailabilityType.Blocked &&
                 currentLevelNeighbor.TileAvailability != TileAvailabilityType.OccupiedByEnemies &&
                 currentLevelNeighbor.TileAvailability != TileAvailabilityType.OccupiedByFriends))
            {
                currentLevelNeighbor.IsTraversedDuringPathfinding = true;
                currentLevelNeighbor.DistanceFromSelectedNode = _dfsCount + 1;
                NodeObjectDictionary[new Vector2(currentLevelNeighbor.GetXCoord(), currentLevelNeighbor.GetYCoord())].GetComponent<NodeObject>().AssignPathCostText((_dfsCount + 1).ToString() );
                if (highlightTiles)
                {
                    NodeObjectDictionary[new Vector2(currentLevelNeighbor.GetXCoord(), currentLevelNeighbor.GetYCoord())].GetComponent<NodeObject>().Highlight(HighlightType.Available);
                }


            }
            if ((currentLevelNeighbor.TileAvailability == TileAvailabilityType.Blocked ||
                currentLevelNeighbor.TileAvailability == TileAvailabilityType.OccupiedByEnemies||
                currentLevelNeighbor.TileAvailability == TileAvailabilityType.OccupiedByFriends) && highlightTiles)
            {
                NodeObjectDictionary[new Vector2(currentLevelNeighbor.GetXCoord(), currentLevelNeighbor.GetYCoord())].GetComponent<NodeObject>().Highlight(HighlightType.Blocked);
            }
        }

        _dfsCount++;
        TraverseNeighboursAndSetNodeObjectHighlights(currentLevelNeighbors, range, highlightTiles);
    }

    public List<Node> GetShortestPathToTargetNode(Node nodeEnd)
    {
        List<Node> pathList = new List<Node>();
        pathList.Add(nodeEnd);
        for (int i = nodeEnd.DistanceFromSelectedNode; i > 0; i--)
        {
            pathList.Add(_grid.GetNeighborWithLowestCostProgressive(pathList[pathList.Count -1].GetXCoord(),pathList[pathList.Count -1].GetYCoord(), i ));
        }
        

        return pathList;
    }
    
    public void HighlightDebug()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                NodeObjectDictionary[new Vector2(i,j)].GetComponent<NodeObject>().Highlight((HighlightType)Random.Range(2, 5));
            }
        }
    }
    public void ClearNodeObjectHighlights()
    {
        foreach (var value in NodeObjectDictionary.Values)
        {
            value.gameObject.GetComponent<NodeObject>().Highlight(HighlightType.None);
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
