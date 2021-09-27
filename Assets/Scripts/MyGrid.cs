
using System.Collections.Generic;
using Enumerations;
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
        if (x < Nodes.GetLength(0) && y < Nodes.GetLength(1) && x >= 0 && y >= 0)
        {
            return Nodes[x, y];
        }
        else
        {
            Debug.LogErrorFormat("Error! Node X: {0} Y: {1} are not found in Grid!" , x, y);
            return null;
        }

    }

    public List<Node> GetNeighboringNodes(int x, int y)
    {
        List<Node> nodeList = new List<Node>();
        if (CheckNodeAvailabilityNW(GetNode(x, y)) != TileAvailabilityType.NonExistent/* && CheckNodeAvailabilityNW(GetNode(x, y)) != TileAvailabilityType.Blocked*/)
        {
            nodeList.Add(GetNode(x, y-1));
        }
        if (CheckNodeAvailabilityNE(GetNode(x, y)) != TileAvailabilityType.NonExistent/* && CheckNodeAvailabilityNE(GetNode(x, y)) != TileAvailabilityType.Blocked*/)
        {
            nodeList.Add(GetNode(x-1, y));
        }
        if (CheckNodeAvailabilitySW(GetNode(x, y)) != TileAvailabilityType.NonExistent/* && CheckNodeAvailabilitySW(GetNode(x, y)) != TileAvailabilityType.Blocked*/)
        {
            nodeList.Add(GetNode(x + 1, y));
        }
        if (CheckNodeAvailabilitySE(GetNode(x, y)) != TileAvailabilityType.NonExistent/* && CheckNodeAvailabilitySE(GetNode(x, y)) != TileAvailabilityType.Blocked*/)
        {
            nodeList.Add(GetNode(x, y +1));
        }
        
        // if (CheckNodeAvailabilityNW(GetNode(x, y)) == TileAvailabilityType.AvailableForMovement)
        // {
        //     nodeList.Add(GetNode(x, y-1));
        // }
        // if (CheckNodeAvailabilityNE(GetNode(x, y)) ==TileAvailabilityType.AvailableForMovement)
        // {
        //     nodeList.Add(GetNode(x-1, y));
        // }
        // if (CheckNodeAvailabilitySW(GetNode(x, y)) == TileAvailabilityType.AvailableForMovement)
        // {
        //     nodeList.Add(GetNode(x + 1, y));
        // }
        // if (CheckNodeAvailabilitySE(GetNode(x, y)) == TileAvailabilityType.AvailableForMovement)
        // {
        //     nodeList.Add(GetNode(x, y +1));
        // }

        return nodeList;

    }

    public Node GetNeighborWithLowestCost(int x, int y, int currentNodePathCost)
    {
        if ((CheckNodeAvailabilityNW(GetNode(x, y-1)) != TileAvailabilityType.NonExistent && CheckNodeAvailabilityNW(GetNode(x, y-1)) != TileAvailabilityType.Blocked && GetNode(x, y-1).DistanceFromSelectedNode== currentNodePathCost - 1) || GetNode(x, y - 1).DistanceFromSelectedNode == 0)
        {
            return GetNode(x, y-1);
        }
        else if ((CheckNodeAvailabilityNE(GetNode(x-1, y)) != TileAvailabilityType.NonExistent && CheckNodeAvailabilityNW(GetNode(x-1, y)) != TileAvailabilityType.Blocked && GetNode(x-1, y).DistanceFromSelectedNode== currentNodePathCost - 1) || GetNode(x - 1, y).DistanceFromSelectedNode == 0)
        {
           return GetNode(x-1, y);
        }
        else if ((CheckNodeAvailabilitySW(GetNode(x+1, y)) != TileAvailabilityType.NonExistent && CheckNodeAvailabilityNW(GetNode(x+1, y)) != TileAvailabilityType.Blocked && GetNode(x+1, y).DistanceFromSelectedNode== currentNodePathCost - 1) || GetNode(x + 1, y).DistanceFromSelectedNode == 0)
        {
            return GetNode(x + 1, y);
        }
        else if ((CheckNodeAvailabilitySE(GetNode(x, y+1)) != TileAvailabilityType.NonExistent && CheckNodeAvailabilityNW(GetNode(x, y+1)) != TileAvailabilityType.Blocked && GetNode(x, y+1).DistanceFromSelectedNode== currentNodePathCost - 1) || GetNode(x, y + 1).DistanceFromSelectedNode == 0)
        {
            return GetNode(x, y +1);
        }
        else if (currentNodePathCost == 0)
        {
            return GetNode(x, y);
        }
        // if (CheckNodeAvailabilityNW(GetNode(x, y)) == TileAvailabilityType.AvailableForMovement && GetNode(x, y-1).DistanceFromSelectedNode== currentNodePathCost - 1)
        // {
        //     return GetNode(x, y-1);
        // }
        // else if (CheckNodeAvailabilityNE(GetNode(x, y)) != TileAvailabilityType.AvailableForMovement && CheckNodeAvailabilityNW(GetNode(x, y)) != TileAvailabilityType.Blocked && GetNode(x-1, y).DistanceFromSelectedNode== currentNodePathCost - 1)
        // {
        //     return GetNode(x-1, y);
        // }
        // else if (CheckNodeAvailabilitySW(GetNode(x, y)) != TileAvailabilityType.AvailableForMovement && CheckNodeAvailabilityNW(GetNode(x, y)) != TileAvailabilityType.Blocked && GetNode(x+1, y).DistanceFromSelectedNode== currentNodePathCost - 1)
        // {
        //     return GetNode(x + 1, y);
        // }
        // else if (CheckNodeAvailabilitySE(GetNode(x, y)) != TileAvailabilityType.AvailableForMovement && CheckNodeAvailabilityNW(GetNode(x, y)) != TileAvailabilityType.Blocked && GetNode(x, y+1).DistanceFromSelectedNode== currentNodePathCost - 1)
        // {
        //     return GetNode(x, y +1);
        // }
        // else if (currentNodePathCost == 0)
        // {
        //     return GetNode(x, y);
        // }
        else
        {
            Debug.LogErrorFormat("No neighbor for node X: {0} Y:{1} has a lowest cost neighbor, likely an error occured", x, y);
            return null;
        }
    }

    public void ResetAllNodesForPathfinding()
    {
        Debug.Log("Resetting all grid nodes for pathfinding values!");
        foreach (var node in Nodes)
        {
            node.IsTraversedDuringPathfinding = false;
            node.DistanceFromSelectedNode = -1;
        }
    }
    public TileAvailabilityType CheckNodeAvailabilityNW(Node node)
    {
        if (node == null)
        {
            //Debug.LogError("Node is null!");
            return TileAvailabilityType.NonExistent;
        }

        int startX = node.GetXCoord();
        int startY = node.GetYCoord();

        if (startY - 1 < 0)
        {
            //Debug.LogFormat("Node X: {0}  Y:{1} has no NW boundary!", startX, startY);
            return TileAvailabilityType.NonExistent;
        }

        TileAvailabilityType isAvailable = GetNode(startX, startY - 1).TileAvailability;
        //Debug.LogFormat("Node X: {0}  Y:{1}  NW is {2}!", startX, startY, isAvailable ? "Available" : "TileAvailability");
        return isAvailable;
    }
    public TileAvailabilityType CheckNodeAvailabilityNE(Node node)
    {
        if (node == null)
        {
            //Debug.LogError("Node is null!");
            return TileAvailabilityType.NonExistent;
        }
        int startX = node.GetXCoord();
        int startY = node.GetYCoord();

        if (startX - 1 < 0)
        {
            //Debug.LogFormat("Node X: {0}  Y:{1} has no NE boundary!", startX, startY);
            return TileAvailabilityType.NonExistent;
        }
        TileAvailabilityType isAvailable = GetNode(startX - 1, startY).TileAvailability;
        //Debug.LogFormat("Node X: {0}  Y:{1}  NE is {2}!", startX, startY, isAvailable ? "Available" : "TileAvailability");
        return isAvailable;
    }
    public TileAvailabilityType CheckNodeAvailabilitySW(Node node)
    {
        if (node == null)
        {
            //Debug.LogError("Node is null!");
            return TileAvailabilityType.NonExistent;
        }
        int startX = node.GetXCoord();
        int startY = node.GetYCoord();

        if (startX + 1 >= Nodes.GetLength(0))
        {
            //Debug.LogFormat("Node X: {0}  Y:{1} has no SW boundary!", startX, startY);
            return TileAvailabilityType.NonExistent;
        }

        TileAvailabilityType isAvailable = GetNode(startX + 1, startY).TileAvailability;

        //Debug.LogFormat("Node X: {0}  Y:{1}  SW is {2}!", startX, startY, isAvailable ? "Available" : "TileAvailability");
        return isAvailable;
    }

    public TileAvailabilityType CheckNodeAvailabilitySE(Node node)
    {
        if (node == null)
        {
            //Debug.LogError("Node is null!");
            return TileAvailabilityType.NonExistent;
        }
        int startX = node.GetXCoord();
        int startY = node.GetYCoord();
        if (startY + 1 >= Nodes.GetLength(1))
        {
            //Debug.LogFormat("Node X: {0}  Y:{1} has no SE boundary!", startX, startY);
            return TileAvailabilityType.NonExistent;
        }
        TileAvailabilityType isAvailable = GetNode(startX, startY + 1).TileAvailability;

        //Debug.LogFormat("Node X: {0}  Y:{1}  SE is {2}!", startX, startY, isAvailable ? "Available" : "TileAvailability");
        return isAvailable;
    }

}
