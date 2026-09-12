using UnityEngine;
using System.Collections;
public abstract class Enemy : MonoBehaviour, IDamageable
{

    [SerializeField] private float hitFlashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine hitFlashCoroutine;
    [SerializeField] private int maxHealth = 1;

    [SerializeField] private float minX = -8.5f;
    [SerializeField] private float maxX = 8.5f;

    protected GameObject player;
    protected Rigidbody2D rb;

    private int health;

    public int Health => health;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        health = maxHealth;
        player = GameObject.FindWithTag("Player");
    }


    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0);

        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
        }

        hitFlashCoroutine = StartCoroutine(HitFlash());

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFlash()
    {
        spriteRenderer.color = Color.Lerp(
            originalColor,
            Color.red,
            0.5f
        );

        yield return new WaitForSeconds(hitFlashDuration);

        spriteRenderer.color = originalColor;

        hitFlashCoroutine = null;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void ClampPosition()
    {
        if (rb.position.x < minX)
        {
            rb.position = new Vector2(
                minX,
                rb.position.y
            );
        }

        if (rb.position.x > maxX)
        {
            rb.position = new Vector2(
                maxX,
                rb.position.y
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
            return;
    }



    public abstract void Attack();
}