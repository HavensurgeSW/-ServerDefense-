using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EnemyHandler enemyHandler;
    [SerializeField] private Location[] locations;
    [SerializeField] private WaveData[] waveTemplates;
    [SerializeField] private PacketData packetData;


    private int waveIndex = 0;
    private WaveData activeWave;
    public Action OnWaveEnd;

    public Location[] LOCATIONS => locations;


    private void Awake()
    {
        activeWave = waveTemplates[waveIndex];
    }

    private void OnEnable()
    {
        OnWaveEnd += QueueWave;
    }
    private void OnDisable()
    {
        OnWaveEnd -= QueueWave;
    }

    void LoadNextWave()
    {
        if (waveIndex < waveTemplates.Length - 1)
        {
            waveIndex++;
            activeWave = waveTemplates[waveIndex];
            enemyHandler.ToggleWave(true);
        }
    }
    public void BeginWave()
    {
        enemyHandler.ToggleWave(true);
    }
    public void PauseWave()
    {
        enemyHandler.ToggleWave(false);
    }

    public float GetEnemySpawnTimerData()
    {
        return activeWave.SPAWN_DELAY;
    }
    public GameObject GetEnemyPrefab()
    {
        return activeWave.SPIDERS;
    }
    public int GetEnemyCount()
    {
        return activeWave.SPIDER_COUNT;
    }

    public float GetPacketSpawnTimerData()
    {
        return packetData.SPAWN_DELAY;
    }
    public GameObject GetPacketPrefab()
    {
        return packetData.PACKET_PREFAB;

    }

    IEnumerator DelayBetweenWaves(int n)
    {
        yield return new WaitForSeconds(n);
        LoadNextWave();
    }

    void QueueWave()
    {
        StartCoroutine(DelayBetweenWaves(10));
    }
}