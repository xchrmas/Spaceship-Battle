using DG.Tweening;
using SpaceshipBattle.Core;
using UnityEngine;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// DOTween-based camera shake. Caches original position
    /// so it always snaps back cleanly after the shake.
    /// </summary>
    public class CameraShaker : ICameraShaker
    {
        private readonly Transform _camTransform;
        private readonly Vector3   _originalPos;

        public CameraShaker()
        {
            _camTransform = Camera.main.transform;
            _originalPos  = _camTransform.position;
        }

        public void Shake(float duration, float strength)
        {
            if (_camTransform == null) return;

            _camTransform
                .DOShakePosition(duration, strength, GameConstants.Camera.ShakeVibrato)
                .OnComplete(() => _camTransform.position = _originalPos);
        }
    }
}