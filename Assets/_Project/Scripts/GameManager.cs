using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public float spawnInterval = 10f; // Time between spawns
    public Vector2 spawnRange; // Range for spawning enemies

    private int _maxEnemies = 3; // Increases based on collected enemies
    private int _lastCollected = 0; // Last collected amount

    private void Start()
    {
        InvokeRepeating("trySpawnEnemy", 0f, spawnInterval); // Start spawning enemies
    }

    // Attempt to spawn an enemy, if there are less than the maximum amount
    void trySpawnEnemy()
    {
        int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        int currentCollected = CollectionPoint.getCollected();

        if (currentCollected != _lastCollected)
        {
            _lastCollected = currentCollected;
            // Increase max possible enemies based on collected enemies
            if (currentCollected <= 4)
            {
                _maxEnemies++;
            }
            else if (currentCollected > 4 && currentCollected <= 8)
            {
                _maxEnemies++;
            }
        }

        // Spawn if there are less than the maximum amount of enemies
        if (currentEnemies < _maxEnemies)
        {
            SpawnEnemy();
            Debug.Log("Enemy spawned! Current enemies: " + currentEnemies);
        }
    }

    // Spawn enemy at random position
    void SpawnEnemy()
    {
        Vector2 spawnPosition;

        // Randomly choose if spawning on vertical or horizontal borders
        if (Random.value < 0.5f)
        {
            // Spawn on left or right border
            float x = Random.value < 0.5f ? -spawnRange.x : spawnRange.x;
            float y = Random.Range(-spawnRange.y, spawnRange.y);
            spawnPosition = new Vector2(x, y);
        }
        else
        {
            // Spawn on top or bottom border
            float x = Random.Range(-spawnRange.x, spawnRange.x);
            float y = Random.value < 0.5f ? -spawnRange.y : spawnRange.y;
            spawnPosition = new Vector2(x, y);
        }

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}