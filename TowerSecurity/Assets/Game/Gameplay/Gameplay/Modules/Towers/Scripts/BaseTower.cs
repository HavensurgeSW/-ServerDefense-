using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] protected Aimbot aimbot = null;
    [SerializeField] protected GameObject towerLaserPrefab = null;
    [SerializeField] protected Transform towerLasersHolder = null;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float rangeRadius = 1.0f;
    [SerializeField] protected float fireRate = 1.0f;

    protected ObjectPool<TowerLaser> laserPool = null;

    private string id = string.Empty;
    private int level = 0;
    private float timer = 0.0f;

    public string ID { get => id;}
    public int DAMAGE { get => damage; }
    public float RANGE { get => rangeRadius; }
    public float FIRE_RATE { get => fireRate; }
    public int CURRENT_LEVEL { get => level; set => level = value; }
    public int NEXT_LEVEL { get => level + 1; }

    public virtual void Init(string id, int damage, float radius, float fireRate)
    {
        this.id = id;
        SetData(damage, radius, fireRate, 0);
        laserPool = new ObjectPool<TowerLaser>(GenerateLaser, GetLaser, ReleaseLaser);
    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            HandleTimedAttack();

            timer = 0;
        }
    }

    protected abstract void HandleTimedAttack();

    protected void DealDamage(Enemy enemy)
    {
        enemy.ReceiveDamage(damage);
    }

    protected TowerLaser GenerateLaser()
    {
        return Instantiate(towerLaserPrefab, towerLasersHolder).GetComponent<TowerLaser>();
    }

    protected void GetLaser(TowerLaser item)
    {
        item.gameObject.SetActive(true);
    }

    protected void ReleaseLaser(TowerLaser item)
    {
        item.gameObject.SetActive(false);
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

    public void SetData(int damage, float radius, float fireRate, int targetCount) 
    {      
        this.damage = damage;
        rangeRadius = radius;
        aimbot.SetRange(rangeRadius);
        this.fireRate = fireRate;
        
        //TargetCount contra el aimbot aca
    }
}