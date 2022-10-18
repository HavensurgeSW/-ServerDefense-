using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

public class TowersController : MonoBehaviour
{
    [SerializeField] private TowerData[] towersData = null;
    [SerializeField] private Transform towersHolder = null;

    private Dictionary<string, TowerData> towersDictionary = null;
    private Dictionary<string, ObjectPool<BaseTower>> towerPools = null;
    private Dictionary<string, List<BaseTower>> towersListsDictionary = null;

    public void Init()
    {
        towersDictionary = new Dictionary<string, TowerData>();
        towerPools = new Dictionary<string, ObjectPool<BaseTower>>();
        towersListsDictionary = new Dictionary<string, List<BaseTower>>();

        for (int i = 0; i < towersData.Length; i++)
        {
            string id = towersData[i].ID;
            towersDictionary.Add(towersData[i].ID, towersData[i]);
            towerPools.Add(id, new ObjectPool<BaseTower>(() => SpawnTower(id), GetTower, ReleaseTower));
            towersListsDictionary.Add(id, new List<BaseTower>());
        }
    }

    public bool DoesTowerIdExist(string id)
    {
        foreach (var item in towersDictionary)
        {
            if (item.Key == id)
            {
                return true;
            }
        }

        return false;
    }

    public TowerData GetTowerData(string towerId)
    {
        if (towersDictionary.ContainsKey(towerId))
        {
            return towersDictionary[towerId];
        }

        return null;
    }

    // Lo dejo para acordarme como sacar towers de forma correcta
    public void ReleaseActiveTower(BaseTower tower)
    {
        string id = tower.ID;
        towersListsDictionary[id].Remove(tower);
        towerPools[id].Release(tower);
    }

    public BaseTower GenerateTower(string towerId, Transform holder)
    {
        BaseTower tower = towerPools[towerId].Get();
        towersListsDictionary[towerId].Add(tower);
        tower.SetParent(holder);
        return tower;
    }

    private BaseTower SpawnTower(string towerId)
    {
        TowerData data = towersDictionary[towerId];
        BaseTower tower = Instantiate(data.TOWER_PREFAB, towersHolder).GetComponent<BaseTower>();

        tower.Init(data.ID, data.LEVELS[0].DAMAGE, data.LEVELS[0].RANGE, data.LEVELS[0].FIRE_RATE);
        tower.SetFocusTargets(data.LEVELS[0].TARGETS); 
        return tower;
    }

    private void GetTower(BaseTower tower)
    {
        tower.gameObject.SetActive(true);
    }

    private void ReleaseTower(BaseTower tower)
    {
        tower.gameObject.SetActive(false);
    }
}