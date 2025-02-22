using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool beingSucked = false;
    public int damageAmount = 10; // Amount of damage the enemy will deal
    public float baseSpeed= 2f; // Speed of enemy movement
    public float speedIncreasePerCollection = 0.2f; 
    public float maxSpeedMultiplier = 2f;

    private Transform _playerTransform; // Reference to the player's transform
    private Vector2 _direction;
    private Rigidbody2D rb;
    private float currentSpeed = 2f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        UpdateSpeed();
    }

    void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            UpdateSpeed();

            // Move towards player
            _direction = (_playerTransform.position - transform.position).normalized;
            rb.velocity = _direction * currentSpeed;

            // Rotate towards player
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    private void UpdateSpeed()
    {
        // Get the number of collected prisoners
        int collected = CollectionPoint.getCollected();

        // Calculate speed multiplier (capped at maxSpeedMultiplier)
        float speedMultiplier = 1f + (speedIncreasePerCollection * collected);
        speedMultiplier = Mathf.Min(speedMultiplier, maxSpeedMultiplier);

        // Apply the multiplier to base speed
        currentSpeed = baseSpeed * speedMultiplier;
    }

    private void Update()
    { 
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
                other.GetComponent<PlayerController>().hasPrisoner = true;
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
