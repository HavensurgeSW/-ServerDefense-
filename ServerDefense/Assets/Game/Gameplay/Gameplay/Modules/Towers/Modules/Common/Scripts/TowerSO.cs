using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers/TowerSO", fileName = "TowerSO_")]
public class TowerSO : ScriptableObject
{
    [SerializeField] private string id = string.Empty;
    [SerializeField] private GameObject towerPrefab = null;
    [SerializeField] private TowerLevelSO[] levels = null;
    [SerializeField] private Vector2 offset = Vector2.zero;

    public string ID { get => id; }
    public GameObject TOWER_PREFAB { get => towerPrefab; }
    public TowerLevelSO[] LEVELS { get => levels; }
    public Vector2 OFFSET { get => offset; }
}