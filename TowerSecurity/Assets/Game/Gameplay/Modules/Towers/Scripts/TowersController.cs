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

    public bool CanPurchase(string id, int currentMoney)
    {
        TowerData data = towersDictionary[id];
        return currentMoney >= data.PRICE;
    }

    public TowerData GetTowerData(string towerId)
    {
        if (towersDictionary.ContainsKey(towerId))
        {
            return towersDictionary[towerId];
        }

        return null;
    }

    public void TestRelease(string id)
    {
        BaseTower tower = towersListsDictionary[id][0];
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
        BaseTower tower = Instantiate(towersDictionary[towerId].TOWER_PREFAB, towersHolder).GetComponent<BaseTower>();

        TowerData data = towersDictionary[towerId];
        tower.Init(data.DAMAGE, data.RANGE, data.FIRE_RATE);
        tower.SetFocusTargets(data.TARGETS);
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
