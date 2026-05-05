using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceshipBattle.Presenters
{
    public class ScoresPresenter : MonoBehaviour
    {
        [SerializeField] private Button _buttonBack;
        [SerializeField] private TextMeshProUGUI  _itemPrefab;
        [SerializeField] private VerticalLayoutGroup _itemHolder;


        private GameStateModel _gameState;
        private ScoresModel _scores;

        private readonly List<GameObject> _spawnedItems = new();

        private void Awake()
        {
            _gameState = ServiceLocator.Get<GameStateModel>();
            _scores    = ServiceLocator.Get<ScoresModel>();
        }

        private void OnEnable()
        {
            _buttonBack.onClick.AddListener(OnBackClicked);
            PopulateScores();
        }

        private void OnDisable()
        {
            _buttonBack.onClick.RemoveListener(OnBackClicked);
            ClearScores();
        }

        private void OnBackClicked() => _gameState.State = GameState.Menu;

        private void PopulateScores()
        {
            ClearScores();

            foreach (var item in _scores.Scoreboard)
            {
                var go  = Instantiate(_itemPrefab.gameObject, _itemHolder.transform);
                var tmp = go.GetComponent<TextMeshProUGUI>();

                if (tmp != null)
                    tmp.text = $"<color=#00ffff>{item.Score}</color> - {item.Date}";

                _spawnedItems.Add(go);
            }
        }

        private void ClearScores()
        {
            foreach (var go in _spawnedItems)
                Destroy(go);

            _spawnedItems.Clear();
        }
    }
}