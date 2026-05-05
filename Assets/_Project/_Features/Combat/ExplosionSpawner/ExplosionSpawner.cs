using System.Collections.Generic;
using SpaceshipBattle.Presenters;
using UnityEngine;
using UnityEngine.Pool;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Pooled explosion effects. Default capacity of 8 is enough
    /// since explosions are short-lived and rarely overlap.
    /// </summary>
    public class ExplosionSpawner : IExplosionSpawner
    {
        private readonly ObjectPool<ExplosionPresenter> _pool;

        private readonly List<ExplosionPresenter> _active = new();


        public ExplosionSpawner(GameObject prefab)
        {
            _pool = new ObjectPool<ExplosionPresenter>(
                createFunc:  () => Object.Instantiate(prefab).GetComponent<ExplosionPresenter>(),
                actionOnGet:  e => e.gameObject.SetActive(true),
                actionOnRelease: e => e.gameObject.SetActive(false),
                actionOnDestroy: e => Object.Destroy(e.gameObject),
                defaultCapacity: 8
            );
        }

        public void Spawn(Vector3 position)
        {
            var explosion = _pool.Get();
            _active.Add(explosion);
            explosion.OnSpawned(position);
        }

        public void Despawn(ExplosionPresenter explosion)
        {
            if (explosion == null || !_active.Contains(explosion)) return;
            explosion.OnDespawned();

            _pool.Release(explosion);
            _active.Remove(explosion);
        }

        public void DespawnAll()
        {
            foreach (var e in _active) { e.OnDespawned(); _pool.Release(e); }
            _active.Clear();
        }
    }
}