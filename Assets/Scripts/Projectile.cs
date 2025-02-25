using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    private float damage;

    private Rigidbody2D rb;

    public void Initialize(float attackPower)
    {
        damage = attackPower;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.right * speed; // 修正 linearVelocity
        Destroy(gameObject, lifetime);
    }

    public float GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // 传递 damage 参数
            }
            Destroy(gameObject);
        }
    }
}
