using UnityEngine;
using UnityEngine.Pool;

using TMPro;

namespace ServerDefense.Gameplay.Gameplay.Modules.Packets
{
    public class PacketsHandler : MonoBehaviour
    {
        [SerializeField] private PacketData packetData = null;
        [SerializeField] private Transform packetsHolder = null;
        [SerializeField] private TMP_Text packetPointsText = null;
        [SerializeField] private int packetsPerWave = 1;

        private Transform[] waypoints = null;
        private ObjectPool<Packet> packetPool = null;

        private int currentPackets = 0;
        private float spawnTimer = 0.0f;
        private bool allowPackets = false;

        public void Init(Transform[] waypoints)
        {
            this.waypoints = waypoints;
            packetPool = new ObjectPool<Packet>(GeneratePacket, GetPacket, ReleasePacket);
            allowPackets = false;
            UpdatePacketPointsText(0);
        }

        public void UpdatePacketPointsText(int points)
        {
            packetPointsText.text = "Current KB: " + points.ToString();
        }

        public void StartPacketsWave()
        {
            allowPackets = true;
        }

        private void Update()
        {
            if (!allowPackets)
            {
                return;
            }

            spawnTimer += Time.deltaTime;

            if (spawnTimer >= packetData.SPAWN_DELAY)
            {
                SpawnPackets();
                spawnTimer = 0;

                if (currentPackets == packetsPerWave)
                {
                    currentPackets = 0;
                    allowPackets = false;
                }
            }
        }

        private void SpawnPackets()
        {
            currentPackets++;
            allowPackets = true;

            Packet packet = packetPool.Get();
            packet.Init(waypoints, packetPool.Release);
        }

        private Packet GeneratePacket()
        {
            return Instantiate(packetData.PACKET_PREFAB, packetsHolder).GetComponent<Packet>();
        }

        private void GetPacket(Packet item)
        {
            item.gameObject.SetActive(true);
        }

        private void ReleasePacket(Packet item)
        {
            item.gameObject.SetActive(false);
        }
    }
}