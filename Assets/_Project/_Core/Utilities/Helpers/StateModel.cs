using System;

namespace SpaceshipBattle.Helpers
{
    public abstract class StateModel<T> where T : Enum
    {
        T _state;

        public event Action<T, T> OnStateChanged;

        public StateModel(T initialState)
        {
            _state = initialState;
        }

        public T State
        {
            get => _state;
            set
            {
                if (_state.Equals(value)) return;
                var prev = _state;
                _state = value;
                OnStateChanged?.Invoke(prev, value);
            }
        }
    }
}