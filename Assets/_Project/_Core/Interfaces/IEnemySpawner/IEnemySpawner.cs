using System.Collections.Generic;
using SpaceshipBattle.Presenters;

namespace SpaceshipBattle.Services
{
    public interface IEnemySpawner
    {
        List<EnemyPresenter> Enemies { get; }

        void Despawn(EnemyPresenter enemy);
        void DespawnAll();
        void SpawnAll();
    }
}
