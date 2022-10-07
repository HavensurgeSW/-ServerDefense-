using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers/TowerData", fileName = "TowerData_")]
public class TowerData : ScriptableObject
{
    [SerializeField] private string id = string.Empty;
    [SerializeField] private GameObject towerPrefab = null;
    [SerializeField] private int price = 1;
    [SerializeField] private int damage = 1;                  // 
    [SerializeField] private float fireRate = 1.0f;           // Revisar si hay cosas aca que sean innecesarias y puedan existir solo en el prefab
    [SerializeField] private float range = 1.0f;              //
    [SerializeField] private string[] targets = null;         //

    public string ID { get => id; }
    public GameObject TOWER_PREFAB { get => towerPrefab; }
    public int PRICE { get => price; }
    public int DAMAGE { get => damage; }
    public float FIRE_RATE { get => fireRate; }
    public float RANGE { get => range; }
    public string[] TARGETS { get => targets; }
}