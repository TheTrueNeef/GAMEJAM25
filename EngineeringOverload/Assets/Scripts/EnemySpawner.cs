using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    List <Transform> spawnPoints = new List<Transform>();

    float spawnTimer = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0.0f)
        {
            spawnTimer = 1;
            spawnEnemy();
        }
    }

    void spawnEnemy()
    {
        Transform spawnTransform = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        Instantiate(Enemy, spawnTransform.position, spawnTransform.rotation);
    }
}
