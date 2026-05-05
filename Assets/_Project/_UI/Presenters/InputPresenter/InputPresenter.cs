using SpaceshipBattle.Core;
using SpaceshipBattle.Helpers;
using SpaceshipBattle.Models;
using UnityEngine;

namespace SpaceshipBattle.Presenters
{
    /// <summary>
    /// Reads input from keyboard (PC/WebGL) and touch buttons (mobile).
    /// Keyboard always works — touch buttons are additive on top.
    /// </summary>
    public class InputPresenter : MonoBehaviour
    {
        private const string AxisHorizontal = "Horizontal";
        private const string ButtonFire     = "Fire1";

        [SerializeField] private ButtonAxis  _buttonHorizontal;
        [SerializeField] private ButtonTouch _buttonFire;

        private GameStateModel _gameState;
        private InputModel _input;

        private void Awake()
        {
            _gameState = ServiceLocator.Get<GameStateModel>();
            _input = ServiceLocator.Get<InputModel>();
        }

        private void Update()
        {
            if (_gameState.State != GameState.Gameplay) return;

            // Keyboard — works everywhere: Editor, PC build, WebGL
            float horizontal = Input.GetAxisRaw(AxisHorizontal);
            bool  fire       = Input.GetButton(ButtonFire);

            // Touch buttons — additive on top of keyboard
            // Only read if the button exists and is active
            if (_buttonHorizontal && _buttonHorizontal.gameObject.activeInHierarchy)
                horizontal += _buttonHorizontal.Axis;

            if (_buttonFire && _buttonFire.gameObject.activeInHierarchy)
                fire |= _buttonFire.IsPressed;

            _input.Horizontal = Mathf.Clamp(horizontal, -1f, 1f);
            _input.Fire = fire;
        }
    }
}