using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TowerLaser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer = null;

    public void SetPositionCount(int positionCount)
    {
        lineRenderer.positionCount = positionCount;
    }

    public void DrawLine(Vector3 origin, Vector3 target)
    {
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, target);
    }

    public void SetLaserMaterial(Material material)
    {
        lineRenderer.material = material;
    }

    public void DrawChainedLines(Vector3 origin, Vector3[] target)
    {
        lineRenderer.SetPosition(0, origin);
        
        for (int i = 1; i < target.Length; i++)
        {
            lineRenderer.SetPosition(i, target[i - 1]);
        }
    }

    public void ClearLine()
    {
        SetPositionCount(0);
    }
}
