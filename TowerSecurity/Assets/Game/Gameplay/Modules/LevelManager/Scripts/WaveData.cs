using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Wave")]
public class WaveData : ScriptableObject
{
    [SerializeField] GameObject spiders;
    [SerializeField] int spiderCount;
    [SerializeField] float spawnDelay;

    public GameObject SPIDERS => spiders;
    public int SPIDERCOUNT => spiderCount;
    public float SPAWNDELAY => spawnDelay;
}
