using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    public Transform playerPosition;
    public Transform seekerPosition;
    public Transform fleerPosition;

    public LayerMask wallMask;
    public Vector2 gridWorldSize;

    public float nodeRadius;
    float nodeDiameter;

    public float distanceBetweenNodes;

    Node[,] nodeGrid;

    public List<Node> seekPath;
    public List<Node> fleePath;

    int gridSizeX, gridSizeY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGrid()
    {
        nodeGrid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft +
                    (Vector3.right * (x * nodeDiameter + nodeRadius)) +
                    (Vector3.forward * (y * nodeDiameter + nodeRadius));

                bool isWall = Physics.CheckSphere(worldPoint, nodeRadius, wallMask);

                nodeGrid[x, y] = new Node(isWall, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node neighborNode)
    {
        List<Node> listNeighborNodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = neighborNode.gridX + x;
                int checkY = neighborNode.gridY + y;

                if (checkX >= 0 && checkX < gridWorldSize.x &&
                    checkY >= 0 && checkY < gridWorldSize.y) listNeighborNodes.Add(nodeGrid[checkX, checkY]);
            }
        }

        return listNeighborNodes;
    }

    public Node NodeFromWorldPoint(Vector3 vectorWorldPosition)
    {
        float xPosition = (vectorWorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float yPosition = (vectorWorldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        xPosition = Mathf.Clamp01(xPosition);
        yPosition = Mathf.Clamp01(yPosition);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPosition);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPosition);

        return nodeGrid[x, y];
    }

    private void OnDrawGizmos()
    {
        if (nodeGrid != null)
        {
            foreach (Node n in nodeGrid)
            {
                Node playerNode = NodeFromWorldPoint(playerPosition.position);
                Node seekerNode = NodeFromWorldPoint(seekerPosition.position);
                Node fleerNode = NodeFromWorldPoint(fleerPosition.position);

                if (n.IsWall) Gizmos.color = Color.gray;
                else Gizmos.color = Color.white;

                if (seekPath != null)
                {
                    if (seekPath.Contains(n)) Gizmos.color = Color.lightYellow;
                }

                if (fleePath != null)
                {
                    if (fleePath.Contains(n)) Gizmos.color = Color.lightGreen;
                }

                if (playerNode == n) Gizmos.color = Color.red;
                if (seekerNode == n) Gizmos.color = Color.yellow;
                if (fleerNode == n) Gizmos.color = Color.green;

                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - distanceBetweenNodes));
            }
        }
    }
}
