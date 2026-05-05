using SpaceshipBattle.Models;
using SpaceshipBattle.Presenters;
using UnityEngine;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Instantiates the player from an Addressable prefab.
    /// Only one player exists at a time — Spawn does nothing if already alive.
    /// </summary>
    public class PlayerSpawner : IPlayerSpawner
    {
        private readonly IAssetService _assetService;
        private PlayerPresenter _player;

        public PlayerSpawner(IAssetService assetService)
        {
            _assetService = assetService;
        }

        public void Spawn()
        {
            if (_player) return;

            var prefab = _assetService.Get<GameObject>(Constants.Objects.Player);
            if (!prefab) { Debug.LogError("[PlayerSpawner] Player prefab not found."); return; }

            _player = Object.Instantiate(prefab).GetComponent<PlayerPresenter>();
            _player.gameObject.SetActive(true);
        }

        /// <summary>
        /// Destroys the player. Called on game over or scene transition.
        /// </summary>
        public void Despawn()
        {
            if (!_player) return;
            Object.Destroy(_player.gameObject);
            _player = null;
        }
    }
}