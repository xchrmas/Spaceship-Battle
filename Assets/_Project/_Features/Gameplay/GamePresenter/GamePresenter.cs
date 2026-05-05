using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using SpaceshipBattle.Services;
using UnityEngine;

namespace SpaceshipBattle.Presenters
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _background;

        private IAssetService _assetService;
        private AudioConfig  _audioConfig;
        private IAudioService _audioService;
        private IEnemiesManager _enemiesManager;
        private EnemyConfig _enemyConfig;
        private IEnemySpawner _enemySpawner;
        private GameplayModel  _gameplay;
        private GameStateModel _gameState;
        private LevelConfig _levelConfig;
        private IPlayerSpawner _playerSpawner;
        private IProjectileSpawner _projectileSpawner;

        private void Awake()
        {
            _assetService = ServiceLocator.Get<IAssetService>();
            _audioConfig  = ServiceLocator.Get<AudioConfig>();
            _audioService = ServiceLocator.Get<IAudioService>();

            _enemiesManager = ServiceLocator.Get<IEnemiesManager>();
            _enemyConfig    = ServiceLocator.Get<EnemyConfig>();
            _enemySpawner   = ServiceLocator.Get<IEnemySpawner>();

            _gameplay    = ServiceLocator.Get<GameplayModel>();
            _gameState   = ServiceLocator.Get<GameStateModel>();
            _levelConfig = ServiceLocator.Get<LevelConfig>();

            _playerSpawner     = ServiceLocator.Get<IPlayerSpawner>();
            _projectileSpawner = ServiceLocator.Get<IProjectileSpawner>();
        }

        private void OnEnable()  => _gameState.OnStateChanged += OnStateChanged;
        private void OnDisable() => _gameState.OnStateChanged -= OnStateChanged;

        private void Start()
        {
            _background.sharedMaterial = _assetService.Get<Material>(Constants.Materials.Background);
            _background.enabled        = true;
        }

        private void Update()
        {
            if (_gameState.State == GameState.Gameplay)
                GameplayLoop();
        }

        private void OnStateChanged(GameState prev, GameState current)
        {
            if (current == GameState.Gameplay) OnGameplayStarted();
            else if (prev == GameState.Gameplay) OnGameplayEnded();
        }

        private void GameplayLoop()
        {
            HandleEnemyWaves();
            HandleEnemyMovement();
            HandleEnemyShooting();
            DetectEndGame();
        }

        private void HandleEnemyWaves()
        {
            if (_enemySpawner.Enemies.Count != 0) return;
            _enemiesManager.Reset();
            _enemySpawner.SpawnAll();
            _gameplay.CurrentWave++;
        }

        private void HandleEnemyMovement()
        {
            EnemyPresenter first = _enemySpawner.Enemies[0];
            EnemyPresenter last  = _enemySpawner.Enemies[^1];

            _enemiesManager.Move(
                first.transform.position,
                last.transform.position,
                _enemySpawner.Enemies.Count,
                Time.deltaTime);

            for (int i = 0; i < _enemySpawner.Enemies.Count; i++)
            {
                EnemyPresenter enemy = _enemySpawner.Enemies[i];
                enemy.transform.position = _enemiesManager.Position + enemy.Position;
            }
        }

        private void HandleEnemyShooting()
        {
            if (!_enemiesManager.Shoot(Time.time)) return;

            int   index  = Random.Range(0, _enemySpawner.Enemies.Count);
            float posX   = _enemySpawner.Enemies[index].transform.position.x;
            Vector3 lowestPos = Vector3.one * float.MaxValue;

            for (int i = 0; i < _enemySpawner.Enemies.Count; i++)
            {
                Vector3 pos = _enemySpawner.Enemies[i].transform.position;
                if (Mathf.Approximately(pos.x, posX) && pos.z < lowestPos.z)
                    lowestPos = pos;
            }

            _projectileSpawner.Spawn(
                lowestPos + Vector3.back * GameConstants.Combat.ProjectileSpawnOffset,
                Vector3.back,
                _enemyConfig.ProjectileSpeed);

            _audioService.PlaySfx(Constants.Audio.Blaster, GameConstants.Audio.BlasterVolume);
        }

        private void DetectEndGame()
        {
            Vector3 lowestPos = Vector3.one * float.MaxValue;
            for (int i = 0; i < _enemySpawner.Enemies.Count; i++)
            {
                Vector3 pos = _enemySpawner.Enemies[i].transform.position;
                if (pos.z < lowestPos.z) lowestPos = pos;
            }

            if (_levelConfig.IsPosOutOfVerticalBounds(lowestPos))
                _gameState.State = GameState.Results;
        }

        private void OnGameplayStarted()
        {
            _playerSpawner.Spawn();
            _enemiesManager.Reset();
            _audioService.PlayMusic(Constants.Audio.Music, _audioConfig.MusicVolume);
        }

        private void OnGameplayEnded()
        {
            _playerSpawner.Despawn();
            _enemySpawner.DespawnAll();
            _projectileSpawner.DespawnAll();
            _audioService.StopMusic();
        }
    }
}