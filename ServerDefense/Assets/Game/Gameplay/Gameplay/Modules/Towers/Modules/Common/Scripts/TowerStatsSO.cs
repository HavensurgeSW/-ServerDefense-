using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers/Stats/StatsData", fileName = "StatsData_Tower_")]
public class TowerStatsSO : ScriptableObject
{
    [Header("Tower Data Configuration")]
    [SerializeField] private int damage = 0;
    [SerializeField] private float fireRate = 0;
    [SerializeField] private float range = 1.0f;
    [SerializeField] private string[] targets = null;

    public int DAMAGE { get => damage; }
    public float FIRE_RATE { get => fireRate; }
    public float RANGE { get => range; }
    public string[] TARGETS { get => targets; }
}