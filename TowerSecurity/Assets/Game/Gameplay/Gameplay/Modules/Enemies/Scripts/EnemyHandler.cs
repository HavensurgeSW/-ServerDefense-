using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

public class EnemyHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private EnemyData[] enemiesData = null;
    [SerializeField] private Transform enemiesHolder = null;
    [SerializeField] private LevelManager levelManager = null;
    [SerializeField] private WaypointManager waypoints = null;
    [SerializeField] private Transform spawnPoint = null;

    private Dictionary<string, EnemyData> enemiesDictionary = null;
    private Dictionary<string, ObjectPool<Enemy>> enemyPools = null;
    private Dictionary<string, List<Enemy>> enemyListsDictionary = null;

    private float enemySpawnTimer;
    private float packetSpawnTimer;

    private int enemiesSpawned;

    private bool waveEnabled = false;

    public void Init()
    {
        enemiesDictionary = new Dictionary<string, EnemyData>();
        enemyPools = new Dictionary<string, ObjectPool<Enemy>>();
        enemyListsDictionary = new Dictionary<string, List<Enemy>>();

        for (int i = 0; i < enemiesData.Length; i++)
        {
            string id = enemiesData[i].ID;
            enemiesDictionary.Add(id, enemiesData[i]);
            enemyPools.Add(id, new ObjectPool<Enemy>(() => SpawnEnemy(id), GetEnemy, ReleaseEnemy));
            enemyListsDictionary.Add(id, new List<Enemy>());
        }
    }

    private void Update()
    {
        if (!waveEnabled)
        {
            return;
        }

        enemySpawnTimer -= Time.deltaTime;
        if (enemySpawnTimer < 0)
        {
            enemySpawnTimer = levelManager.GetEnemySpawnTimerData();

            if (enemiesSpawned < levelManager.GetEnemyCount())
            {
                enemiesSpawned++;
                //SpawnEnemy();
            }
            else if (enemiesSpawned == levelManager.GetEnemyCount())
            {
                ToggleWave(false);
                enemiesSpawned = 0;
                levelManager.OnWaveEnd?.Invoke();
                UIManager.OnWaveEnd?.Invoke(true);
            }
        }

        packetSpawnTimer -= Time.deltaTime;
        if (packetSpawnTimer < 0)
        {
            packetSpawnTimer = levelManager.GetPacketSpawnTimerData();
            SpawnPacket();
        }
    }

    public Enemy GenerateEnemy(string enemyId)
    {
        Enemy enemy = enemyPools[enemyId].Get();
        enemyListsDictionary[enemyId].Add(enemy);
        enemy.transform.SetParent(enemiesHolder);
        return enemy;
    }

    private Enemy SpawnEnemy(string enemyId)
    {
        EnemyData data = enemiesDictionary[enemyId];
        Enemy enemy = Instantiate(data.ENEMY_PREFAB, enemiesHolder).GetComponent<Enemy>();
        return enemy;
    }

    private void GetEnemy(Enemy item)
    {
        item.gameObject.SetActive(true);
    }

    private void ReleaseEnemy(Enemy item)
    {
        item.gameObject.SetActive(false);
    }

    private void SpawnPacket()
    {
        GameObject newInstance = Instantiate(levelManager.GetPacketPrefab(), spawnPoint);
        newInstance.GetComponent<Packets>().Init(waypoints.WAYPOINTS);
        newInstance.SetActive(true);
    }

    public void ToggleWave(bool b)
    {
        waveEnabled = b;
    }
}