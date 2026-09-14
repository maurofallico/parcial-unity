using UnityEngine;

public class FastEnemy : Enemy
{
    [SerializeField] private float speed = 3f;

    [SerializeField] private float attackCooldown = 0.7f;
    [SerializeField] private int damage = 1;

    private float attackTimer;

    private void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;

        FollowPlayer(speed * GetSpeedMultiplier());
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