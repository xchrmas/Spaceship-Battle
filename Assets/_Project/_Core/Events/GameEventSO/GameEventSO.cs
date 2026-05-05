using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceshipBattle.Core
{
    [CreateAssetMenu(menuName = "SpaceshipBattle/Events/Game Event")]
    public class GameEventSO : ScriptableObject
    {
        readonly List<GameEventListener> _listeners = new();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener l) => _listeners.Add(l);
        public void UnregisterListener(GameEventListener l) => _listeners.Remove(l);
    }

    [CreateAssetMenu(menuName = "SpaceshipBattle/Events/Game Event Int")]
    public class GameEventIntSO : ScriptableObject
    {
        readonly List<System.Action<int>> _listeners = new();

        public void Raise(int value)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i](value);
        }

        public void RegisterListener(System.Action<int> l) => _listeners.Add(l);
        public void UnregisterListener(System.Action<int> l) => _listeners.Remove(l);
    }
}