using System;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EnemyHandler enemyHandler = null;
    [SerializeField] private WaypointManager waypoints = null;
    [SerializeField] private Location[] locations = null;
    [SerializeField] private WavesController wavesController = null;
    [SerializeField] private PacketsHandler packetsHandler = null;

    private Action OnWaveEnd = null;

    public Location[] LOCATIONS { get => locations; }

    public void Init(Action onWaveEnd)
    {
        OnWaveEnd = onWaveEnd;

        enemyHandler.Init(waypoints.WAYPOINTS);
        
        packetsHandler.Init(waypoints.WAYPOINTS);

        //wavesController.Init(
        //    (id, onDeath) =>
        //    {
        //        enemyHandler.GenerateEnemy(id, onDeath);
        //    },
        //    OnWaveEnd);

        wavesController.Init(enemyHandler.GenerateEnemy);

    }

    public void BeginWave(int index)
    {
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