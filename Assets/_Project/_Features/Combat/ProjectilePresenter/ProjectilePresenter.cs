using System;
using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using SpaceshipBattle.Services;
using UnityEngine;

namespace SpaceshipBattle.Presenters
{
    public class ProjectilePresenter : MonoBehaviour, IDisposable
    {
        [SerializeField] private Collider _collider;

        private bool _isDestroyed;
        private IAudioService  _audioService;
        private ProjectileModel _projectile;
        private IProjectileSpawner _projectileSpawner;


        private void Awake()
        {
            _audioService = ServiceLocator.Get<IAudioService>();
            _projectileSpawner = ServiceLocator.Get<IProjectileSpawner>();
            _projectile = new ProjectileModel(ServiceLocator.Get<LevelConfig>());
        }

        private void Update()
        {
            if (_projectile == null) return;

            if (_projectile.Move(Time.deltaTime))
                _projectileSpawner.Despawn(this);
            else
                transform.position = _projectile.Position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isDestroyed) return;
            if (!other.TryGetComponent(out ProjectilePresenter projectile)) return;
            _isDestroyed = true;

            _audioService.PlaySfx(Constants.Audio.Explosion, 0.15f);
            _projectileSpawner.Despawn(projectile);
            _projectileSpawner.Despawn(this);
        }

        public void OnSpawned(Vector3 position, Vector3 direction, float speed)
        {
            _projectile.Init(position, direction, speed);
            transform.position = _projectile.Position;
            transform.forward  = _projectile.Direction;
            _collider.enabled  = true;
        }

        public void OnDespawned()
        {
            _projectile.Reset();
            _collider.enabled = false;
        }

        public void Dispose() { }
    }
}