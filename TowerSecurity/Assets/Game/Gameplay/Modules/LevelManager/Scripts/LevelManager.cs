using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField]EnemyHandler enemyHandler;
    [SerializeField]Location[] locations;
    [SerializeField]WaveData[] waveTemplates;
    [SerializeField]WaveData activeWave;


    public Location[] LOCATIONS => locations;

    private void Awake()
    {
        activeWave = waveTemplates[0];
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
