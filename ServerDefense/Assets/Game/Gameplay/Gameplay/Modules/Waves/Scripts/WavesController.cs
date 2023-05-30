using System;
using System.Collections;

using UnityEngine;

public class WavesController : MonoBehaviour
{
    [Header("Waves Configuration")]
    [SerializeField] private WaveData[] waves = null;

    [Header("Timer Configuration")]
    [SerializeField] private WavesControllerUI wavesControllerUI = null;
    [SerializeField] private float timerSeconds = 0;

    private bool isTimerEnabled = false;
    private float timeLeft = 0;
    private float maxTime = 0;

    private WaveData activeWave = null;
    private int currentWaveEnemyCount = 0;
    private int currentWaveIndex = 0;

    private bool isWaveSpawning = false;
    private bool isInWave = false;
    private bool allWavesComplete = false;

    private Action OnTimerEnd = null;
    private Action<string, Action> OnEnemySpawned = null;
    private Action OnWaveStart = null;
    private Action OnWaveComplete = null;
    private Action OnAllWavesComplete = null;

    public int CURRENT_WAVE_INDEX { get => currentWaveIndex; set => currentWaveIndex = value; }

    private void Update()
    {
        if (!isTimerEnabled)
        {
            return;
        }

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            wavesControllerUI.SetBarProgress(timeLeft / maxTime);

            if (timeLeft <= 0)
            {
                timeLeft = maxTime;
                isTimerEnabled = false;
                OnTimerEnd?.Invoke();
            }
        }
    }

    public void Init(Action<string, Action> onEnemySpawned, Action onWaveStart, Action onAllWavesComplete)
    {
        OnWaveStart = onWaveStart;
        OnEnemySpawned = onEnemySpawned;
        OnAllWavesComplete = onAllWavesComplete;

        maxTime = timerSeconds;
        timeLeft = maxTime;

        OnTimerEnd = BeginCurrentWave;
    }

    private void ToggleTimer(bool status)
    {
        isTimerEnabled = status;
        timeLeft = maxTime;
    }

    private void BeginCurrentWave()
    {
        StartWave(currentWaveIndex, null);
    }

    public void StartWave(int index, Action onWaveComplete)
    {
        if (isWaveSpawning || isInWave || index >= waves.Length)
        {
            return;
        }

        if (allWavesComplete)
        {
            OnAllWavesComplete?.Invoke();
            return;
        }

        SetActiveWave(waves[index]);

        OnWaveComplete = onWaveComplete;

        BeginWave(activeWave, null);
    }

    private void DecreaseEnemyCountOnEnemyDeath()
    {
        currentWaveEnemyCount -= 1;

        if (currentWaveEnemyCount <= 0)
        {
            isInWave = false;
            currentWaveIndex++;
            allWavesComplete = activeWave == waves[^1];
            OnWaveComplete?.Invoke();

            if (!allWavesComplete)
            {
                ToggleTimer(true);
            }
            else
            {
                OnAllWavesComplete?.Invoke();
            }
        }
    }

    private void SetActiveWave(WaveData wave)
    {
        activeWave = wave;
    }

    private void BeginWave(WaveData wave, Action onWaveSpawnEnd)
    {
        OnWaveStart?.Invoke();
        isWaveSpawning = true;
        isInWave = true;
        onWaveSpawnEnd += () => isWaveSpawning = false;

        currentWaveEnemyCount = GetWaveEnemyCount(wave);
        StartCoroutine(ISpawnWaveEnemies(wave, onWaveSpawnEnd));
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

    private IEnumerator ISpawnWaveEnemies(WaveData wave, Action onWaveSpawnEnd)
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
