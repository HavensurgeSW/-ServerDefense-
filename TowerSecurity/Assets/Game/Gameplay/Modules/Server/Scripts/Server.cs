using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
    [SerializeField] private int hp;

    void DealDamageToServer(int dmg) {
        hp = hp - dmg;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy)) {
            DealDamageToServer(enemy.DAMAGE);
            enemy.Die();
        }
    }

 

}
