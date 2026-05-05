using SpaceshipBattle.Core;
using SpaceshipBattle.Services;
using UnityEngine;
using UnityEngine.UI;
using SpaceshipBattle.Models;

namespace SpaceshipBattle.Helpers
{
    [RequireComponent(typeof(Button))]
    public class ButtonSfx : MonoBehaviour
    {
        IAudioService _audioService;
        Button        _button;

        void Awake()
        {
            _audioService = ServiceLocator.Get<IAudioService>();
            _button       = GetComponent<Button>();
        }

        void OnEnable()  => _button.onClick.AddListener(OnClick);
        void OnDisable() => _button.onClick.RemoveListener(OnClick);

        void OnClick() => _audioService.PlaySfx(Constants.Audio.Click, 1.0f);
    }
}