using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5.5f;
    public float shootCooldown = 0f;

    Rigidbody2D rb;
    public GameObject bullet;

    void Shoot()
    {
        float direction = transform.localScale.x;

        Instantiate(
            bullet,
            transform.position + Vector3.right * direction,
            Quaternion.identity
        )
        .GetComponent<Bullet>()
        .direction = direction;

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        shootCooldown += Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(speed * Time.deltaTime * Vector3.left);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Time.deltaTime * Vector3.right);
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (shootCooldown >= 0.2f)
            {
                Shoot();
                shootCooldown = 0f;
            }
        }
    }
}
