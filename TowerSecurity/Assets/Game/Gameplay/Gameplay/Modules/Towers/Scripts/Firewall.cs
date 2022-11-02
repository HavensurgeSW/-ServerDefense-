using UnityEngine;

public class Firewall : BaseTower
{
    [Header("Firewall Configuration")]
    [SerializeField] private int maxTargets = 1;
    [SerializeField] private LineRenderer LRPrefab;
    [SerializeField] private LineRenderer[] lr;
   


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

    protected override void Update()
    {
        base.Update();

        if (aimbot.ContainsTargets())
        {

            for (int i = 0; i < maxTargets; i++)
            {
                //if (i < aimbot.TARGETS.Count)
                    laser.DrawLaser(this.transform, aimbot.TARGETS[i].transform, lr[i]);
            }

        }

        laser.DrawLaser(this.transform, aimbot.TARGETS[0].transform, lr[0]);
        //else
        //{

        //    for (int i = 0; i < maxTargets; i++)
        //    {
        //        if (i < aimbot.TARGETS.Count)
        //        {
        //            laser.ClearLaser(); 
        //        }
        //    }
        //}
    }

    private void Start()
    {
        for (int i = 0; i < maxTargets; i++)
        {
            lr[i] = Instantiate(LRPrefab, transform);
        }
    }
}
