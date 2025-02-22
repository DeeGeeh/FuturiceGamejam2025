using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage the enemy will deal
    public float moveSpeed = 2f; // Speed of enemy movement

    void Update()
    {
        // Example movement behavior (can be replaced with AI)
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // Destroy enemy when it goes off-screen (optional)
        if (transform.position.x < -10) // Change -10 to your left boundary
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
                playerHealth.TakeDamage(damageAmount); // Call the TakeDamage method
            }
            Destroy(gameObject); // Destroy enemy after damaging player?
        }
    }
}
