using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EnemyHandler enemyHandler = null;
    [SerializeField] private Location[] locations = null;
    [SerializeField] private WaveData[] waveTemplates = null;
    [SerializeField] private PacketData packetData = null;
    [SerializeField] private float timeBtwWaves = 10;

    private int waveIndex = 0;
    public Action OnWaveEnd;    

    public Location[] LOCATIONS => locations;
    public float TIMEBTWWAVES=> timeBtwWaves;

    private void Awake()
    {
        //activeWave = waveTemplates[waveIndex];
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
            //activeWave = waveTemplates[waveIndex];
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
        return 0;
    }
    public GameObject GetEnemyPrefab()
    {
        return null;
    }
    public int GetEnemyCount()
    {
        return 0;
    }

    public float GetPacketSpawnTimerData()
    {
        return packetData.SPAWN_DELAY;
    }
    public GameObject GetPacketPrefab()
    {
        return packetData.PACKET_PREFAB;

    }

    IEnumerator DelayBetweenWaves(float n)
    {
        yield return new WaitForSeconds(n);
        LoadNextWave();
    }

    void QueueWave()
    {

        StartCoroutine(DelayBetweenWaves(timeBtwWaves));
    }
}