using System.Collections.Generic;
using UnityEngine;

public class Firewall : BaseTower
{
    [Header("Firewall Configuration")]
    [SerializeField] private int maxTargets = 1;

    private List<TowerLaser> lasersList = null;

    public override void Init(string id, int damage, float radius, int targets, float fireRate)
    {
        base.Init(id, damage, radius, targets, fireRate);
        lasersList = new List<TowerLaser>();
        maxTargets = targets;
        SetLasers(maxTargets);
    }

    public override void SetData(int damage, float radius, float fireRate, int targetCount)
    {
        base.SetData(damage, radius, fireRate, targetCount);
        maxTargets = targetCount;
        SetLasers(maxTargets);
    }

    protected override void HandleTimedAttack()
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

        int currentMaxTargets = aimbot.TARGETS.Count >= maxTargets ? maxTargets : aimbot.TARGETS.Count;

        if(lasersList == null || lasersList.Count <= 0)
        {
            return;
        }

        if (currentMaxTargets > 0)
        {
            for (int i = 0; i < lasersList.Count; i++)
            {
                lasersList[i].ClearLine();
            }
            Vector3[] positions = new Vector3[currentMaxTargets];

            for (int i = 0; i < currentMaxTargets; i++)
            {
                positions[i] = aimbot.TARGETS[i].transform.position;
            }

            for (int i = 0; i < positions.Length; i++)
            {
                lasersList[i].SetPositionCount(2);
                lasersList[i].DrawLine(transform.position, positions[i]);
            }
        }
        else
        {
            for (int i = 0; i < lasersList.Count; i++)
            {
                lasersList[i].ClearLine();
            }
        }
    }

    private void SetLasers(int count)
    {
        if (lasersList.Count == count)
        {
            return;
        }

        int diff = Mathf.Abs(lasersList.Count - count);
        if(lasersList.Count < count)
        {
            for (int i = 0; i < diff; i++)
            {
                lasersList.Add(laserPool.Get());
            }
        }
        else if(lasersList.Count > count)
        {
            for (int i = 0; i < diff; i++)
            {
                laserPool.Release(lasersList[i]);
                lasersList.RemoveAt(i);
            }
        }        
    }
}
