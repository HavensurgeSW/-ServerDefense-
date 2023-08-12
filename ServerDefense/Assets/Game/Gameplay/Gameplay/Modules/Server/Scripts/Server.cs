using System;

using UnityEngine;

using ServerDefense.Gameplay.Gameplay.Modules.Enemies;
using ServerDefense.Gameplay.Gameplay.Modules.Packets;

using TMPro;

namespace ServerDefense.Gameplay.Gameplay.Modules.Servers
{
    public class Server : MonoBehaviour
    {
        public static Action<int> OnDamaged = null;
        public static Action OnDeath = null;
        public static Action<int> OnPacketEntry = null;

        [SerializeField] private int hp = 10;
        [SerializeField] private TMP_Text serverHealthText = null;

        public int HEALTH { get => hp; }

        public void Init()
        {
            UpdateHealthText(hp);
        }

        private void DealDamageToServer(int dmg)
        {
            hp -= dmg;
            OnDamaged?.Invoke(hp);

            UpdateHealthText(hp);

            if (hp <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        private void UpdateHealthText(int health)
        {
            serverHealthText.text = "Server Health: " + health.ToString();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                DealDamageToServer(enemy.DAMAGE);
                enemy.Die();
            }

            if (other.TryGetComponent(out Packet packet))
            {
                OnPacketEntry?.Invoke(packet.PACKET_DATA.KB_WORTH);
                packet.Die();
            }
        }
    }
}