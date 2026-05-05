using System.Collections.Generic;
using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using SpaceshipBattle.Presenters;
using UnityEngine;
using UnityEngine.Pool;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Spawns the enemy grid using Unity's ObjectPool.
    /// Uses HashSet for O(1) lookups instead of List.Contains O(n).
    /// </summary>
    public class EnemySpawner : IEnemySpawner
    {
        private readonly EnemyConfig _enemyConfig;
        private readonly GameObject _prefab;
        private readonly ObjectPool<EnemyPresenter> _pool;

        // HashSet for fast O(1) Contains checks — List would be O(n)
        private readonly HashSet<EnemyPresenter> _activeSet  = new();
        private readonly List<EnemyPresenter> _activeList = new();

        /// <summary>
        /// Read-only list for iteration in GamePresenter.
        /// HashSet doesn't guarantee order — List preserves spawn order.
        /// </summary>
        public List<EnemyPresenter> Enemies => _activeList;

        public EnemySpawner(GameObject prefab, EnemyConfig enemyConfig)
        {
            _prefab = prefab;
            _enemyConfig = enemyConfig;

            _pool = new ObjectPool<EnemyPresenter>(
                createFunc:      () => Object.Instantiate(_prefab).GetComponent<EnemyPresenter>(),
                actionOnGet:     e => e.gameObject.SetActive(true),
                actionOnRelease: e => e.gameObject.SetActive(false),
                actionOnDestroy: e => Object.Destroy(e.gameObject),
                defaultCapacity: enemyConfig.Columns * enemyConfig.Rows
            );
        }

        /// <summary>
        /// Spawns a full grid. Enemy type is assigned by row —
        /// top rows are worth more points.
        /// </summary>
        public void SpawnAll()
        {
            for (int col = 0; col < _enemyConfig.Columns; col++)
            for (int row = 0; row < _enemyConfig.Rows; row++)
            {
                int type = row <= GameConstants.Enemy.TypeZeroMaxRow ? 0
                         : row <= GameConstants.Enemy.TypeOneMaxRow  ? 1
                         : 2;

                EnemyPresenter enemy = _pool.Get();
                enemy.OnSpawned(type, row, col);

                _activeList.Add(enemy);
                _activeSet.Add(enemy);
            }
        }

        public void Despawn(EnemyPresenter enemy)
        {
            // O(1) lookup instead of O(n) List.Contains
            if (enemy == null || !_activeSet.Contains(enemy)) return;

            enemy.OnDespawned();
            _pool.Release(enemy);
            _activeSet.Remove(enemy);
            _activeList.Remove(enemy);
        }

        public void DespawnAll()
        {
            foreach (EnemyPresenter e in _activeList)
            {
                e.OnDespawned();
                _pool.Release(e);
            }

            _activeList.Clear();
            _activeSet.Clear();
        }
    }
}