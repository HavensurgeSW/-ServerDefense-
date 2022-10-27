using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Healthbar enemyHP = null;
    [SerializeField] private float targetChangeDist = 0.1f;

    private int damage;
    private float speed;
    private int hp;
    private int targetIndex;
    private Transform[] wpPath;


    public int DAMAGE { get => damage; }

    public void Init(EnemyData data, Transform[] wpList)
    {
        wpPath = wpList;
        
        damage = data.DAMAGE;
        speed = data.SPEED;
        hp = data.HP;
        enemyHP.SetMaxHP(hp);       

        targetIndex = 0;
    }
    
    private void Update()
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

    public void Die() 
    {
        Destroy(gameObject);
    }

    public void UpdateTargetWP()
    {
        if (targetIndex < wpPath.Length-1)
            targetIndex++;
    }
}
