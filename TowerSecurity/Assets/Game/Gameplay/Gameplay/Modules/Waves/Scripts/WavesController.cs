using System;
using System.Collections;

using UnityEngine;

public class WavesController : MonoBehaviour
{
    [SerializeField] private WaveData[] waves = null;

    private WaveData activeWave = null;
    private int currentWaveEnemyCount = 0;
    private bool isWaveSpawning = false;
    private bool isInWave = false;

    private Action<string, Action> OnEnemySpawned = null;
    private Action OnWaveCompleted = null;

    public bool IS_WAVE_SPAWNING { get => isWaveSpawning; }
    public bool IS_IN_WAVE { get => isInWave; }

    public void Init(Action<string, Action> onEnemySpawned, Action onWaveCompleted)
    {
        OnEnemySpawned = onEnemySpawned;
        OnWaveCompleted = onWaveCompleted;
    }

    public void StartWave(int index)
    {
        if (isWaveSpawning || isInWave)
        {
            return;
        }

        if (index >= waves.Length - 1)
        {
            return;
        }

        SetActiveWave(waves[index]);
        BeginWave(null);
    }

    private void DecreaseEnemyCountOnEnemyDeath()
    {
        currentWaveEnemyCount -= 1;

        if (currentWaveEnemyCount <= 0)
        {
            isInWave = false;
            OnWaveCompleted?.Invoke();
        }
    }

    private void SetActiveWave(WaveData wave)
    {
        activeWave = wave;
    }

    private void BeginWave(Action onWaveSpawnEnd)
    {
        isWaveSpawning = true;
        isInWave = true;
        onWaveSpawnEnd += () => isWaveSpawning = false;

        currentWaveEnemyCount = GetWaveEnemyCount(activeWave);
        StartCoroutine(IBeginWave(activeWave, onWaveSpawnEnd));
    }

    private int GetWaveEnemyCount(WaveData wave)
    {
        int toReturn = 0;
        
        for (int i = 0; i < wave.WAVE_SPAWN_DATA.Length; i++)
        {
            toReturn += wave.WAVE_SPAWN_DATA[i].ENEMY_COUNT;
        }

        return toReturn;
    }

    private IEnumerator IBeginWave(WaveData wave, Action onWaveSpawnEnd)
    {
        for (int i = 0; i < wave.WAVE_SPAWN_DATA.Length; i++)
        {
            WaveData.WaveSpawnData data = wave.WAVE_SPAWN_DATA[i];

            WaitForSeconds delay = new WaitForSeconds(data.SPAWN_DELAY_PER_ENEMY);

            for (int j = 0; j < data.ENEMY_COUNT; j++)
            {
                OnEnemySpawned?.Invoke(data.ENEMY_ID, DecreaseEnemyCountOnEnemyDeath);
                yield return delay;
            }
        }
        
        onWaveSpawnEnd?.Invoke();
    }
}
