using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers/Stats/FirewallStatsData", fileName = "StatsData_Firewall_")]
public class FirewallTowerStatsSO : TowerStatsSO
{
    [SerializeField] private int targetCount = 0;

    public int TARGET_COUNT { get => targetCount; }
}
