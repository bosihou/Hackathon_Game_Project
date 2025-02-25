using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 7f;
    public float mass = 1f;

    [Header("Combat")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackSpeed = 1f;
    public float attackForce = 10f;

    [Header("Debug Info")]
    public bool isGrounded;
    public bool isSprinting;
    private float lastAttackTime;
    private Rigidbody2D rb;
    private Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        Move();

        // Example: press F to shoot, respecting attackSpeed cooldown
        if (Input.GetKeyDown(KeyCode.F) && Time.time > lastAttackTime + (1f / attackSpeed))
        {
            Shoot();
            lastAttackTime = Time.time;
        }
    }

    // Called by GameManager after instantiating this Player,
    // or invoked in Start() if you want to apply GPT stats right away.
    public void InitializeStats(PlayerStats stats)
    {
        // Apply the formula used previously: 
        //   finalSpeed   = stats.Speed * 0.8f + 2f
        //   finalJump    = stats.JumpForce * 1.2f + 4f
        //   finalAttack  = stats.AttackSpeed * 0.4f + 1f
        
        
        
        speed = PlayerConstants.CalculateSpeed(stats.Speed);
        jumpForce = PlayerConstants.CalculateJumpForce(stats.JumpForce);
        attackSpeed = PlayerConstants.CalculateAttackSpeed(stats.AttackSpeed);
    }

    void Move()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A)) moveInput = -1f;
        if (Input.GetKey(KeyCode.D)) moveInput = 1f;

        isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isSprinting ? sprintSpeed : speed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
        bool isMoving = moveInput != 0;
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
        animator.SetBool("isJumping", true);

    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(attackForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadZone"))
        {
            GameManager.Instance.LoseHeart(); 
            Respawn();
        }
        else if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.LoseHeart();
            Respawn();
        }
        else if (other.CompareTag("NextLevelWall"))
        {
            // Example of loading next scene via SceneManager, or a custom SceneManagerController
            SceneManager.LoadScene("Level2");
        }
        else if (other.CompareTag("NextLevelWall2"))
        {
            SceneManager.LoadScene("Level3");
        }
        else if (other.CompareTag("SuccessWall"))
        {
            SceneManager.LoadScene("SuccessScene");
        }
    }

    // Called after losing a heart; moves player back to the restartPoint
    public void Respawn()
    {
        if (GameManager.Instance.hearts > 0)
        {
            transform.position = GameManager.Instance.restartPoint.position;
        }
    }
}
