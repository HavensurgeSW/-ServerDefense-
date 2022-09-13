using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]EnemyHandler enemyHandler;
    [SerializeField]Location[] locations;

    public Location[] LOCATIONS => locations;
}
