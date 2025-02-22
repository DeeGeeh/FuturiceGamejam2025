using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;  // Reference to the UI slider
    public int maxHealth = 100;
    private int currentHealth = 100;

    // Update is called once per frame
    void Update()
    {

    }
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log(damage + " damage taken! Current health: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Player Died!");
        gameObject.SetActive(false);  // Disable player (or trigger respawn)
    }
}
