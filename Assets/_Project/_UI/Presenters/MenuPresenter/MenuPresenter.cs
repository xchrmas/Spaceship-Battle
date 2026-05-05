using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceshipBattle.Presenters
{
    public class MenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button _buttonStart;
        [SerializeField] private Button _buttonScores;

        private GameStateModel _gameState;

        private void Awake()
        {
            _gameState = ServiceLocator.Get<GameStateModel>();
        }

        private void OnEnable()
        {
            _buttonStart.onClick.AddListener(OnStartClicked);
            _buttonScores.onClick.AddListener(OnScoresClicked);
        }

        private void OnDisable()
        {
            _buttonStart.onClick.RemoveListener(OnStartClicked);
            _buttonScores.onClick.RemoveListener(OnScoresClicked);
        }

        private void OnStartClicked()  => _gameState.State = GameState.Gameplay;
        private void OnScoresClicked() => _gameState.State = GameState.Scores;
    }
}