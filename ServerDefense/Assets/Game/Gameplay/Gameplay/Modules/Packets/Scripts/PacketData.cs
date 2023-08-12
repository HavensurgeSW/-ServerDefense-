using UnityEngine;

namespace ServerDefense.Gameplay.Gameplay.Modules.Packets
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Packet")]
    public class PacketData : ScriptableObject
    {
        [SerializeField] private GameObject packetPrefab = null;
        [SerializeField] private int KBWorth = 1;
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float targetChangeDist = 0.1f;

        public GameObject PACKET_PREFAB => packetPrefab;
        public int KB_WORTH => KBWorth;
        public float SPAWN_DELAY => spawnDelay;
        public float SPEED => speed;
        public float TARGET_CHANGE_DISTANCE => targetChangeDist;
    }
}