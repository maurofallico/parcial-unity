using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{

    [SerializeField] private float minX = -8.5f;
    [SerializeField] private float maxX = 8.5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5.5f;


    [SerializeField] private int maxHealth = 3;
    [SerializeField] private GameObject heart;
    [SerializeField] private Transform heartsContainer;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;


    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private float shootCooldownTime = 0.2f;


    [SerializeField] private GameObject losePanel;

    private Rigidbody2D rb;
    private Animator anim;
    
    private List<Image> hearts = new List<Image>();

    private int health;
    private float move;
    private float shootCooldown;

    public bool IsGrounded => canJump;
    private bool canJump = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>(); 
        health = maxHealth; 
        CreateHearts();
    }

    private void CreateHearts() 
    { 
        for (int i = 0; i < maxHealth; i++) 
        { 
            GameObject newHeart = Instantiate(heart, heartsContainer); 
            Image heartImage = newHeart.GetComponent<Image>(); 
            hearts.Add(heartImage); 
        } 
    }

    private void Update() 
    { 
        HandleMovementInput(); 
        HandleJumpInput(); 
        HandleShootInput(); 
        UpdateAnimations(); 
        if (health <= 0) 
        { StartCoroutine(Die()); 
        } 
    }

    private void HandleMovementInput() 
    { 
        move = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) 
        { 
            move = -1f; 
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } 
        if (Input.GetKey(KeyCode.RightArrow)) 
        { move = 1f; 
            transform.localScale = new Vector3(1f, 1f, 1f);
        } 
    }

    private void HandleJumpInput() 
    { 
        if (Input.GetKeyDown(KeyCode.Space) && canJump) 
        { 
            Jump(); 
        } 
    }

    private void HandleShootInput() 
    { 
        shootCooldown += Time.deltaTime; 
        if (Input.GetKeyDown(KeyCode.F) && shootCooldown >= shootCooldownTime) 
        { 
            Shoot(); 
            anim.SetTrigger("Shoot"); 
            shootCooldown = 0f; 
        } 
    }

    private void Jump() 
    { 
        anim.SetBool("isGrounded", false);
        canJump = false; 
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); 
    }

    private void Shoot()
    {
        float direction = transform.localScale.x;

        GameObject newBullet = bulletPool.GetBullet();

        if (newBullet == null)
            return;

        newBullet.transform.position =
            transform.position + Vector3.right * direction;

        newBullet.transform.rotation = Quaternion.identity;

        Bullet bulletScript = newBullet.GetComponent<Bullet>();

        bulletScript.ResetBullet(direction);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }

    private void LateUpdate() 
    { 
        ClampPosition(); 
    }

    private void ClampPosition() 
    { 
        if (transform.position.x < minX) 
        { 
            rb.position = new Vector2(minX, rb.position.y); 
        } 
        if (transform.position.x > maxX) 
        { 
            rb.position = new Vector2(maxX, rb.position.y); 
        } 
    }

    public void TakeDamage(int damage)
    { 
        health -= damage; 
        health = Mathf.Max(health, 0); 
        UpdateHearts(); 
    }

    private void UpdateHearts() 
    { 
        for (int i = 0; i < hearts.Count; i++) 
        { 
            hearts[i].sprite = i < health ? fullHeart : emptyHeart; 
        } 
    }

    private void UpdateAnimations() 
    { 
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x)); 
    }


    private void OnCollisionEnter2D(Collision2D collision) 
    { 
        if (collision.gameObject.CompareTag("Floor")) 
        { 
            canJump = true; 
            anim.SetBool("isGrounded", true); 
        } 
    }

    private IEnumerator Die() 
    { 
        rb.linearVelocity = Vector2.zero; 
        rb.constraints = RigidbodyConstraints2D.FreezeAll; 
        UpdateHearts(); 
        anim.SetTrigger("Death"); 
        enabled = false; 
        yield return new WaitForSeconds(1f); 
        losePanel.SetActive(true); 
        Destroy(gameObject); 
    }
}
