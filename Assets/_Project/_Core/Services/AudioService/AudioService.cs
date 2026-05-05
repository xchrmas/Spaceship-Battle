using System;
using DG.Tweening;
using SpaceshipBattle.Core;
using UnityEngine;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Music volume config. Exposed in the Inspector via GameConfig.
    /// </summary>
    [Serializable]
    public class AudioConfig
    {
        [Range(0f, 1f)]
        public float MusicVolume = 0.5f;
    }

    /// <summary>
    /// Handles SFX and music with DOTween fade transitions.
    /// NOTE: PlaySfx uses PlayClipAtPoint — fine for now,
    /// would need pooling for high-frequency sounds.
    /// </summary>
    public class AudioService : IAudioService
    {
        readonly IAssetService _assetService;

        private Transform  _camTransform;
        private AudioSource _music;
        private Tween  _tween;


        public AudioService(IAssetService assetService)
        {
            _assetService = assetService;
        }


        public void PlaySfx(string key, float volume)
        {
            AudioClip clip = _assetService.Get<AudioClip>(key);
            if (clip == null) { Debug.LogWarning($"AudioClip not found: {key}"); return; }

            if (_camTransform == null)
                _camTransform = Camera.main.transform;

            AudioSource.PlayClipAtPoint(clip, _camTransform.position, volume);
        }

        public void PlayMusic(string key, float volume)
        {
            AudioClip clip = _assetService.Get<AudioClip>(key);
            if (clip == null) { Debug.LogWarning($"AudioClip not found: {key}"); return; }

            if (_music == null)
            {
                var go = new GameObject("Music");
                _music = go.AddComponent<AudioSource>();
                _music.spatialBlend = GameConstants.Audio.SpatialBlendFlat;
                _music.loop  = true;
            }

            _tween?.Kill();
            _tween = _music
                .DOFade(volume, GameConstants.Audio.MusicFadeDuration)
                .SetEase(Ease.InQuad)
                .OnStart(() => { _music.clip = clip; _music.volume = 0f; _music.Play(); })
                .OnComplete(() => _music.volume = volume);
        }

        public void StopMusic()
        {
            if (_music == null) return;

            _tween?.Kill();
            _tween = _music
                .DOFade(0f, GameConstants.Audio.MusicFadeDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => { _music.volume = 0f; _music.Stop(); });
        }

        /// <summary>
        /// Briefly ducks the music then fades it back. Used on player hit.
        /// </summary>
        public void DuckMusic(float targetVolume, float originalVolume, float duration)
        {
            if (_music == null) return;

            _tween?.Kill();
            _tween = _music
                .DOFade(targetVolume, duration)
                .SetEase(Ease.OutQuad)
                .SetLoops(GameConstants.Animation.VignetteLoops, LoopType.Yoyo)
                .OnComplete(() => _music.volume = originalVolume);
        }
    }
}