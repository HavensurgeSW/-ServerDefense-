using UnityEngine;

public class Packets : MonoBehaviour
{
    [SerializeField] private PacketData packetData = null;

    private int targetIndex;
    private Transform[] wpPath;

    public PacketData PACKET_DATA => packetData;

    public void Init(Transform[] wpList)
    {
        wpPath = wpList;
        targetIndex = 0;
    }

    private void Update()
    {
        float step = packetData.SPEED * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, wpPath[targetIndex].transform.position, step);

        if (Vector2.Distance(transform.position, wpPath[targetIndex].transform.position) < packetData.TARGET_CHANGE_DISTANCE)
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
        Destroy(gameObject);
    }
}
