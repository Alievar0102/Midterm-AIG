using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (move.sqrMagnitude > 1)
        {
            move.Normalize();
        }
        rb.linearVelocity = move * speed;
    }
}
