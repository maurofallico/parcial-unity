using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float speed = 5f;
    public float jumpForce = 5.5f;
    public float shootCooldown = 0f;
    public bool canJump = false;
    public float move = 0f;

    Rigidbody2D rb;
    public GameObject bullet;

    void Jump()
    {
        canJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

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
        move = 0f;

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            move = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }

        
        rb.linearVelocity = new Vector2(
            move * speed,
            rb.linearVelocity.y
        );

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }

        shootCooldown += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (shootCooldown >= 0.2f)
            {
                Shoot();
                shootCooldown = 0f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // PISO NORMAL
        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = true;
        }

    }

}
