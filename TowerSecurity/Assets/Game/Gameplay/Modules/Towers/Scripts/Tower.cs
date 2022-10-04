using System.Data.SqlTypes;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private Aimbot aimbot;

    [SerializeField] private float firerate;
    float timer = 0f;

    private void Awake()
    {
        aimbot.Init("Enemy");
        aimbot.SetRange(range);
    }

    private void Update()
    {
        if (timer >= firerate)
        {
           
            if (aimbot.ContainsTargets())
            {
                if (aimbot.TryGetTargetComponent(out Enemy enemy))
                {
                    DealDamage(enemy);
                }
            }

            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void DealDamage(Enemy enemy)
    {
        enemy.ReceiveDamage(damage);
    }
}
