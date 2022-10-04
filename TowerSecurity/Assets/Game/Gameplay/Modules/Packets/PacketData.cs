using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Packet")]
public class PacketData : ScriptableObject
{
    [SerializeField] GameObject packetPrefab;
    [SerializeField] int KBWorth;
    [SerializeField] float spawnDelay;
    [SerializeField] float speed;
    [SerializeField] float targetChangeDist;

    public GameObject PACKETPREFAB => packetPrefab;
    public int KBWORTH => KBWorth;
    public float SPAWNDELAY => spawnDelay;
    public float SPEED => speed;
    public float TARGETCHANGEDIST => targetChangeDist;
}
