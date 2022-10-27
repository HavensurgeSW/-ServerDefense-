using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Wave")]
public class WaveData : ScriptableObject
{
    [SerializeField] private GameObject spiders;
    [SerializeField] private int spiderCount;
    [SerializeField] private float spawnDelay;

    public GameObject SPIDERS => spiders;
    public int SPIDER_COUNT => spiderCount;
    public float SPAWN_DELAY => spawnDelay;
}
