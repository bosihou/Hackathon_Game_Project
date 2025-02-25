using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 2; 
    public float speed = 2f; 
    public bool isVertical = false; 
    public float moveRange = 3f; 
    private bool movingUp = true;
    private Vector2 startPos;

    public GameObject deathEffect; 

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (isVertical)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * speed) * moveRange;
            transform.position = new Vector2(transform.position.x, newY);
        }
        else
        {
            float direction = movingUp ? 1 : -1;
            transform.position += new Vector3(speed * Time.deltaTime * direction, 0, 0);

            if (Mathf.Abs(transform.position.x - startPos.x) >= moveRange)
            {
                movingUp = !movingUp;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= (int)damage; 
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile != null)
            {
                TakeDamage(projectile.GetDamage()); 
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player")) 
        {
            Debug.Log("Player touched Enemy! Losing a heart...");
            GameManager.Instance.LoseHeart();
            other.GetComponent<PlayerController>().Respawn();
        }
    }


}
