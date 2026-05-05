using UnityEngine;

namespace SpaceshipBattle.StateMachines
{
    /// <summary>
    /// Triggered when enemy HP drops below AggressionThreshold.
    /// Moves faster and shoots more frequently — keeps pressure on the player.
    /// </summary>
    public class AggressiveState : IEnemyState
    {
        // 30% faster movement
        public float SpeedMultiplier => 1.3f;

        // 40% faster shooting — FireRate is a delay so lower = faster
        public float FireRateMultiplier => 0.6f;

        public void Enter()
        {
            Debug.Log("[EnemyFSM] → Aggressive");
        }

        public void Exit()
        {
            Debug.Log("[EnemyFSM] Aggressive →");
        }
    }
}