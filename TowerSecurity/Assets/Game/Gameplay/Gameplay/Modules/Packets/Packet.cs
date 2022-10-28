using System;

using UnityEngine;

public class Packet : MonoBehaviour
{
    [SerializeField] private PacketData packetData = null;

    private int targetIndex;
    private Transform[] wpPath;

    private Action<Packet> OnDeath = null;

    public PacketData PACKET_DATA => packetData;

    public void Init(Transform[] wpList, Action<Packet> onDeath)
    {
        wpPath = wpList;
        OnDeath = onDeath;
        targetIndex = 0;
        transform.position = wpPath[targetIndex].position;
    }

    private void Update()
    {
        float step = packetData.SPEED * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, wpPath[targetIndex].position, step);

        if (Vector2.Distance(transform.position, wpPath[targetIndex].position) < packetData.TARGET_CHANGE_DISTANCE)
        {
            UpdateTargetWP();
        }
    }

    public void UpdateTargetWP()
    {
        if (targetIndex < wpPath.Length - 1)
        {
            targetIndex++;
        }
    }

    public void Die()
    {
        OnDeath?.Invoke(this);
    }
}
