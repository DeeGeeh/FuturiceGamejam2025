using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public float spawnInterval = 10f; // Time between spawns
    public Vector2 spawnRange; // Range for spawning enemies

    private const int _maxEnemies = 3;

    private void Start()
    {
        InvokeRepeating("trySpawnEnemy", 0f, spawnInterval); // Start spawning enemies
    }

    // Attempt to spawn an enemy, if there are less than the maximum amount
    void trySpawnEnemy()
    {
        int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (currentEnemies < _maxEnemies)
        {
            SpawnEnemy();
            Debug.Log("Enemy spawned! Current enemies: " + currentEnemies);
        }
    }

    // Spawn enemy at random position
    void SpawnEnemy()
    {
        float randomY = Random.Range(-spawnRange.y, spawnRange.y);
        float randomX = Random.Range(-spawnRange.x, spawnRange.x);
        Vector2 spawnPosition = new Vector2(randomX, randomY);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); 
    }
}
