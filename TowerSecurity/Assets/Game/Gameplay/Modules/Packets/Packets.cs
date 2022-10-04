using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packets : MonoBehaviour
{
    public PacketData PACKETDATA;

    int targetIndex;
    Transform[] wpPath;

    public void Init(Transform[] wpList)
    {
        wpPath = wpList;
        targetIndex = 0;
    }

    private void Update()
    {
        float step = PACKETDATA.SPEED * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, wpPath[targetIndex].transform.position, step);

        if (Vector2.Distance(transform.position, wpPath[targetIndex].transform.position) < PACKETDATA.TARGETCHANGEDIST)
        {
            UpdateTargetWP();
        }
    }

    public void UpdateTargetWP()
    {
        if (targetIndex < wpPath.Length - 1)
            targetIndex++;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
