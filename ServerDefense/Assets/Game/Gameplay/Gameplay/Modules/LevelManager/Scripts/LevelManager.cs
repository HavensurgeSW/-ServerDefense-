using System;

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action OnAllWavesCompleted = null;

    [Header("Main Configuration")]
    [SerializeField] private EnemyHandler enemyHandler = null;
    [SerializeField] private WavesController wavesController = null;
    [SerializeField] private PacketsHandler packetsHandler = null;
    [SerializeField] private WaypointManager waypointManager = null;
    
    [Header("Locations Configuration")]
    [SerializeField] private Location[] locations = null;

    private string MXState = null;

    private Action OnWaveEnd = null;

    public Location[] LOCATIONS { get => locations; }

    public void Init(Action onWaveEnd)
    {
        OnWaveEnd = onWaveEnd;

        enemyHandler.Init(waypointManager.WAYPOINTS);        
        packetsHandler.Init(waypointManager.WAYPOINTS);
        wavesController.Init(enemyHandler.GenerateEnemy);
    }

    public void BeginWave(int index)
    {
        if (MXState == null)
        {
            MXState = "Easy";
            AkSoundEngine.SetState("Level", MXState);
            Debug.Log(MXState);
        }

        if (wavesController.IS_WAVE_SPAWNING || wavesController.IS_IN_WAVE)
        {
            return;
        }

        if (wavesController.ALL_WAVES_COMPLETE)
        {
            OnAllWavesCompleted?.Invoke();
            return;
        }

        wavesController.StartWave(index,
            () =>
            {
                OnWaveEnd();
                if (wavesController.ALL_WAVES_COMPLETE)
                {
                    OnAllWavesCompleted?.Invoke();
                }
            });
        //UIManager.OnWaveEnd?.Invoke(true);
        packetsHandler.StartPacketsWave();
    }

    public void AddOnWaveEndCallback(Action callback)
    {
        OnWaveEnd += callback;
    }

    public void RemoveOnWaveEndCallback(Action callback)
    {
        OnWaveEnd -= callback;
    }
}