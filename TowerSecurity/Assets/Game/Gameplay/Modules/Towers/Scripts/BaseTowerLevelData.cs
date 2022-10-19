using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers/LevelData", fileName = "LevelData_")]
public class BaseTowerLevelData : ScriptableObject
{
    [Header("Level Configuration")]
    [SerializeField] private int level = 0;
    [SerializeField] private int price = 0;
    
    [Header("Tower Data Configuration")]
    [SerializeField] private int damage = 0;
    [SerializeField] private float fireRate = 0;
    [SerializeField] private float range = 1.0f;           
    [SerializeField] private string[] targets = null;
    [SerializeField] private int targetCount = 0;

    public int LEVEL { get => level; }
    public int PRICE { get => price; }
    public int DAMAGE { get => damage; }
    public float FIRE_RATE { get => fireRate; }
    public float RANGE { get => range; }
    public string[] TARGETS { get => targets; }
    public int TARGET_COUNT { get => targetCount; }
}
