using System;

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] private EnemyHandler enemyHandler = null;
    [SerializeField] private WavesController wavesController = null;
    [SerializeField] private PacketsHandler packetsHandler = null;
    [SerializeField] private WaypointManager waypointManager = null;
    
    [Header("Locations Configuration")]
    [SerializeField] private Location[] locations = null;

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
        if (wavesController.IS_WAVE_SPAWNING || wavesController.IS_IN_WAVE)
        {
            return;
        }

        wavesController.StartWave(index, OnWaveEnd);
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