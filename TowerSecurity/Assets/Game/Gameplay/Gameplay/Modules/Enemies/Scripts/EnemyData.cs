using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Enemies/EnemyData", fileName = "EnemyData_")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string id = string.Empty;
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int hp = 1;
   
    public string ID { get => id; }
    public GameObject ENEMY_PREFAB { get => enemyPrefab; }
    public float SPEED { get => speed; }
    public int DAMAGE { get => damage; }
    public int HP { get =>hp; }
}
