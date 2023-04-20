using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers/TowerLevelSO", fileName = "TowerLevelSO_tower_")]
public class TowerLevelSO : ScriptableObject
{
    [Header("Level Configuration")]
    [SerializeField] private int level = 0;
    [SerializeField] private int price = 0;

    [Header("Visual Configuration")]
    [SerializeField] private Material towerMaterial = null;
    [SerializeField] private Material laserMaterial = null;

    [Header("Tower Data Configuration")]
    [SerializeField] private TowerStatsSO stats = null;

    public int LEVEL { get => level; }
    public int PRICE { get => price; }
    public TowerStatsSO STATS { get => stats; }
    public Material TOWER_MATERIAL { get => towerMaterial; }
    public Material LASER_MATERIAL { get => laserMaterial; }
}
