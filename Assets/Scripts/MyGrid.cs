using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Playables;

public class MyGrid
{
    private readonly float SPACING_OFFSET;
    private Node[ , ] Nodes;
    public MyGrid(int _width, int _height, int _cellSize, Vector3 gridObjectPos, GameObject outline, Transform parent)
    {
        SPACING_OFFSET = _cellSize * 0.5f;
        Nodes = new Node[_width, _height];
        Vector3 gridSize = GetGridSize(_width, _height, _cellSize);
        for (int i = 0; i < Nodes.GetLength(0); i++)
        {
            for (int y = 0; y < Nodes.GetLength(1); y++)
            {
                Vector3 pos = gridObjectPos + new Vector3(i, 0, y) * _cellSize - new Vector3(gridSize.x, 0, gridSize.y) * 0.5f +
                              new Vector3(1, 0, 1) * SPACING_OFFSET;
                Nodes[i, y] =  new Node(i, y, _cellSize, outline, pos, parent);
            }
        }
    }

    public Vector3 GetGridSize(int width, int height, int cellSize)
    {
        return new Vector3(width, height) * cellSize; 
    }

    public Node[,] GetNodes()
    {
        return Nodes;
    }
    
}
