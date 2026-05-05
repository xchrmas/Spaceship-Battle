using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpaceshipBattle.Models
{
    /// <summary>
    /// Player tuning values. Exposed in the Inspector via GameConfig.
    /// </summary>
    [Serializable]
    public class PlayerConfig
    {
        [Tooltip("Number of lives.")]
        public int Lives = 3;

        [Tooltip("Duration of invulnerability in seconds.")]
        public float Invulnerability = 3.0f;

        [Tooltip("Horizontal speed in m/s.")]
        public float Speed = 5.0f;

        [Tooltip("Time between shots in seconds.")]
        public float FireRate = 0.5f;

        [Tooltip("Speed of a player projectile in m/s.")]
        public float ProjectileSpeed = 15.0f;

        [Tooltip("Spawn position at game start.")]
        public Vector3 SpawnPosition = new(0f, 0f, -10f);
    }

    /// <summary>
    /// Level boundary config. Handles aspect ratio scaling automatically.
    /// </summary>
    [Serializable]
    public class LevelConfig
    {
        public Vector3 Bounds = new(20f, 0f, 11f);

        /// <summary>
        /// Scales horizontal bounds by current aspect ratio
        /// so the game feels the same on any screen.
        /// </summary>
        public bool IsPosOutOfHorizontalBounds(Vector3 pos)
        {
            var refAspect     = 16f / 9f;
            var currentAspect = Screen.width / (float)Screen.height;
            var boundX        = Bounds.x * (currentAspect / refAspect);
            return pos.x > boundX || pos.x < -boundX;
        }

        public bool IsPosOutOfVerticalBounds(Vector3 pos) =>
            pos.z > Bounds.z || pos.z < -Bounds.z;
    }

    /// <summary>
    /// Player state — position, lives, invulnerability.
    /// Uses C# events instead of ReactiveProperty to avoid UniRx dependency.
    /// </summary>
    public class PlayerModel
    {
        private readonly PlayerConfig _playerConfig;
        private readonly LevelConfig  _levelConfig;

        private Vector3 _position;
        private int _lives;
        private bool _isInvulnerable;
        private float _shotTime;

        public event Action<Vector3> OnPositionChanged;
        public event Action<int> OnLivesChanged;
        public event Action<bool> OnInvulnerableChanged;

        public PlayerModel(PlayerConfig playerConfig, LevelConfig levelConfig)
        {
            _playerConfig = playerConfig;
            _levelConfig  = levelConfig;
            _shotTime = float.MinValue;
            _position = playerConfig.SpawnPosition;
            _lives  = playerConfig.Lives;
            _isInvulnerable = false;
        }

        public Vector3 Position
        {
            get => _position;
            private set { _position = value; OnPositionChanged?.Invoke(value); }
        }

        public int Lives
        {
            get => _lives;
            private set { _lives = value; OnLivesChanged?.Invoke(value); }
        }

        public bool IsInvulnerable
        {
            get => _isInvulnerable;
            private set { _isInvulnerable = value; OnInvulnerableChanged?.Invoke(value); }
        }

        public bool IsDead => _lives <= 0;

        /// <summary>
        /// Resets to initial state. Called at the start of each game.
        /// </summary>
        public void Reset()
        {
            Position = _playerConfig.SpawnPosition;
             Lives = _playerConfig.Lives;
            _shotTime  = float.MinValue;
            IsInvulnerable = false;
        }

        /// <summary>
        /// Moves the player horizontally. Stops at level bounds.
        /// </summary>
        public void Move(float horizontal, float dt)
        {
            var delta = horizontal * dt * _playerConfig.Speed * Vector3.right;
            if (_levelConfig.IsPosOutOfHorizontalBounds(Position + delta)) return;
            Position += delta;
        }

        /// <summary>
        /// Returns true if enough time has passed since the last shot.
        /// </summary>
        public bool Shoot(float currentTime)
        {
            if (currentTime < _shotTime + _playerConfig.FireRate) return false;
            _shotTime = currentTime;
            return true;
        }

        /// <summary>
        /// Reduces lives by 1 and starts invulnerability timer.
        /// Does nothing if already dead or invulnerable.
        /// </summary>
        public async UniTask<bool> DamageAsync()
        {
            if (IsDead || IsInvulnerable) return false;

            Lives--;
            IsInvulnerable = true;

            await UniTask.Delay(TimeSpan.FromSeconds(_playerConfig.Invulnerability));

            IsInvulnerable = false;
            return true;
        }
    }
}