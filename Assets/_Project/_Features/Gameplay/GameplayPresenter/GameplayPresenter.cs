using DG.Tweening;
using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceshipBattle.Presenters
{
    public class GameplayPresenter : MonoBehaviour
    {
        [SerializeField] private Button _buttonExit;
        [SerializeField] private TextMeshProUGUI _labelScore;
        [SerializeField] private TextMeshProUGUI _labelWave;
        [SerializeField] private TextMeshProUGUI _labelLives;
        [SerializeField] private Image _imageVignette;

        private GameplayModel  _gameplay;
        private GameStateModel _gameState;
        private PlayerModel _player;

        private int _prevLives;
        private int _prevScore;

        private void Awake()
        {
            _gameplay  = ServiceLocator.Get<GameplayModel>();
            _gameState = ServiceLocator.Get<GameStateModel>();
            _player    = ServiceLocator.Get<PlayerModel>();
        }

        private void OnEnable()
        {
            _buttonExit.onClick.AddListener(OnExitClicked);
            _gameState.OnStateChanged += OnStateChanged;
            _gameplay.OnScoreChanged += OnScoreChanged;
            _gameplay.OnWaveChanged += OnWaveChanged;
            _player.OnLivesChanged += OnLivesChanged;

            _labelScore.text = _gameplay.CurrentScore.ToString();
            _labelWave.text  = _gameplay.CurrentWave.ToString();
            _labelLives.text = _player.Lives.ToString();
            _prevLives       = _player.Lives;
            _prevScore       = _gameplay.CurrentScore;
        }

        private void OnDisable()
        {
            _buttonExit.onClick.RemoveListener(OnExitClicked);
            _gameState.OnStateChanged -= OnStateChanged;
            _gameplay.OnScoreChanged  -= OnScoreChanged;
            _gameplay.OnWaveChanged   -= OnWaveChanged;
            _player.OnLivesChanged    -= OnLivesChanged;
        }

        private void OnExitClicked() => _gameState.State = GameState.Menu;

        private void OnStateChanged(GameState prev, GameState current)
        {
            if (current == GameState.Gameplay) _gameplay.Reset();
        }

        private void OnScoreChanged(int score)
        {
            _labelScore.text = score.ToString();

            if (score > _prevScore)
            {
                _labelScore.rectTransform
                    .DOPunchScale(
                        new Vector3(
                            GameConstants.Animation.ScorePunchScale,
                            GameConstants.Animation.ScorePunchScale,
                            0f),
                        GameConstants.Animation.ScorePunchDuration)
                    .SetEase(Ease.OutQuint)
                    .OnComplete(() => _labelScore.rectTransform.localScale = Vector3.one);
            }

            _prevScore = score;
        }

        private void OnWaveChanged(int wave) => _labelWave.text = wave.ToString();

        private void OnLivesChanged(int lives)
        {
            _labelLives.text = lives.ToString();

            if (lives < _prevLives)
            {
                _imageVignette.color = new Color(1f, 1f, 1f, 0f);
                _imageVignette
                    .DOFade(1f, GameConstants.Animation.VignetteFadeDuration)
                    .SetLoops(GameConstants.Animation.VignetteLoops, LoopType.Yoyo)
                    .SetEase(Ease.OutQuint);
            }

            _prevLives = lives;
        }
    }
}