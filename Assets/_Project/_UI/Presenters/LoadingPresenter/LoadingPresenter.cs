using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SpaceshipBattle.Core;
using SpaceshipBattle.Models;
using SpaceshipBattle.Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceshipBattle.Presenters
{
    public class LoadingPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _labelLoading;

        private IAssetService  _assetService;
        private GameStateModel _gameState;
        private ScoresModel    _scores;

        // Custom CancellationTokenSource - not tied to object destruction.
        private CancellationTokenSource _cts;
        private bool _loadingComplete;

        private readonly string[] _loadingTexts =
        {
            "Loading",
            "Loading.",
            "Loading..",
            "Loading..."
        };

        private void Awake()
        {
            _assetService = ServiceLocator.Get<IAssetService>();
            _gameState    = ServiceLocator.Get<GameStateModel>();
            _scores       = ServiceLocator.Get<ScoresModel>();

            // Own token - cancellation is only possible manually.
            _cts = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            if (!_loadingComplete) _cts?.Cancel();
            _cts?.Dispose();
        }

        private async void Start()
        {
            DOTween.Init();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            _scores.Load();

            AnimateLoadingTextAsync(_cts.Token).Forget();

            try
            {
                await LoadAssetsAsync(_cts.Token);

                // Register spawners after loading
                GameBootstrap.Instance.RegisterSpawners();

                _loadingComplete = true;

                // Load the next scene
                await SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).ToUniTask(cancellationToken: _cts.Token);

                _gameState.State = GameState.Menu;
            }
            catch (System.OperationCanceledException)
            {
                // Normal - object destroyed before completion
                Debug.Log("[LoadingPresenter] Loading cancelled.");
            }
        }

        async UniTaskVoid AnimateLoadingTextAsync(CancellationToken ct)
        {
            var tick = 0;
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    // Check if the object already exists before accessing it, which can happen if the scene is unloaded during loading.
                    if (this == null || !gameObject.activeSelf) break;

                    _labelLoading.text = _loadingTexts[tick % _loadingTexts.Length];
                    tick++;
                    await UniTask.Delay(250, cancellationToken: ct);
                }
                catch (System.OperationCanceledException)
                {
                    break;
                }
                catch (MissingReferenceException)
                {
                    break;
                }
            }
        }

        private async UniTask LoadAssetsAsync(CancellationToken cancellationToken)
        {
            await _assetService.Load<GameObject>(Constants.Objects.Enemy,cancellationToken);
            await _assetService.Load<GameObject>(Constants.Objects.Player,cancellationToken);
            await _assetService.Load<GameObject>(Constants.Objects.Projectile,cancellationToken);
            await _assetService.Load<GameObject>(Constants.Objects.Blast,cancellationToken);

            await _assetService.Load<AudioClip>(Constants.Audio.Blaster,cancellationToken);
            await _assetService.Load<AudioClip>(Constants.Audio.Explosion,cancellationToken);
            await _assetService.Load<AudioClip>(Constants.Audio.Click,cancellationToken);
            await _assetService.Load<AudioClip>(Constants.Audio.Music,cancellationToken);

            await _assetService.Load<Material>(Constants.Materials.Background, cancellationToken);
        }
    }
}