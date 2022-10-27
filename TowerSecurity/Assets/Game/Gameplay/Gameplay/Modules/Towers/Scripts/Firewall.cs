using UnityEngine;

public class Firewall : BaseTower
{
    [Header("Firewall Configuration")]
    [SerializeField] private int maxTargets = 1;

    protected override void HandleAttackingBehaviour()
    {
        int currentMaxTargets = aimbot.TARGETS.Count >= maxTargets ? maxTargets : aimbot.TARGETS.Count;

        if (currentMaxTargets > 0)
        {
            GameObject[] objs = new GameObject[currentMaxTargets];

            for (int i = 0; i < currentMaxTargets; i++)
            {
                objs[i] = aimbot.TARGETS[i];
            }

            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].TryGetComponent(out Enemy enemy))
                {
                    DealDamage(enemy);
                }                
            }
        }
    }
}
