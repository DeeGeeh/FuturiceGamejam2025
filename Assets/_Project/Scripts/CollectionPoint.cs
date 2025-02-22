using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPoint : MonoBehaviour
{
    public static int collected = 0;

    public static int getCollected()
    {
        return collected;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the enemy
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (!playerController.hasPrisoner)
                return;

            playerController.ReleasePrisoner();
            collected++;
        }
    }
}
