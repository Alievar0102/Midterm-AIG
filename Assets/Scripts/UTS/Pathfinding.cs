using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    GridBlock gridReference;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridReference = GetComponent<GridBlock>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Node> FindPath(Vector3 startInput, Vector3 targetInput)
    {
        gridReference.ResetNodes();

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

            if (currentNode == targetNode) return GetFinalPath(startNode, targetNode);

            foreach (Node neighbor in gridReference.GetNeighboringNodes(currentNode))
            {
                if ((neighbor.IsWall && neighbor != targetNode) || closedList.Contains(neighbor)) continue;

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

        return null;
    }

    List<Node> GetFinalPath(Node startInput, Node endInput)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = endInput;

        while (currentNode != startInput)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }

        finalPath.Reverse();

        return finalPath;
    }

    int GetManhattanDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return distanceX + distanceY;
    }
}
