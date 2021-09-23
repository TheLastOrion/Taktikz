
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

    public bool CheckNodeAvailabilityNW(Node node)
    {
        int startX = node.GetXCoord();
        int startY = node.GetYCoord();

        if (startY - 1 < 0)
        {
            Debug.LogFormat("Node X: {0}  Y:{1} has no NW boundary!", startX, startY);
            return false;
        }
        bool isAvailable = !GetNode(startX, startY - 1).Blocked;
        Debug.LogFormat("Node X: {0}  Y:{1}  NW is {2}!", startX, startY, isAvailable ? "Available" : "Blocked");
        return isAvailable;
    }
    public bool CheckNodeAvailabilityNE(Node node)
    {
        int startX = node.GetXCoord();
        int startY = node.GetYCoord();

        if (startX - 1 < 0)
        {
            Debug.LogFormat("Node X: {0}  Y:{1} has no NE boundary!", startX, startY);
            return false;
        }
        bool isAvailable = !GetNode(startX - 1, startY).Blocked;
        Debug.LogFormat("Node X: {0}  Y:{1}  NE is {2}!", startX, startY, isAvailable ? "Available" : "Blocked");
        return isAvailable;
    }
    public bool CheckNodeAvailabilitySW(Node node)
    {
        int startX = node.GetXCoord();
        int startY = node.GetYCoord();

        if (startX + 1 >= Nodes.GetLength(0))
        {
            Debug.LogFormat("Node X: {0}  Y:{1} has no SW boundary!", startX, startY);
            return false;
        }
        bool isAvailable =  !GetNode(startX + 1, startY).Blocked;

        Debug.LogFormat("Node X: {0}  Y:{1}  SW is {2}!", startX, startY, isAvailable ? "Available" : "Blocked");
        return isAvailable;
    }

    public bool CheckNodeAvailabilitySE(Node node)
    {
        int startX = node.GetXCoord();
        int startY = node.GetYCoord();
        if (startY + 1 >= Nodes.GetLength(1))
        {
            Debug.LogFormat("Node X: {0}  Y:{1} has no SE boundary!", startX, startY);
            return false;
        }
        bool isAvailable = !GetNode(startX, startY + 1).Blocked;

        Debug.LogFormat("Node X: {0}  Y:{1}  SE is {2}!", startX, startY, isAvailable ? "Available" : "Blocked");
        return isAvailable;
    }

}
