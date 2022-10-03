using UnityEngine;
using System;
using UnityEditor;

public class LevelManager : MonoBehaviour
{

    [SerializeField]EnemyHandler enemyHandler;
    [SerializeField]Location[] locations;
    [SerializeField]WaveData[] waveTemplates;
    int waveIndex = 0;
    [SerializeField]WaveData activeWave;
    public Location[] LOCATIONS => locations;

    public Action OnWaveEnd;

    private void Awake()
    {
        activeWave = waveTemplates[waveIndex];
    }

    private void OnEnable()
    {
        OnWaveEnd += LoadNextWave;
    }
    private void OnDisable()
    {
        OnWaveEnd -= LoadNextWave;
    }

    void LoadNextWave() 
    {
        if(waveIndex<waveTemplates.Length-1)
            activeWave = waveTemplates[waveIndex++];
    }
    public void BeginWave() {
        enemyHandler.ToggleWave(true);
    }
    public void PauseWave()
    {
        enemyHandler.ToggleWave(false);
    }

    public float GetSpawnTimerData() {
        return activeWave.SPAWNDELAY;
    }
    public GameObject GetEnemyPrefab() {
        return activeWave.SPIDERS;
    }
    public int GetEnemyCount() {
        return activeWave.SPIDERCOUNT;
    }

}
