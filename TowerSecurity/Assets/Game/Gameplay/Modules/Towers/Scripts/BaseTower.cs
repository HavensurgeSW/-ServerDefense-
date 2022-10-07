using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] protected Aimbot aimbot = null;
    [SerializeField] protected float rangeRadius = 1.0f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float fireRate = 1.0f;

    private float timer = 0.0f;

    public virtual void Init(int damage, float radius, float fireRate)
    {
        this.damage = damage;
        rangeRadius = radius;
        aimbot.SetRange(rangeRadius);
        this.fireRate = fireRate;
    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            HandleAttackingBehaviour();

            timer = 0;
        }
    }

    protected abstract void HandleAttackingBehaviour();

    protected void DealDamage(Enemy enemy)
    {
        enemy.ReceiveDamage(damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
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
}