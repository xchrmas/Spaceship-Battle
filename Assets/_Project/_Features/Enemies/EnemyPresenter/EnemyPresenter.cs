using System;
using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using SpaceshipBattle.Services;
using UnityEngine;

namespace SpaceshipBattle.Presenters
{
    /// <summary>
    /// Handles enemy visuals, hit detection and death logic.
    /// Notifies EnemyStateMachine on death so the formation can escalate.
    /// </summary>
    public class EnemyPresenter : MonoBehaviour, IDisposable
    {
        [SerializeField] private GameObject[] _models;

        private IAudioService _audioService;
        private IEnemiesManager _enemiesManager;
        private EnemyConfig _enemyConfig;
        private EnemyModel _enemy;
        private IEnemySpawner _enemySpawner;
        private IExplosionSpawner _explosionSpawner;
        private GameplayModel _gameplay;
        private IProjectileSpawner _projectileSpawner;

        private void Awake()
        {
            _audioService = ServiceLocator.Get<IAudioService>();
            _enemyConfig = ServiceLocator.Get<EnemyConfig>();
            _enemy = new EnemyModel(_enemyConfig);
            _enemySpawner = ServiceLocator.Get<IEnemySpawner>();
            _enemiesManager = ServiceLocator.Get<IEnemiesManager>();
            _explosionSpawner = ServiceLocator.Get<IExplosionSpawner>();
            _gameplay = ServiceLocator.Get<GameplayModel>();
            _projectileSpawner = ServiceLocator.Get<IProjectileSpawner>();
        }

        public Vector3 Position => _enemy.Position;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out ProjectilePresenter projectile)) return;

            _gameplay.CurrentScore += (_enemy.Type + 1) * _enemyConfig.BaseScore;
            _audioService.PlaySfx(Constants.Audio.Explosion, GameConstants.Audio.ExplosionVolume);
            _explosionSpawner.Spawn(transform.position);
            _projectileSpawner.Despawn(projectile);
            _enemySpawner.Despawn(this);

            // Tell FSM an enemy died — may trigger Aggressive state
            _enemiesManager.OnEnemyDied(_enemySpawner.Enemies.Count);
        }

        /// <summary>
        /// Called by EnemySpawner when taking from pool.
        /// </summary>
        public void OnSpawned(int type, int row, int col)
        {
            _enemy.Init(type, row, col);
            _models[_enemy.Type].SetActive(true);
        }

        /// <summary>
        /// Called by EnemySpawner when returning to pool.
        /// </summary>
        public void OnDespawned()
        {
            _models[_enemy.Type].SetActive(false);
            _enemy.Reset();
        }

        public void Dispose() { }
    }
}