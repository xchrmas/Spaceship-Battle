using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceshipBattle.Helpers
{
    public abstract class StateReactor<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] List<T> _visibleInStates;

        CanvasGroup _canvasGroup;
        bool _initialized;

        protected abstract StateModel<T> Model { get; }

        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();

            if (Model == null)
            {
                Debug.LogError($"[StateReactor] Model is null on {gameObject.name}");
                return;
            }

            _initialized = true;
            Model.OnStateChanged += OnStateChanged;

            // Применяем текущее состояние
            ApplyVisibility(Model.State);
        }

        void OnDestroy()
        {
            if (_initialized && Model != null)
                Model.OnStateChanged -= OnStateChanged;
        }

        void OnStateChanged(T prev, T current)
        {
            ApplyVisibility(current);
        }

        void ApplyVisibility(T state)
        {
            if (_canvasGroup == null) return;

            bool visible = _visibleInStates.Contains(state);
            _canvasGroup.alpha          = visible ? 1f : 0f;
            _canvasGroup.interactable   = visible;
            _canvasGroup.blocksRaycasts = visible;
        }
    }
}