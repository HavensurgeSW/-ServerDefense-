using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserLine : MonoBehaviour
{
    [SerializeField] LineRenderer lr;

    public void DrawLaser(Transform self, Transform target, LineRenderer LR)
    {
        LR.SetPosition(0, self.position);
        LR.SetPosition(1, target.position);
        Debug.Log("Drawing 3 arg laser");
    }
    public void DrawLaser(Transform self, Transform target)
    {
        lr.SetPosition(0, self.position);
        lr.SetPosition(1, target.position);
    }

    public void ClearLaser() {
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
    }

    public void EnableLaser()
    {
        lr.enabled = true;
    }

    public void DisableLaser() {
        lr.enabled = false;
    }
}
