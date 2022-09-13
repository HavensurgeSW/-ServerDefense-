using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int damage;
    private float speed;
    private int hp;
    private int targetIndex;
    private Transform[] wpPath;

    [SerializeField] private float targetChangeDist = 0.5f;
    [SerializeField] private EnemyData ENEMYDATA;

    public int DAMAGE { get => damage; }
    //public WaypointManager TARGET { get => target; }

    [SerializeField]Healthbar enemyHP;


    public void Init(Transform[] wpList)
    {
        wpPath = wpList;
        targetIndex = 0;
    }

    void Start()
    {
        damage = ENEMYDATA.DAMAGE;
        speed = ENEMYDATA.SPEED;
        hp = ENEMYDATA.HP;
       
        enemyHP.SetMaxHP(hp);
       
    }

    
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, wpPath[targetIndex].transform.position, step);

        if (Vector2.Distance(transform.position, wpPath[targetIndex].transform.position) < targetChangeDist) 
        {
           UpdateTargetWP();
        }
    }

    public void ReceiveDamage(int dmg)
    {
        hp = hp - dmg;
        enemyHP.SetHealthbarFill(hp);
        if (hp <= 0) {
            Die();
        }
    }

    public void Die() {
        Destroy(gameObject);
    }

    public void UpdateTargetWP()
    {
        if (targetIndex < wpPath.Length-1)
            targetIndex++;
    }
}
