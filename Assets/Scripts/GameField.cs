using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameField : MonoBehaviour
{
    [SerializeField] public Material StandardNodeMaterial;
    [SerializeField] public Material HighlightNodeMaterial;
    private static MyGrid _grid;
    [SerializeField]private GameObject _nodePrefab;
    [SerializeField]private int _width;
    [SerializeField]private int _height;
    [SerializeField]private int _cellSize;
    [SerializeField]private Transform _fieldTransform;
    [SerializeField]private Transform _nodesTransform;
    private const int NODE_TILE_LAYER = 8;

    public static GameField Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _grid = new MyGrid(_width, _height, _cellSize, transform.position, _nodePrefab, _nodesTransform);
        _fieldTransform.localScale = new Vector3(_width, 1, _height);
        UIEvents.GridVisibilityChanged += UIEvents_GridVisibilityChanged;

    }

    void Update()
    {
        //if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        //{
        //    //Debug.LogFormat("Camera To World Point: {0}",Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    //if (Physics.Raycast(ray, out hit, Single.MaxValue, LayerMask.NameToLayer("NodeTile")))
        //    //{
        //    //    Transform objectHit = hit.transform;
        //    //    Debug.LogFormat("Node Name: {0}", hit.transform.gameObject.name);

        //    //}
        //    if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << NODE_TILE_LAYER))
        //    {
        //        Transform objectHit = hit.transform;
        //        //Debug.LogFormat("Node Name: {0}", hit.transform.gameObject.name);
        //        objectHit.gameObject.GetComponent<MeshRenderer>().material = HighlightNodeMaterial;

        //    }
        //}
    }

    private void UIEvents_GridVisibilityChanged(bool isVisible)
    {
        SetAllNodesVisibility(isVisible);
    }

    // Update is called once per frame

    public void SetNodeVisibility(Node node, bool isVisible)
    {
        Node[,] nodes = _grid.GetNodes();
        Vector2 coords = node.GetNodeCoords();
        nodes[(int)coords.x, (int)coords.y].SetVisible(isVisible);
    }

    public void SetAllNodesVisibility(bool isVisible)
    {
        Node[,] nodes = _grid.GetNodes();
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                nodes[i, y].SetVisible(isVisible);
            }
        }
    }

    public void HighlightNode(Node node, bool isHighlighted)
    {
        Node[,] nodes = _grid.GetNodes();
        Vector2 coords = node.GetNodeCoords();
        nodes[(int)coords.x, (int)coords.y].Highlight(isHighlighted);
    }

    public Node GetNodeFromGrid(int x, int y)
    {
        return _grid.GetNode(x, y);
    }




}
