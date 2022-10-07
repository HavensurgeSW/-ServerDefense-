using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private Aimbot aimbot = null;
    [SerializeField] private float rangeRadius = 1.0f;
    [SerializeField] private int damage = 1;

    [SerializeField] private float fireRate = 1.0f;

    private float timer = 0.0f;

    public void Init(int damage, float radius, float fireRate)
    {
        this.damage = damage;
        rangeRadius = radius;
        aimbot.SetRange(rangeRadius);
        this.fireRate = fireRate;
    }

    public void SetFocusTargets(params string[] targetTags)
    {
        aimbot.Init(targetTags);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    private void Update()
    {
        timer += Time.deltaTime;

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
    }

    private void DealDamage(Enemy enemy)
    {
        enemy.ReceiveDamage(damage);
    }
}