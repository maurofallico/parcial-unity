using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    public float jumpForce = 5.5f;

    public GameObject player;
    public float killDistance = 1.3f;
    public GameObject bullet;

    public float jumpCooldown = 3f;

    public float shootDelay = 0;

    int difficultySelected = Options.difficulty;

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void Shoot()
    {
        if (shootDelay <= 0)
        {
            Instantiate(bullet, transform.position + Vector3.right * -1, Quaternion.identity).GetComponent<Bullet>().direction = -1;
            if (difficultySelected == 0)
            {
                shootDelay = 0.5f;
            }
            if (difficultySelected == 1)
            {
                shootDelay = 0.2f;
            }
            if (difficultySelected == 2)
            {
                shootDelay = 0.12f;
            }
        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (difficultySelected == 0)
        {
            shootDelay = 0.5f;
        }
        if (difficultySelected == 1)
        {
            shootDelay = 0.2f;
        }
        if (difficultySelected == 2)
        {
            shootDelay = 0.12f;
        }
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpCooldown -= Time.deltaTime;
        shootDelay -= Time.deltaTime;

        if (jumpCooldown <= 0f)
        {
            Jump();
            jumpCooldown = 4f;

        }

        if (transform.position.y >= -0.4f)
        {
            Shoot();
        }


        if (player)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= killDistance)
            {
                player.GetComponent<Player>().health = 0;
            }
        }
        
    }


}
