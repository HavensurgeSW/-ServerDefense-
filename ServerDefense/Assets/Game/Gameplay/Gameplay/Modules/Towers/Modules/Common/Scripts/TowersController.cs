using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

public class TowersController : MonoBehaviour
{
    [SerializeField] private TowerSO[] towersData = null;
    [SerializeField] private Transform towersHolder = null;

    private Dictionary<string, TowerSO> towersDictionary = null;
    private Dictionary<string, ObjectPool<BaseTower>> towerPools = null;
    private Dictionary<string, List<BaseTower>> towersListsDictionary = null;

    public void Init()
    {
        towersDictionary = new Dictionary<string, TowerSO>();
        towerPools = new Dictionary<string, ObjectPool<BaseTower>>();
        towersListsDictionary = new Dictionary<string, List<BaseTower>>();

        for (int i = 0; i < towersData.Length; i++)
        {
            string id = towersData[i].ID;
            towersDictionary.Add(id, towersData[i]);
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

    public TowerSO GetTowerData(string towerId)
    {
        if (towersDictionary.ContainsKey(towerId))
        {
            return towersDictionary[towerId];
        }

        return null;
    }

    public TowerLevelSO GetLevelDataFromIndex(string towerId, int levelIndex)
    {
        TowerSO data = GetTowerData(towerId);

        if (data != null)
        {
            for (int i = 0; i < data.LEVELS.Length; i++)
            {
                if (data.LEVELS[i].LEVEL == levelIndex)
                {
                    return data.LEVELS[i];
                }
            }
        }

        return null;
    }

    public TowerLevelSO[] GetTowerLevelsData(string towerId)
    {
        TowerSO data = GetTowerData(towerId);

        if (data != null)
        {
            return data.LEVELS;
        }

        return null;
    }

    public void UpgradeTower(BaseTower tower, int levelToSet)
    {
        TowerLevelSO levelData = GetLevelDataFromIndex(tower.ID, levelToSet);

        tower.CURRENT_LEVEL = levelToSet;
        tower.ConfigureStats(levelData.STATS);
        tower.SetTowerMaterial(levelData.TOWER_MATERIAL);
        tower.SetLaserMaterial(levelData.LASER_MATERIAL);
    }

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
        TowerSO towerData = towersDictionary[towerId];
        BaseTower tower = Instantiate(towerData.TOWER_PREFAB, towersHolder).GetComponent<BaseTower>();
        tower.gameObject.transform.localPosition += new Vector3(0, towerData.OFFSET.y, 0);

        TowerLevelSO levelData = towerData.LEVELS[0];

        tower.Init(towerData.ID, levelData.STATS, levelData.TOWER_MATERIAL);
        tower.CURRENT_LEVEL = levelData.LEVEL;
        tower.SetFocusTargets(levelData.STATS.TARGETS);
        tower.SetLaserMaterial(levelData.LASER_MATERIAL);

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