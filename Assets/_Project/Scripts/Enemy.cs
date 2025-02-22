using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage the enemy will deal
    public float moveSpeed = 2f; // Speed of enemy movement

    private Transform _playerTransform; // Reference to the player's transform
    private Vector2 _direction;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (_playerTransform != null)
        {
            // Calculate direction to player
            _direction = (_playerTransform.position - transform.position).normalized;

            // Move towards player
            transform.Translate(_direction * moveSpeed * Time.deltaTime);

            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Keep the boundary check
        if (transform.position.x < -10 || transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the enemy
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Player hit by enemy!");
                playerHealth.TakeDamage(damageAmount);
            }
            Destroy(gameObject); // Destroy enemy after damaging player?
        }
    }
}
