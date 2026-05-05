using SpaceshipBattle.Core;
using SpaceshipBattle.Helpers;
using SpaceshipBattle.Models;

namespace SpaceshipBattle.Presenters
{
    public class GameStateReactor : StateReactor<GameState>
    {
        private GameStateModel _gameState;

        private void Awake()
        {
            _gameState = ServiceLocator.Get<GameStateModel>();
        }

        protected override StateModel<GameState> Model => _gameState;
    }
}