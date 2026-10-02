using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool IsGuard;

    public Transform playerPosition;

    float fleeDistance = 15f;

    Pathfinding pathfinding;
    GridBlock gridReference;
    SterringMover steerMovement;
    Wandering wanderMovement;

    bool isWandering;
    bool isStopping;
    List<Node> currentPath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pathfinding = FindFirstObjectByType<Pathfinding>();
        gridReference = FindFirstObjectByType<GridBlock>();
        steerMovement = GetComponent<SterringMover>();
        wanderMovement = GetComponent<Wandering>();
    }

    // Update is called once per frame
    void Update()
    {
        isWandering = false;
        isStopping = false;
        currentPath = null;

        if (IsGuard) // mama duck
        {
            // find seek path
            List<Node> seekPath = pathfinding.FindPath(transform.position, playerPosition.position);

            if (seekPath != null && seekPath.Count > 0) // enemy is n unit apart from player
            {
                bool isSeeking = AI.IsSeeking(seekPath);

                if (isSeeking)
                {
                    gridReference.seekPath = seekPath;
                    currentPath = seekPath;
                }
                else
                {
                    gridReference.seekPath = null;
                    isWandering = true;
                }
            }
            else // count == 0 -> player and enemy in the same node
            {
                gridReference.seekPath = null;
                isStopping = true;
            }
        }
        else // duckling
        {
            bool isFleeing = AI.IsFleeing(pathfinding.FindPath(transform.position, playerPosition.position));

            if (isFleeing)
            {
                // find flee path
                Vector3 fleeDirection = transform.position - playerPosition.position;
                fleeDirection.Normalize();

                Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;
                Node fleeNode = FindFleeTarget(fleeTarget);

                List<Node> fleePath = pathfinding.FindPath(transform.position, fleeNode.worldPosition);

                gridReference.fleePath = fleePath;
                currentPath = fleePath;
            }
            else
            {
                gridReference.fleePath = null;
                isWandering = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isWandering)
        {
            wanderMovement.Wander();
        }
        else if (isStopping)
        {
            steerMovement.Stop();
        }
        else if (currentPath != null)
        {
            steerMovement.FollowPath(currentPath);
        }
    }

    Node FindFleeTarget(Vector3 initialTarget)
    {
        int steps = Mathf.RoundToInt(fleeDistance / gridReference.nodeDiameter);

        Node fleeNode = gridReference.NodeFromWorldPoint(initialTarget);

        if (fleeNode.IsWall)
        {
            List<Node> checkNodes = new List<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            checkNodes.Add(fleeNode);
            bool foundNode = false;

            while (!foundNode && checkNodes.Count > 0)
            {
                Node currentNode = checkNodes[0];
                checkNodes.RemoveAt(0);

                if (visited.Contains(currentNode)) continue;
                visited.Add(currentNode);

                List<Node> listNeighbor = gridReference.GetNeighboringNodes(currentNode);

                int fleeNodeDistance = 0;

                bool foundCandidate = false;

                foreach (Node n in listNeighbor)
                {
                    if (visited.Contains(n)) continue;

                    if (!n.IsWall)
                    {
                        List<Node> path = pathfinding.FindPath(n.worldPosition, playerPosition.position);

                        if (path != null)
                        {
                            int distance = path.Count;
                            if (!foundCandidate || distance > fleeNodeDistance)
                            {
                                fleeNode = n;
                                fleeNodeDistance = distance;
                                foundCandidate = true;
                            }
                        }
                    }
                    else checkNodes.Add(n);
                }

                if (foundCandidate)
                {
                    if (steps > fleeNodeDistance) checkNodes.Add(fleeNode);
                    else foundNode = true;
                }
            }
        }
        
        return fleeNode;
    }
}
