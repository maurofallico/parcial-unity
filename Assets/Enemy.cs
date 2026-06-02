using UnityEngine;


public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    public float jumpForce = 5.5f;
    public float jumpTime = 1.5f;
    public GameObject player;
    public float killDistance = 1.3f;

    void Jump()
    {
        jumpTime = 0;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpTime += Time.deltaTime;
        if (jumpTime >= 3)
        {
            Jump();
        }
        float distance = Vector2.Distance(
        transform.position,
        player.transform.position
    );

        if (distance <= killDistance)
        {
            Destroy(player);
        }
    }


}
