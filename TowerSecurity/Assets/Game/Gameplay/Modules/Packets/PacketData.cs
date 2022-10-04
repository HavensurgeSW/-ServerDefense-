using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Packet")]
public class PacketData : ScriptableObject
{
    [SerializeField] private GameObject packetPrefab;
    [SerializeField] private int KBWorth;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float speed;
    [SerializeField] private float targetChangeDist;

    public GameObject PACKET_PREFAB => packetPrefab;
    public int KB_WORTH => KBWorth;
    public float SPAWN_DELAY => spawnDelay;
    public float SPEED => speed;
    public float TARGET_CHANGE_DISTANCE => targetChangeDist;
}
