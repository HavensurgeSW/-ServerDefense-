using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

namespace ServerDefense.Gameplay.Gameplay.Modules.Enemies
{
    public class EnemyHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private EnemySO[] enemiesData = null;
        [SerializeField] private Transform enemiesHolder = null;

        private Dictionary<string, EnemySO> enemiesDictionary = null;
        private Dictionary<string, ObjectPool<Enemy>> enemyPools = null;
        private Dictionary<string, List<Enemy>> enemyListsDictionary = null;

        private Transform[] waypoints = null;

        public void Init(Transform[] waypoints)
        {
            this.waypoints = waypoints;

            enemiesDictionary = new Dictionary<string, EnemySO>();
            enemyPools = new Dictionary<string, ObjectPool<Enemy>>();
            enemyListsDictionary = new Dictionary<string, List<Enemy>>();

            for (int i = 0; i < enemiesData.Length; i++)
            {
                string id = enemiesData[i].ID;
                enemiesDictionary.Add(id, enemiesData[i]);
                enemyPools.Add(id, new ObjectPool<Enemy>(() => SpawnEnemy(id), GetEnemy, ReleaseEnemy));
                enemyListsDictionary.Add(id, new List<Enemy>());
            }
        }

        public void GenerateEnemy(string enemyId, Action onDeath)
        {
            Enemy enemy = enemyPools[enemyId].Get();
            enemyListsDictionary[enemyId].Add(enemy);
            enemy.transform.SetParent(enemiesHolder);

            enemy.Init(enemiesDictionary[enemyId], waypoints,
                (enemy) =>
                {
                    ReleaseActiveEnemy(enemy);
                    onDeath?.Invoke();
                });
        }

        private Enemy SpawnEnemy(string enemyId)
        {
            EnemySO data = enemiesDictionary[enemyId];
            Enemy enemy = Instantiate(data.ENEMY_PREFAB, enemiesHolder).GetComponent<Enemy>();
            return enemy;
        }

        private void GetEnemy(Enemy item)
        {
            item.gameObject.SetActive(true);
        }

        private void ReleaseEnemy(Enemy item)
        {
            item.gameObject.SetActive(false);
        }

        public void ReleaseActiveEnemy(Enemy enemy)
        {
            string id = enemy.ID;
            enemyListsDictionary[id].Remove(enemy);
            enemyPools[id].Release(enemy);
        }
    }
}