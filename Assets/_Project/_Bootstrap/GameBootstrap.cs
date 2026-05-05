using System;
using SpaceshipBattle.Models;
using SpaceshipBattle.Services;
using UnityEngine;

namespace SpaceshipBattle.Core
{
    /// <summary>
    /// Game entry point — registers all services before any scene starts.
    /// Survives scene loads via DontDestroyOnLoad.
    /// Attach to Bootstrap GameObject in the Loading scene.
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        private GameConfig _config;

        public static GameBootstrap Instance { get; private set; }

        private void Awake()
        {
            // Checking for a duplicate singleton instance, which can happen if the Loading scene is loaded more than once.
            if (Instance != null)
            { Destroy(gameObject); return; }
            Instance = this;

            DontDestroyOnLoad(gameObject);

            _config = GetComponent<GameConfig>();

            if (_config == null)
            {
                Debug.LogError("[GameBootstrap] GameConfig not found!");
                return;
            }

            RegisterAll();
        }

        /// <summary>
        /// Registration order.
        /// Spawners are separate.
        /// </summary>
        private void RegisterAll()
        {
            ServiceLocator.Clear();

            ServiceLocator.Register(_config.Enemy);
            ServiceLocator.Register(_config.Player);
            ServiceLocator.Register(_config.Level);
            ServiceLocator.Register(_config.Audio);

            var storage = new StorageService();
            var assets  = new AddressablesService();

            ServiceLocator.Register<IStorageService>(storage);
            ServiceLocator.Register<IAssetService>(assets);
            ServiceLocator.Register<IAudioService>(new AudioService(assets));
            ServiceLocator.Register<ICameraShaker>(new CameraShaker());

            ServiceLocator.Register(new GameStateModel());
            ServiceLocator.Register(new GameplayModel());
            ServiceLocator.Register(new InputModel());
            ServiceLocator.Register(new PlayerModel(_config.Player, _config.Level));
            ServiceLocator.Register(new ScoresModel(storage));

            ServiceLocator.Register<IEnemiesManager>(new EnemiesManager(_config.Level, _config.Enemy)); ServiceLocator.Register<IPlayerSpawner>(new PlayerSpawner(assets));
        }

        /// <summary>
        /// Called from LoadingPresenter after Addressables have finished loading.
        /// Spawners are in prefabs, so they are not in RegisterAll.
        /// </summary>
        public void RegisterSpawners()
        {
            var assets = ServiceLocator.Get<IAssetService>();
            var enemyConfig = ServiceLocator.Get<EnemyConfig>();

            ServiceLocator.Register<IEnemySpawner>(new EnemySpawner(assets.Get<GameObject>(Constants.Objects.Enemy),enemyConfig));
            ServiceLocator.Register<IProjectileSpawner>(new ProjectileSpawner(assets.Get<GameObject>(Constants.Objects.Projectile)));
            ServiceLocator.Register<IExplosionSpawner>(new ExplosionSpawner(assets.Get<GameObject>(Constants.Objects.Blast)));
        }


        private void OnDestroy()
        {
            if (ServiceLocator.TryGet<IAssetService>(out var assets))
                (assets as IDisposable)?.Dispose();

            ServiceLocator.Clear();
            Instance = null;
        }
    }
}