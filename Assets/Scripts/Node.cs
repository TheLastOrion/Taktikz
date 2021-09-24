using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Interfaces;
using Enumerations;
using Interfaces;
using UnityEngine;

public class Node
{
    private int _x;
    private int _y;
    private int _cellSize;
    private int _distanceFromSelectedNode;
    private bool _isTraversedDuringPathfinding;

    public int DistanceFromSelectedNode
    {
        get { return _distanceFromSelectedNode;}
        set { _distanceFromSelectedNode = value; }
    }

    public bool IsTraversedDuringPathfinding
    {
        get { return _isTraversedDuringPathfinding; }
        set { _isTraversedDuringPathfinding = value; }
    }
    private TileAvailabilityType _isTileAvailability = TileAvailabilityType.OutOfRange;
    public TileAvailabilityType TileAvailability {
        get
        {
            return _isTileAvailability;
        } set
        {
            _isTileAvailability = value;
        }

    }

    public Node(int x, int y, int cellSize, TileAvailabilityType isTileAvailability = TileAvailabilityType.OutOfRange)
    {
        _isTileAvailability = isTileAvailability;
        _x = x;
        _y = y;
        _cellSize = cellSize;
        _distanceFromSelectedNode = -1;
        _isTraversedDuringPathfinding = false;
    }


    /// <summary>
    /// Originally preceded GetXCoord and GetYCoord methods,
    /// I've decided to deprecate it and use cleaner methods
    /// without creating new Vector2 overheads
    /// </summary>
    /// <returns></returns>
    public Vector2 GetNodeCoords()
    {
        return new Vector2(_x, _y);
    }

    public int GetXCoord()
    {
        return _x;
    }

    public int GetYCoord()
    {
        return _y;
    }
}
