using UnityEngine;

[System.Serializable]
public class TowerLevelData
{
    [Header("Level Configuration")]
    [SerializeField] private int level = 0;
    [SerializeField] private int price = 0;

    [Header("Visual Configuration")]
    [SerializeField] private Material towerLevelMaterial = null;

    [Header("Tower Data Configuration")]
    [SerializeField] private TowerStatsData stats = null;

    public int LEVEL { get => level; }
    public int PRICE { get => price; }
    public TowerStatsData STATS { get => stats; }
    public Material TOWER_LEVEL_MATERIAL { get => towerLevelMaterial; }
}