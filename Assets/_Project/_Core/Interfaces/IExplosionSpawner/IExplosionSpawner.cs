using SpaceshipBattle.Presenters;
using UnityEngine;

namespace SpaceshipBattle.Services
{
    public interface IExplosionSpawner
    {
        void Despawn(ExplosionPresenter explosion);
        void DespawnAll();
        void Spawn(Vector3 position);
    }
}
