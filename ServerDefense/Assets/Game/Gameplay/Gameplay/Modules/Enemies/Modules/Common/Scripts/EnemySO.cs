using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemies/EnemySO", fileName = "EnemySO_")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private string id = string.Empty;
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private int hp = 1;
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 1.0f;
   
    public string ID { get => id; }
    public GameObject ENEMY_PREFAB { get => enemyPrefab; }
    public int HP { get => hp; }
    public int DAMAGE { get => damage; }
    public float SPEED { get => speed; }
}
