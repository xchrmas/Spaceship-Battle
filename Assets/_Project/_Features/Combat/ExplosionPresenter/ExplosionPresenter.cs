using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SpaceshipBattle.Core;
using SpaceshipBattle.Services;
using UnityEngine;

namespace SpaceshipBattle.Presenters
{
    public class ExplosionPresenter : MonoBehaviour, IDisposable
    {
        [SerializeField] private ParticleSystem _particle;

        private IExplosionSpawner _spawner;
        private CancellationTokenSource _cts;

        private void Awake()
        {
            _spawner = ServiceLocator.Get<IExplosionSpawner>();
        }

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public async void OnSpawned(Vector3 position)
        {
            transform.position = position;
            _particle.Play();

            try
            {
                await UniTask.Delay(
                    Mathf.RoundToInt(_particle.main.duration * 1000),
                    cancellationToken: _cts.Token
                );
                _spawner.Despawn(this);
            }
            catch (OperationCanceledException)
            {
                // The object was returned to the pool before the time expired
            }
        }

        public void OnDespawned()
        {
            _particle.Stop();
            transform.position = Vector3.zero;
        }

        public void Dispose() { }
    }
}