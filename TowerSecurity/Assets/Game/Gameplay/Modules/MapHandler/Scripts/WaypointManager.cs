using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    public Transform[] WAYPOINTS => points;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(points[i].position, 0.5f);

            if (i < points.Length - 1) {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(points[i].position, points[i+1].position);
            }
        }
    }
}
