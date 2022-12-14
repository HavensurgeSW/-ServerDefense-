using System;

using UnityEngine;

public class Server : MonoBehaviour
{
    public static Action<int> OnDamaged = null;
    public static Action OnDeath = null;
    public static Action<int> OnPacketEntry = null;

    [SerializeField] private int hp;
    
    public int HEALTH { get => hp; }

    private void DealDamageToServer(int dmg) 
    {
        hp -= dmg;
        OnDamaged?.Invoke(hp);
        //AkSoundEngine.PostEvent("Play_Virus_Pass", gameObject);

        if (hp <= 0) 
        {
            OnDeath?.Invoke();   
        }
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
