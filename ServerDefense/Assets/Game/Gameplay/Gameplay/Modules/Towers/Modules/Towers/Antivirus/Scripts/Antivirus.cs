using UnityEngine;

public class Antivirus : BaseTower
{
    [Header("Antivirus Configuration")]
    [SerializeField] private int maxTargets = 1;

    private TowerLaser laser = null;

    public override void Init(string id)
    {
        base.Init(id);

        lasersList.Add(laserPool.Get());
        laser = lasersList[0];
        laser.SetPositionCount(maxTargets + 1);
    }

    protected override void HandleTimedAttack()
    {
        if (aimbot.ContainsTargets())
        {
            if (aimbot.TryGetTargetComponent(out Enemy enemy))
            {
                DealDamage(enemy);
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (aimbot.ContainsTargets())
        {
            laser.SetPositionCount(maxTargets + 1);
            laser.DrawLine(towerLasersHolder.transform.position, aimbot.TARGETS[0].transform.position);
        }
        else
        {
            laser.ClearLine();
        }
    }
}
