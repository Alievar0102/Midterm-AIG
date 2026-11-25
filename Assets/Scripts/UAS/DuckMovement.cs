using UnityEngine;
using UnityEngine.AI;

public class DuckMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DuckMove(Vector3 destination)
    {
        if (agent == null || !agent.isOnNavMesh) return;

        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    public void DuckStop()
    {
        if (agent == null || !agent.isOnNavMesh) return;

        agent.isStopped = true;
    }

    public bool IsDestinationReached()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}
