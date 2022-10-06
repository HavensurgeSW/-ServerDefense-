using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private Aimbot aimbot;

    [SerializeField] private float fireRate;
    float timer = 0f;

    private void Awake()
    {
        aimbot.Init("Enemy");
        aimbot.SetRange(range);
    }

    private void Update()
    {
        if (timer >= fireRate)
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