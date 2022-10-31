using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserLine : MonoBehaviour
{
    [SerializeField] LineRenderer lr;

    public void DrawLaser(Transform self, Transform target)
    {
        lr.SetPosition(0, self.position);
        lr.SetPosition(1, target.position);

        //for (int i = 0; i < target.Length; i++)
        //{
        //    lr.SetPosition(i+1, target[i].position);
        //}
    }

    public void ClearLaser() {
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
    }
}
