using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] WaypointManager waypoints;
    [SerializeField] Transform spawnPoint;

    [SerializeField]LevelManager levelManager;
    private float spawnTimer;
    private int enemiesSpawned;

    private bool waveEnabled = false;


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
                    Debug.Log(enemiesSpawned);
                    SpawnEnemy();
                }
                else if (enemiesSpawned == levelManager.GetEnemyCount())
                {
                    ToggleWave(false);
                    enemiesSpawned = 0;
                    levelManager.OnWaveEnd?.Invoke();
                }                
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = Instantiate(levelManager.GetEnemyPrefab(), spawnPoint);
        newInstance.GetComponent<Enemy>().Init(waypoints.WAYPOINTS);
        newInstance.SetActive(true);
    }

    public void ToggleWave(bool b) {
        waveEnabled = b;
    }
}
