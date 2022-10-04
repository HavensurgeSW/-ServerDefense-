using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] WaypointManager waypoints;
    [SerializeField] Transform spawnPoint;

    [SerializeField]LevelManager levelManager;

    private float enemySpawnTimer;
    private float packetSpawnTimer;

    private int enemiesSpawned;

    private bool waveEnabled = false;


    private void Update()
    {
        if (waveEnabled)
        {
            enemySpawnTimer -= Time.deltaTime;
            if (enemySpawnTimer < 0)
            {
                enemySpawnTimer = levelManager.GetEnemySpawnTimerData();
                
                if (enemiesSpawned < levelManager.GetEnemyCount())
                {
                    enemiesSpawned++;
                    SpawnEnemy();
                }
                else if (enemiesSpawned == levelManager.GetEnemyCount())
                {
                    ToggleWave(false);
                    enemiesSpawned = 0;
                    levelManager.OnWaveEnd?.Invoke();
                }                
            }

            packetSpawnTimer -= Time.deltaTime;
            if (packetSpawnTimer < 0) 
            {
                packetSpawnTimer = levelManager.GetPacketSpawnTimerData();
                SpawnPacket();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = Instantiate(levelManager.GetEnemyPrefab(), spawnPoint);
        newInstance.GetComponent<Enemy>().Init(waypoints.WAYPOINTS);
        newInstance.SetActive(true);
    }

    private void SpawnPacket() 
    {
        GameObject newInstance = Instantiate(levelManager.GetPacketPrefab(), spawnPoint);
        newInstance.GetComponent<Packets>().Init(waypoints.WAYPOINTS);
        newInstance.SetActive(true);
    }

    public void ToggleWave(bool b) {
        waveEnabled = b;
    }
}
