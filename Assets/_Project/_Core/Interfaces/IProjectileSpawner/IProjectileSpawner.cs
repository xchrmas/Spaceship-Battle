using SpaceshipBattle.Presenters;
using UnityEngine;

namespace SpaceshipBattle.Services
{
    public interface IProjectileSpawner
    {
        void Despawn(ProjectilePresenter projectile);
        void DespawnAll();
        void Spawn(Vector3 position, Vector3 direction, float speed);
    }
}
