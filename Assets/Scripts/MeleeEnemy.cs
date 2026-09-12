using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damage = 1;

    private float attackTimer;

    private void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        FollowPlayer();
        ClampPosition();
    }

    private void FollowPlayer()
    {
        float direction = Mathf.Sign(
            player.transform.position.x - transform.position.x
        );

        rb.linearVelocity = new Vector2(
            direction * speed,
            rb.linearVelocity.y
        );

        if (direction != 0)
        {
            transform.localScale = new Vector3(
                direction,
                1f,
                1f
            );
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (attackTimer > 0f)
            return;

        Attack();

        attackTimer = attackCooldown;
    }

    public override void Attack()
    {
        IDamageable damageable =
            player.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}