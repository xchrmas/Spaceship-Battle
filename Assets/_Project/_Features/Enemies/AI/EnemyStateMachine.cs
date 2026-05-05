using UnityEngine;

namespace SpaceshipBattle.StateMachines
{
    /// <summary>
    /// Manages enemy state transitions based on remaining enemy count.
    /// When fewer than AggressionThreshold percent of enemies remain,
    /// switches from Patrol to Aggressive.
    /// </summary>
    public class EnemyStateMachine
    {
        // Switch to Aggressive when this fraction of enemies remains
        private const float AggressionThreshold = 0.5f;

        private readonly int _totalEnemies;

        private IEnemyState _currentState;

        public IEnemyState CurrentState => _currentState;

        public float SpeedMultiplier    => _currentState.SpeedMultiplier;
        public float FireRateMultiplier => _currentState.FireRateMultiplier;

        public EnemyStateMachine(int totalEnemies)
        {
            _totalEnemies = totalEnemies;
            TransitionTo(new PatrolState());
        }

        /// <summary>
        /// Call every time an enemy dies.
        /// Automatically transitions to Aggressive when threshold is crossed.
        /// </summary>
        public void OnEnemyDied(int remainingEnemies)
        {
            float remainingPercent = remainingEnemies / (float)_totalEnemies;

            bool shouldBeAggressive  = remainingPercent <= AggressionThreshold;
            bool isAlreadyAggressive = _currentState is AggressiveState;

            if (shouldBeAggressive && !isAlreadyAggressive)
                TransitionTo(new AggressiveState());
        }

        /// <summary>
        /// Resets to Patrol state. Call at the start of each new wave.
        /// </summary>
        public void Reset()
        {
            TransitionTo(new PatrolState());
        }

        private void TransitionTo(IEnemyState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}