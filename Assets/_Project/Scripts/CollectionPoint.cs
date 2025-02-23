using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollectionPoint : MonoBehaviour
{
    [SerializeField] private int collectionGoal = 10;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private AudioClip _collectionClip;
    [SerializeField] private AudioSource _sfxSource;

    public Sprite collectionMeter;
    //private GameObject collectionMeter;

    private Vector3 collectionMeterOriginal;

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
            _sfxSource.clip = _collectionClip;
            _sfxSource.loop = false;
            _sfxSource.Play();
            collected++;

            transform.position = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length -1)].position;
        }
    }

    private void Start(){
        //collectionMeter = GameObject.Find("CollectionMeter").GetComponent();
        collectionMeterOriginal = transform.Find("CollectionMeter").position;
    }

    private void Update()
    {
        transform.Find("CollectionMeter").localScale = new Vector3(1, 0.1f*collected, 1);
        // transform.Find("CollectionMeter").position = collectionMeterOriginal + new Vector3(0, -1 + 0.05f*collected, 0);
        if (collected >= collectionGoal){
            // Victory
            SceneManager.LoadScene("WinScreen");
        }
    }
}
