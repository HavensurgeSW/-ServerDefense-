using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
    [SerializeField] private int hp;
    public static Action OnDeath;
    public static Action<int> OnPacketEntry;
    public int HEALTH { get => hp; }

    void DealDamageToServer(int dmg) 
    {
        hp = hp - dmg;
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

        if (other.TryGetComponent(out Packets packet))
        {
            OnPacketEntry?.Invoke(packet.PACKETDATA.KBWORTH);
            packet.Die();
        }

    }

 

}
