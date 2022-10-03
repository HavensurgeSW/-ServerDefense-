using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] WaypointManager waypoints;
    [SerializeField] Transform spawnPoint;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    private float spawnTimer;
    private int enemiesSpawned;
    LevelManager levelManager;

    private bool waveEnabled = false;

    // Update is called once per frame
    void Update()
    {
        if (waveEnabled)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0)
            {
                spawnTimer = levelManager.GetSpawnTimerData();
                if (enemiesSpawned < levelManager.GetEnemyCount())
                {
                    enemiesSpawned++;
                    SpawnEnemy();
                }
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = Instantiate(enemyPrefab, spawnPoint);
        newInstance.GetComponent<Enemy>().Init(waypoints.WAYPOINTS);
        newInstance.SetActive(true);
    }

    public void ToggleWave(bool b) {
        waveEnabled = b;
    }
}
