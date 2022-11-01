using UnityEngine;

public class Antivirus : BaseTower
{

    [Header("Antivirus Configuration")]
    [SerializeField] private int maxTargets = 1;
    protected override void HandleAttackingBehaviour()
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

            for (int i = 0; i < maxTargets; i++)
            {
                if (i < aimbot.TARGETS.Count)
                {
                    laser.DrawLaser(this.transform, aimbot.TARGETS[i].transform);
                }
            }

        }
        else
        {

            for (int i = 0; i < maxTargets; i++)
            {
                if (i < aimbot.TARGETS.Count)
                {
                    laser.ClearLaser(); 
                }
            }
        }
    }
}
