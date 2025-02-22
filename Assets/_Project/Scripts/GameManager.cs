using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public float spawnInterval = 10f; // Time between spawns
    public Vector2 spawnRange; // Range for spawning enemies

    private int _amountOfEnemies = 0; // Amount of enemies currently in the scene

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval); // Start spawning enemies
    }
    
    private void Update()
    {
        _amountOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length; // Update amount of enemies
        if (_amountOfEnemies <= 3)
        {
            SpawnEnemy(); // Spawn enemy if there are less than 3 enemies
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = new Vector2(spawnRange.x, Random.Range(-spawnRange.y, spawnRange.y));
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Spawn enemy at random position
    }
}
