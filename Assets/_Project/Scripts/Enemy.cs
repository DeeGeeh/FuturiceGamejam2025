using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage the enemy will deal
    public float moveSpeed = 2f; // Speed of enemy movement

    private Transform _playerTransform; // Reference to the player's transform
    private Vector2 _direction;
    private Rigidbody2D rb;

    public bool beingSucked = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            // Move towards player
            _direction = (_playerTransform.position - transform.position).normalized;
            rb.velocity = _direction * moveSpeed;

            // Rotate towards player
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    private void Update()
    {
        // Boundary check
        if (transform.position.x < -10 || transform.position.x > 10 ||
            transform.position.y < -10 || transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the enemy
        if (other.CompareTag("Player"))
        {
            if (!beingSucked)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log("Player hit by enemy!");
                    playerHealth.TakeDamage(damageAmount);
                }
            }
            else
            {
                Debug.Log("Suck it FogBog");
            }
            
            Destroy(gameObject); // Destroy enemy after damaging player?
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Push enemies slightly apart
            Vector2 repelDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(repelDirection * 0.5f, ForceMode2D.Impulse);
        }
    }

}
