using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Waves", fileName = "Wave_")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class WaveSpawnData
    {
        [SerializeField] private string enemyId = string.Empty;
        [SerializeField] private int enemyCount = 0;
        [SerializeField] private float spawnDelayPerEnemy = 0.0f;
        
        public string ENEMY_ID => enemyId;
        public int ENEMY_COUNT => enemyCount;
        public float SPAWN_DELAY_PER_ENEMY => spawnDelayPerEnemy;
    }

    [SerializeField] private WaveSpawnData[] waveSpawnData = null;

    public WaveSpawnData[] WAVE_SPAWN_DATA => waveSpawnData;
}
