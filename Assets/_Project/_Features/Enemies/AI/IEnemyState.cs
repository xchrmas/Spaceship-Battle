namespace SpaceshipBattle.StateMachines
{
    /// <summary>
    /// Contract for all enemy states.
    /// Each state controls movement speed multiplier and shoot rate multiplier.
    /// </summary>
    public interface IEnemyState
    {
        /// <summary>
        /// Called once when entering this state.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called once when leaving this state.
        /// </summary>
        void Exit();

        /// <summary>
        /// Multiplier applied to EnemyConfig.SpeedHorizontal.
        /// </summary>
        float SpeedMultiplier { get; }

        /// <summary>
        /// Multiplier applied to EnemyConfig.FireRate.
        /// Lower value = faster shooting.
        /// </summary>
        float FireRateMultiplier { get; }
    }
}