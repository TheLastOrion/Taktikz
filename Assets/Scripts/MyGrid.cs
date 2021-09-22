
using UnityEngine;

public class MyGrid
{
    private Node[ , ] Nodes;
    public MyGrid(int _width, int _height, int _cellSize)
    {
        Nodes = new Node[_width, _height];
        Vector3 gridSize = GetGridTotalSize(_width, _height, _cellSize);
        for (int i = 0; i < Nodes.GetLength(0); i++)
        {
            for (int y = 0; y < Nodes.GetLength(1); y++)
            {
                Nodes[i, y] =  new Node(i, y, _cellSize);
            }
        }
    }

    


    public Vector3 GetGridTotalSize(int width, int height, int cellSize)
    {
        return new Vector3(width, height) * cellSize; 
    }
    public Node[,] GetNodes()
    {
        return Nodes;
    }

    public Node GetNode(int x, int y)
    {
        if (x < Nodes.GetLength(0) && y < Nodes.GetLength(1))
        {
            return Nodes[x, y];
        }
        else
        {
            Debug.LogErrorFormat("Error! Node X: {0} Y: {1} are not found in Grid!" , x, y);
            return null;
        }

    }
    
}
