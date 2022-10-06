using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers/TowerData", fileName = "TowerData_")]
public class TowerData : ScriptableObject
{
    [SerializeField] private string id = string.Empty;
    [SerializeField] private int price = 0;
    [SerializeField] private GameObject towerPrefab = null;

    public string ID { get => id; }
    public int PRICE { get => price; }
    public GameObject TOWER_PREFAB { get => towerPrefab; }
}