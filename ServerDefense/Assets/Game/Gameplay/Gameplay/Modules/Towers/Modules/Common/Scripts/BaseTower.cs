using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] protected Aimbot aimbot = null;
    [SerializeField] protected GameObject towerLaserPrefab = null;
    [SerializeField] protected Transform towerLasersHolder = null;
    
    [Header("Stats Configuration")]
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float rangeRadius = 1.0f;
    [SerializeField] protected float fireRate = 1.0f;

    [Header("Visual Configuration")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    protected ObjectPool<TowerLaser> laserPool = null;
    protected List<TowerLaser> lasersList = null;

    private string id = string.Empty;
    private int level = 0;
    private float timer = 0.0f;

    public string ID { get => id;}
    public int DAMAGE { get => damage; }
    public float RANGE { get => rangeRadius; }
    public float FIRE_RATE { get => fireRate; }
    public int NEXT_LEVEL { get => level + 1; }

    public virtual void Init(string id)
    {
        this.id = id;
        lasersList = new List<TowerLaser>();
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

    public void ConfigureLevel(TowerLevelSO level)
    {
        this.level = level.LEVEL;
        
        ConfigureStats(level.STATS);

        SetTowerMaterial(level.TOWER_MATERIAL);
        SetLaserMaterial(level.LASER_MATERIAL);
    }

    public virtual void ConfigureStats(TowerStatsSO stats)
    {
        damage = stats.DAMAGE;
        rangeRadius = stats.RANGE;
        aimbot.SetRange(rangeRadius);
        fireRate = stats.FIRE_RATE;
        SetFocusTargets(stats.TARGETS);
    }

    public void SetFocusTargets(params string[] targetTags)
    {
        aimbot.SetTargets(targetTags);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    private void SetTowerMaterial(Material material)
    {
        if (material != null)
        {
            spriteRenderer.material = material;
        }
    }

    private void SetLaserMaterial(Material material)
    {
        if (material == null)
        {
            return;
        }

        for (int i = 0; i < lasersList.Count; i++)
        {
            lasersList[i].SetLaserMaterial(material);
        }
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
}