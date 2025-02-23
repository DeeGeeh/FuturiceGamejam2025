using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;  // Reference to the UI slider
    public int maxHealth = 100;
    private int currentHealth;

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
        SceneManager.LoadScene("GameOverScene");
        gameObject.SetActive(false);  // Disable player (or trigger respawn
    }
}
