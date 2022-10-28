using System;

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EnemyHandler enemyHandler = null;
    [SerializeField] private WaypointManager waypoints = null;
    [SerializeField] private Location[] locations = null;
    [SerializeField] private WavesController wavesController = null;
    [SerializeField] private PacketsHandler packetsHandler = null;

    public Action OnWaveEnd = null;

    public Location[] LOCATIONS { get => locations; }

    public void Init()
    {
        enemyHandler.Init(waypoints.WAYPOINTS);
        
        packetsHandler.Init(waypoints.WAYPOINTS);

        wavesController.Init(
            (id, onDeath) =>
            {
                enemyHandler.GenerateEnemy(id, onDeath);
            },
            OnWaveEnd);
    }

    public void BeginWave(int index)
    {
        wavesController.StartWave(index);
        packetsHandler.StartPacketsWave();
    }
}