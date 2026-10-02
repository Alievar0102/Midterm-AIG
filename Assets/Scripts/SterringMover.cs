using NUnit.Framework;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class SterringMover : MonoBehaviour
{
    public float maximumSpeed = 5f;
    public float maximumForce = 10f;
    public float slowingDistance = 2f;
    public float satisfactionDistance = 0.5f;

    Rigidbody rb;
    int pathIndex = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SteerTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position);
        direction.y = 0; // Ignore vertical movement
        float distance = direction.magnitude;
        if (distance < satisfactionDistance)
        {
            rb.linearVelocity *= 0.8f;
            return;
        }

        float speed = maximumSpeed;
        if(distance < slowingDistance)
        {
            speed *= distance / slowingDistance;
        }

        Vector3 desiredVelocity = direction.normalized * speed;
        Vector3 steer = desiredVelocity - rb.linearVelocity;
        steer = Vector3.ClampMagnitude(steer, maximumForce);
        steer.y = 0; // Ignore vertical movement

        rb.AddForce(steer, ForceMode.Acceleration);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maximumSpeed);

        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0;
        if (velocity.sqrMagnitude > 0.1f)
        {
            transform.forward = velocity.normalized;
        }
    }

    public void FleeFrom(Vector3 threatPosition)
    {
        Vector3 away = (transform.position - threatPosition);
        away.y = 0; // Ignore vertical movement
        if(away.sqrMagnitude < satisfactionDistance)
        {
            away = transform.forward;
        }
        SteerTowards(transform.position + away.normalized * slowingDistance);
    }

    public void FollowPath(List<Node> path)
    {
        if (path == null || path.Count == 0) return;
        pathIndex = Mathf.Clamp(pathIndex, 0, path.Count - 1);

        Vector3 targetPosition = path[pathIndex].worldPosition;
        float flatDistance = Vector2.Distance
            (new Vector2(transform.position.x, transform.position.z),
            new Vector2(targetPosition.x, targetPosition.z));

        if (flatDistance < satisfactionDistance && pathIndex < path.Count -1)
        {
            pathIndex++;
        }
        SteerTowards(path[pathIndex].worldPosition);
    }

    public void ResetPath() { pathIndex = 0; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Stop()
    {
        rb.linearVelocity *= 0.8f;
        pathIndex = 0;
    }
}
