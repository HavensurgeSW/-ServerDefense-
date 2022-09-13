using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] WaypointManager waypoints;
    [SerializeField] Enemy[] enemies;

    private void Init()
    {
        foreach (Enemy e in enemies)
        {
            e.Init(waypoints.WAYPOINTS);
        }    
    }

    private void Start()
    {
        Init();
    }

}
