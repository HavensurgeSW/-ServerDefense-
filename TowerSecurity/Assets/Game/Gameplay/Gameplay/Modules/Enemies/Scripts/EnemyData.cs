using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private int hp;
   
    public float SPEED { get => speed; }
    public int DAMAGE { get => damage; }
    public int HP { get =>hp; }
}
