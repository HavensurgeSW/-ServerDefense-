using UnityEngine;
using System.Collections;
using System;

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
        OnWaveEnd += QueueWave;
    }
    private void OnDisable()
    {
        OnWaveEnd -= QueueWave;
    }

    void LoadNextWave() 
    {  
        if (waveIndex < waveTemplates.Length-1)
        {
            waveIndex++;
            activeWave = waveTemplates[waveIndex];
            enemyHandler.ToggleWave(true);
        }
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
    IEnumerator DelayBetweenWaves(int n) 
    {
        yield return new WaitForSeconds(n);
        LoadNextWave();
    }

    void QueueWave() {   
        StartCoroutine(DelayBetweenWaves(3));
    }

}
