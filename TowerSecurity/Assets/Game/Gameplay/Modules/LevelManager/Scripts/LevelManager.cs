using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]EnemyHandler enemyHandler;
    [SerializeField]Location[] locations;

    public Location[] LOCATIONS => locations;

    public void BeginWave() {
        enemyHandler.ToggleWave(true);
    }
    public void PauseWave()
    {
        enemyHandler.ToggleWave(false);
    }

}
