using System.Collections.Generic;
using SpaceshipBattle.Presenters;
using UnityEngine;
using UnityEngine.Pool;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Pooled projectile spawner shared by player and enemies.
    /// Uses HashSet for O(1) Contains checks on Despawn.
    /// Direction determines the shooter:
    /// Vector3.forward = player, Vector3.back = enemy.
    /// </summary>
    public class ProjectileSpawner : IProjectileSpawner
    {
        private readonly ObjectPool<ProjectilePresenter> _pool;

        // Same pattern as EnemySpawner — HashSet for lookup, List for iteration
        private readonly HashSet<ProjectilePresenter> _activeSet = new();
        private readonly List<ProjectilePresenter> _activeList = new();

        public ProjectileSpawner(GameObject prefab)
        {
            _pool = new ObjectPool<ProjectilePresenter>(
                createFunc: () => Object.Instantiate(prefab).GetComponent<ProjectilePresenter>(),
                actionOnGet: p     => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => Object.Destroy(p.gameObject),
                defaultCapacity: 16
            );
        }

        public void Spawn(Vector3 position, Vector3 direction, float speed)
        {
            ProjectilePresenter projectile = _pool.Get();
            _activeList.Add(projectile);
            _activeSet.Add(projectile);
            projectile.OnSpawned(position, direction, speed);
        }

        public void Despawn(ProjectilePresenter projectile)
        {
            // O(1) lookup instead of O(n) List.Contains
            if (!projectile || !_activeSet.Contains(projectile)) return;

            projectile.OnDespawned();
            _pool.Release(projectile);
            _activeSet.Remove(projectile);
            _activeList.Remove(projectile);
        }

        public void DespawnAll()
        {
            foreach (ProjectilePresenter projectilePresenter in _activeList)
            {
                projectilePresenter.OnDespawned();
                _pool.Release(projectilePresenter);
            }

            _activeList.Clear();
            _activeSet.Clear();
        }
    }
}