using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTut : MonoBehaviour
{
    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public GameObject StartVFX;
    public GameObject EndVFX;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    [SerializeField] private Aimbot aimbot;

    void Start()
    {
        FillLists();
        DisableLaser();
    }

    void Update()
    {
           
        //Cambiar por el target registrado de la torre (active)
    }

    void EnableLaser()
    {
        lineRenderer.enabled = true;

        for(int i=0; i<particles.Count; i++)
            particles[i].Play();
    }

    void UpdateLaser()
    {
        var mousePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);

        lineRenderer.SetPosition(0, (Vector2)firePoint.position);

        StartVFX.transform.position = (Vector2)firePoint.position;

        lineRenderer.SetPosition(1, mousePos);

        Vector2 direction = mousePos - (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.magnitude);

        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
        }

        EndVFX.transform.position = lineRenderer.GetPosition(1);
    }
    void DisableLaser()
    {
        lineRenderer.enabled = false;
        for (int i = 0; i < particles.Count; i++)
            particles[i].Stop();
    }

    void FillLists()
    {
        for (int i=0; i<StartVFX.transform.childCount; i++)
        {
            var ps = StartVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
                particles.Add(ps);
        }
        for (int i = 0; i < EndVFX.transform.childCount; i++)
        {
            var ps = EndVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
                particles.Add(ps);
        }
    }

}
