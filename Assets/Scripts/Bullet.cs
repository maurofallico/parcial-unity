using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float bulletTime = 3f;
    [SerializeField] private int damage = 1;

    private Rigidbody2D rb;

    private float direction;

    public float Direction
    {
        get => direction;
        set => direction = value;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector2.right * direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable =
            collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        if (collision.gameObject.CompareTag("Floor") ||
            damageable != null)
        {
            Destroy(gameObject);
        }
    }
}
