using UnityEngine;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Controls the enemy formation — movement, direction reversal and shoot timing.
    /// Enemies get faster as their count drops.
    /// </summary>
    public interface IEnemiesManager
    {
        Vector3 Position  { get; }
        Vector3 Direction { get; }

        void Reset();

        void Move(Vector3 leftPos, Vector3 rightPos, int enemyCount, float dt);

        bool Shoot(float time);

        /// <summary>
        /// Notifies the FSM that an enemy died.
        /// Triggers state transition if threshold is crossed.
        /// </summary>
        void OnEnemyDied(int remainingEnemies);
    }
}