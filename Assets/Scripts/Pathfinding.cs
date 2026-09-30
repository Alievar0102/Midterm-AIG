using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    GridBlock gridReference;
    public Transform playerPosition;
    public Transform enemyPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridReference = GetComponent<GridBlock>();
    }

    // Update is called once per frame
    void Update()
    {
        FindPath(enemyPosition.position, playerPosition.position);
    }

    void FindPath(Vector3 startInput, Vector3 targetInput)
    {
        Node startNode = gridReference.NodeFromWorldPoint(startInput);
        Node targetNode = gridReference.NodeFromWorldPoint(targetInput);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].TotalCost <= currentNode.TotalCost)
                {
                    if (openList[i].heuristicCost < currentNode.heuristicCost) currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in gridReference.GetNeighboringNodes(currentNode))
            {
                if (neighbor.IsWall || closedList.Contains(neighbor)) continue;

                int costToNeighbor = currentNode.moveCost + GetManhattanDistance(currentNode, neighbor);

                if (costToNeighbor < neighbor.moveCost || !openList.Contains(neighbor))
                {
                    neighbor.moveCost = costToNeighbor;
                    neighbor.heuristicCost = GetManhattanDistance(neighbor, targetNode);
                    neighbor.ParentNode = currentNode;

                    if (!openList.Contains(neighbor)) openList.Add(neighbor);
                }
            }
        }
    }

    void GetFinalPath(Node startInput, Node endInput)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = endInput;

        while (currentNode != startInput)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }

        finalPath.Reverse();

        gridReference.seekPath = finalPath;
    }

    int GetManhattanDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return distanceX + distanceY;
    }
}
