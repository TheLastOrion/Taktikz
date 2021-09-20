using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    private static MyGrid _grid;
    [SerializeField]private GameObject _nodePrefab;
    [SerializeField]private int _width;
    [SerializeField]private int _height;
    [SerializeField]private int _cellSize;
    
    // Start is called before the first frame update
    void Start()
    {
        _grid = new MyGrid(_width, _height, _cellSize, transform.position, _nodePrefab, transform);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNodeVisibility(bool isVisible)
    {
        Node[,] nodes = _grid.GetNodes();
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                nodes[i,y].Highlight(isVisible);
            }
        }
    }
}
