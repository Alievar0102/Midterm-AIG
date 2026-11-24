using UnityEngine;

public class Wandering : MonoBehaviour
{
    public float maximumSpeed = 5f;
    public float maximumForce = 10f;
    public float wanderCooldown = 1f;
    public float wanderRange = 1.5f;

    Rigidbody rb;
    Vector3 lastDirection = Vector3.forward;
    float timer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lastDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    public void Wander()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = wanderCooldown;
            lastDirection +=
                new Vector3(Random.Range(-wanderRange, wanderRange),
                0,
                Random.Range(-wanderRange, wanderRange));
            if (lastDirection.sqrMagnitude < 0.1f)
            {
                lastDirection = transform.forward;
            }
            lastDirection.Normalize();
        }
        Vector3 desiredVelocity = lastDirection * maximumSpeed;
        Vector3 steer = desiredVelocity - rb.linearVelocity;
        steer.y= 0; // Ignore vertical movement
        steer = Vector3.ClampMagnitude(steer, maximumForce);

        rb.AddForce(steer, ForceMode.Acceleration);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maximumSpeed);

        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0;
        if (velocity.sqrMagnitude > 0.1f)
        {
            transform.forward = velocity.normalized;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetWander()
    {
        timer = 0f;
    }
}
