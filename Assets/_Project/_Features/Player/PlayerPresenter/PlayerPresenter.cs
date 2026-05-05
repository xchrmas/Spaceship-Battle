using System.Threading;
using Cysharp.Threading.Tasks;
using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using SpaceshipBattle.Services;
using UnityEngine;

namespace SpaceshipBattle.Presenters
{
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] private Light  _light;
        [SerializeField] private ParticleSystem _shield;
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private ParticleSystem _thrusterRight;
        [SerializeField] private ParticleSystem _thrusterLeft;

        private AudioConfig  _audioConfig;
        private IAudioService _audioService;
        private ICameraShaker _cameraShaker;
        private GameStateModel _gameState;
        private InputModel _input;
        private PlayerModel _player;
        private PlayerConfig _playerConfig;
        private IProjectileSpawner _projectileSpawner;

        CancellationTokenSource _cts;

        public PlayerPresenter(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
        }

        private void Awake()
        {
            _audioConfig = ServiceLocator.Get<AudioConfig>();
            _audioService = ServiceLocator.Get<IAudioService>();
            _cameraShaker = ServiceLocator.Get<ICameraShaker>();
            _gameState = ServiceLocator.Get<GameStateModel>();
            _input = ServiceLocator.Get<InputModel>();
            _player = ServiceLocator.Get<PlayerModel>();
            _playerConfig = ServiceLocator.Get<PlayerConfig>();
            _projectileSpawner = ServiceLocator.Get<IProjectileSpawner>();
        }

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            _player.OnPositionChanged  += OnPositionChanged;
            _player.OnInvulnerableChanged += ManageShield;
            _player.OnLivesChanged  += OnLivesChanged;
        }

        private void OnDisable()
        {
            _player.OnPositionChanged -= OnPositionChanged;
            _player.OnInvulnerableChanged -= ManageShield;
            _player.OnLivesChanged  -= OnLivesChanged;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void Start()
        {
            _player.Reset();
            transform.position = _player.Position;
        }

        private void Update()
        {
            Move();
            Shoot();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ProjectilePresenter projectile))
                OnHit(projectile);
        }

        private void OnPositionChanged(Vector3 pos) => transform.position = pos;

        private void OnLivesChanged(int lives)
        {
            if (lives <= 0)
                _gameState.State = GameState.Results;
        }

        private void ManageShield(bool isInvulnerable)
        {
            if (isInvulnerable && !_shield.isPlaying)       _shield.Play();
            else if (!isInvulnerable && _shield.isPlaying)  _shield.Stop();
        }

        private void Move()
        {
            _player.Move(_input.Horizontal, Time.deltaTime);

            switch (_input.Horizontal)
            {
                case > 0f:
                    StopThruster(_thrusterRight);
                    StartThruster(_thrusterLeft);
                    break;
                case < 0f:
                    StopThruster(_thrusterLeft);
                    StartThruster(_thrusterRight);
                    break;
                default:
                    StopThruster(_thrusterRight);
                    StopThruster(_thrusterLeft);
                    break;
            }
        }

        private void Shoot()
        {
            if (!_input.Fire || !_player.Shoot(Time.time)) return;

            Vector3 spawnPos = _player.Position
                + Vector3.forward * GameConstants.Combat.ProjectileSpawnOffset;

            _projectileSpawner.Spawn(spawnPos, Vector3.forward, _playerConfig.ProjectileSpeed);
            _audioService.PlaySfx(Constants.Audio.Blaster, GameConstants.Audio.BlasterVolume);
            FlashLight().Forget();
        }

        private void OnHit(ProjectilePresenter projectile)
        {
            _audioService.PlaySfx(Constants.Audio.Explosion, GameConstants.Audio.ExplosionVolume);

            if (!_player.IsInvulnerable)
            {
                _audioService.DuckMusic(
                    GameConstants.Audio.DuckTargetVolume,
                    _audioConfig.MusicVolume,
                    GameConstants.Audio.DuckDuration);

                _cameraShaker.Shake(
                    GameConstants.Camera.ShakeDuration,
                    GameConstants.Camera.ShakeStrength);
            }

            _projectileSpawner.Despawn(projectile);
            _player.DamageAsync().Forget();
        }

        private void StartThruster(ParticleSystem ps) { if (!ps.isPlaying) ps.Play(); }

        private void StopThruster(ParticleSystem ps)
        {
            if (ps.isPlaying) ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private async UniTaskVoid FlashLight()
        {
            _light.enabled = true;
            _muzzleFlash.Play(true);
            await UniTask.Delay(GameConstants.Vfx.MuzzleFlashDurationMs, cancellationToken: _cts.Token);
            _light.enabled = false;
        }
    }
}