using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    public float jumpForce = 2.5f;

    public GameObject player;
    public float killDistance = 1.3f;
    public GameObject bulletEnemy;

    public float jumpCooldown = 3f;

    public float shootDelay = 0;

    int difficultySelected = Options.difficulty;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;


    void Jump()
    {
        anim.SetBool("isGrounded", false);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void Shoot()
    {

        if (shootDelay <= 0)
        {
            Instantiate(bulletEnemy, transform.position + Vector3.right * -1, Quaternion.identity).GetComponent<Bullet>().direction = -1;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        anim = GetComponent<Animator>();

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

        if (transform.position.y >= 0.2f)
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

    public void FlashRed()
    {
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        spriteRenderer.color = originalColor;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // PISO NORMAL
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("isGrounded", true);
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            FlashRed();
        }
    }

    public void Knockback()
    {
        transform.position +=
            Vector3.right * 0.025f;
    }

}
