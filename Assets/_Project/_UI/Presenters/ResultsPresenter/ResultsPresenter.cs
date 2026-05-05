using System;
using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceshipBattle.Presenters
{
    public class ResultsPresenter : MonoBehaviour
    {
        [SerializeField] private Button _buttonBack;
        [SerializeField] private TextMeshProUGUI _labelScore;
        [SerializeField] private TextMeshProUGUI _labelWaves;

        private GameplayModel  _gameplay;
        private GameStateModel _gameState;
        private ScoresModel    _scores;

        private void Awake()
        {
            _gameplay  = ServiceLocator.Get<GameplayModel>();
            _gameState = ServiceLocator.Get<GameStateModel>();
            _scores    = ServiceLocator.Get<ScoresModel>();
        }

        private void OnEnable()
        {
            _buttonBack.onClick.AddListener(OnBackClicked);
            _gameState.OnStateChanged += OnStateChanged;
            _gameplay.OnScoreChanged  += OnScoreChanged;
            _gameplay.OnWaveChanged   += OnWaveChanged;


            _labelScore.text = _gameplay.CurrentScore.ToString();
            _labelWaves.text = (_gameplay.CurrentWave - 1).ToString();
        }

        private void OnDisable()
        {
            _buttonBack.onClick.RemoveListener(OnBackClicked);
            _gameState.OnStateChanged -= OnStateChanged;
            _gameplay.OnScoreChanged  -= OnScoreChanged;
            _gameplay.OnWaveChanged   -= OnWaveChanged;
        }

        private void OnBackClicked() => _gameState.State = GameState.Menu;

        private void OnStateChanged(GameState prev, GameState current)
        {
            if (current == GameState.Results && _gameplay.CurrentScore > 0)
                AddAndSaveScore();
        }

        private void OnScoreChanged(int score)
        {
            _labelScore.text = score.ToString();
        }

        private void OnWaveChanged(int wave)
        {
            _labelWaves.text = (wave - 1).ToString();
        }

        private void AddAndSaveScore()
        {
            _scores.Add(new ScoreItem
            {
                Score = _gameplay.CurrentScore,
                Date  = DateTime.Now.ToString("MM/dd/yyyy HH:mm")
            });
            _scores.Save();
        }
    }
}