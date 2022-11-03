using System;

using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Healthbar enemyHP = null;
    [SerializeField] private float targetChangeDist = 0.1f;
    [SerializeField] private SpriteRenderer sr;

    private Transform[] wpPath = null;

    private string id = string.Empty;
    private int damage = 1;
    private float speed = 1.0f;
    private int hp = 1;
    private int targetIndex = 0;
    private bool isFlipped = false;

    private Action<Enemy> OnDeath = null;

    public string ID { get => id; }
    public int DAMAGE { get => damage; }

    public void Init(EnemyData data, Transform[] wpList, Action<Enemy> onDeath)
    {
        wpPath = wpList;
        
        id = data.ID;
        hp = data.HP;
        damage = data.DAMAGE;
        speed = data.SPEED;
        enemyHP.SetMaxHP(hp);
        enemyHP.SetHealthbarFill(hp);

        OnDeath = onDeath;

        targetIndex = 0;

        transform.position = wpPath[targetIndex].position;
    }
    
    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, wpPath[targetIndex].position, step);

        if (Vector2.Distance(transform.position, wpPath[targetIndex].position) < targetChangeDist)
        {
            transform.position = wpPath[targetIndex].position;
            UpdateTargetWP();
        }
    }

    public void ReceiveDamage(int dmg)
    {
        hp -= dmg;
        enemyHP.SetHealthbarFill(hp);

        if (hp <= 0) 
        {
            Die();
        }
    }

    public void Die() 
    {
        OnDeath?.Invoke(this);
    }

    public void UpdateTargetWP()
    {
        if (targetIndex < wpPath.Length - 1)
        {
            FlipTexture();
            targetIndex++;
        }
    }

    private void FlipTexture() {


        if (isFlipped)
        {
            sr.flipX = true;
            isFlipped = false;
        }
        else { 
            sr.flipX = false;
            isFlipped = true;
        }
    }
}
