using UnityEngine;

public class Node
{
    public int gridX;
    public int gridY;

    public bool IsWall;

    public Vector3 worldPosition;

    public Node ParentNode;

    public int moveCost;
    public int heuristicCost;
    public int TotalCost { get { return moveCost + heuristicCost; } }

    public Node(bool inputIsWall, Vector3 inputWorldPos, int inputX, int inputY)
    {
        IsWall = inputIsWall;
        worldPosition = inputWorldPos;
        gridX = inputX;
        gridY = inputY;
    }
}
