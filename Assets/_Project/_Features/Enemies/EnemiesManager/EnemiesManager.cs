using SpaceshipBattle.Models;
using SpaceshipBattle.StateMachines;
using UnityEngine;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Controls enemy formation movement, direction and shoot timing.
    /// Uses EnemyStateMachine to scale speed and fire rate as enemies die.
    /// </summary>
    public class EnemiesManager : IEnemiesManager
    {
        private readonly EnemyConfig  _enemyConfig;
        private readonly LevelConfig  _levelConfig;

        private EnemyStateMachine _fsm;
        private float  _lastShotTime;

        public Vector3 Position  { get; private set; }
        public Vector3 Direction { get; private set; }

        public EnemiesManager(LevelConfig levelConfig, EnemyConfig enemyConfig)
        {
            _levelConfig = levelConfig;
            _enemyConfig = enemyConfig;

            Position  = Vector3.zero;
            Direction = Vector3.right;
        }

        public void Reset()
        {
             Position = Vector3.zero;
             Direction = Vector3.right;
            _lastShotTime = 0;

            var total = _enemyConfig.Columns * _enemyConfig.Rows;
            _fsm = new EnemyStateMachine(total);
        }

        /// <summary>
        /// Call when an enemy dies. FSM decides if state should change.
        /// </summary>
        public void OnEnemyDied(int remainingEnemies)
        {
            _fsm?.OnEnemyDied(remainingEnemies);
        }

        /// <summary>
        /// Moves formation. Speed is scaled by FSM state multiplier.
        /// </summary>
        public void Move(Vector3 leftPos, Vector3 rightPos, int enemyCount, float dt)
        {
            var hitRight = Direction.x > 0 && _levelConfig.IsPosOutOfHorizontalBounds(rightPos);
            var hitLeft  = Direction.x < 0 && _levelConfig.IsPosOutOfHorizontalBounds(leftPos);

            if (hitRight || hitLeft)
            {
                Direction *= -1f;
                Position  += _enemyConfig.SpeedVertical * Vector3.back;
            }

            var percent = 1 - enemyCount / (float)(_enemyConfig.Columns * _enemyConfig.Rows);
            var baseSpeed = (1 + percent * _enemyConfig.SpeedMultiplier) * _enemyConfig.SpeedHorizontal;

            // FSM multiplier on top of base speed
            float fsmMultiplier = _fsm?.SpeedMultiplier ?? 1f;
            float finalSpeed    = baseSpeed * fsmMultiplier;

            Position += dt * finalSpeed * Direction;
        }

        /// <summary>
        /// Returns true once per FireRate interval.
        /// Fire rate is scaled by FSM state multiplier.
        /// </summary>
        public bool Shoot(float time)
        {
            float fsmMultiplier  = _fsm?.FireRateMultiplier ?? 1f;
            float adjustedRate   = _enemyConfig.FireRate * fsmMultiplier;

            if (time < _lastShotTime + adjustedRate) return false;
            _lastShotTime = time;
            return true;
        }
    }
}