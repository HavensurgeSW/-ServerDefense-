using System;

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EnemyHandler enemyHandler = null;
    [SerializeField] private Location[] locations = null;
    [SerializeField] private WavesController wavesController = null;
    [SerializeField] private PacketData packetData = null;
    [SerializeField] private float timeBtwWaves = 10;

    public Action OnWaveEnd = null;

    public Location[] LOCATIONS { get => locations; }
    public float TIME_BETWEEN_WAVES { get => timeBtwWaves; }

    public void Init()
    {
        enemyHandler.Init();
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
    }

    public GameObject GetPacketPrefab()
    {
        return packetData.PACKET_PREFAB;
    }
}