using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers/TowerData", fileName = "TowerData_")]
public class TowerData : ScriptableObject
{
    [SerializeField] private string id = string.Empty;
    [SerializeField] private GameObject towerPrefab = null;
    [SerializeField] private List<BaseTowerLevelData> levels = null;
    [SerializeField] private Vector2 offset = Vector2.zero;

    public string ID { get => id; }
    public GameObject TOWER_PREFAB { get => towerPrefab; }
    public List<BaseTowerLevelData> LEVELS { get => levels; }
    public Vector2 OFFSET { get => offset; }
}