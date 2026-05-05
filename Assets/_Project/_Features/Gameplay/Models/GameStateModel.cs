namespace SpaceshipBattle.Models
{
    public enum GameState
    {
        Loading,
        Menu,
        Gameplay,
        Results,
        Scores
    }

    public class GameStateModel : SpaceshipBattle.Helpers.StateModel<GameState>
    {
        public GameStateModel() : base(GameState.Loading) { }
    }
}