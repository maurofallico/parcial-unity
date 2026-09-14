using UnityEngine;
using System.Collections;
using System;
public abstract class Enemy : MonoBehaviour, IDamageable
{
    public event Action<Enemy> OnEnemyDeath;
    [SerializeField] private float hitFlashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine hitFlashCoroutine;
    [SerializeField] protected int maxHealth = 1;

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

        health = Mathf.RoundToInt(maxHealth * GetHealthMultiplier());

        player = GameObject.FindWithTag("Player");
    }

    protected virtual void FixedUpdate()
    {
        ClampPosition();
    }

    protected float GetHealthMultiplier()
    {
        switch (Options.difficulty)
        {
            case 0:
                return 0.8f;

            case 2:
                return 1.5f;

            default:
                return 1f;
        }
    }

    protected float GetSpeedMultiplier()
    {
        switch (Options.difficulty)
        {
            case 0:
                return 0.8f;

            case 2:
                return 1.3f;

            default:
                return 1f;
        }
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
        OnEnemyDeath?.Invoke(this);
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

    protected void FollowPlayer(float speed)
    {
        if (player == null)
            return;

        Player playerScript = player.GetComponent<Player>();

        if (playerScript == null)
            return;

        float direction = Mathf.Sign(
            player.transform.position.x - transform.position.x
        );

        if (!playerScript.IsGrounded)
        {
            direction = Mathf.Sign(rb.linearVelocity.x);
        }

        rb.linearVelocity = new Vector2(
            direction * speed,
            rb.linearVelocity.y
        );

        if (direction != 0)
        {
            Vector3 scale = transform.localScale;

            scale.x = Mathf.Abs(scale.x) * direction;

            transform.localScale = scale;
        }
    }


    public abstract void Attack();
}