using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 8f;
    public float bulletTime = 3f;
    public float direction = 0f;

    TextMeshProUGUI healthText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = Vector2.right * direction * speed;
         bulletTime -= Time.deltaTime;
         if (bulletTime <= 0)
         {
             Destroy(gameObject);
         }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
            Destroy(gameObject);
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyHitBox"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            enemy.Knockback();
            enemy.FlashRed();
            Destroy(gameObject);
        }
    }
}
