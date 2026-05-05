using UnityEngine;

namespace SpaceshipBattle.StateMachines
{
    /// <summary>
    /// Default enemy state — normal speed, normal fire rate.
    /// Active when enemy HP is above the aggression threshold.
    /// </summary>
    public class PatrolState : IEnemyState
    {
        public float SpeedMultiplier => 1f;
        public float FireRateMultiplier => 1f;

        public void Enter()
        {
            Debug.Log("[EnemyFSM] → Patrol");
        }

        public void Exit()
        {
            Debug.Log("[EnemyFSM] Patrol →");
        }
    }
}