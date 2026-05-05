using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceshipBattle.Core
{

    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        /// <summary>
        /// Adds a service to the registry. Overwrites if already exists.
        /// </summary>
        public static void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }

        /// <summary>
        /// Gets a service. Logs an error if missing —
        /// usually means Bootstrap didn't run before the scene started.
        /// </summary>
        public static T Get<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var s)) return (T)s;
            Debug.LogError($"[ServiceLocator] Not found: {typeof(T).Name}");
            return null;
        }


        public static bool TryGet<T>(out T service) where T : class
        {
            if (_services.TryGetValue(typeof(T), out var s))
            {
                service = (T)s;
                return true;
            }
            service = null;
            return false;
        }

        /// <summary>
        /// Clears all services. Call on scene unload or game restart.
        /// </summary>
        public static void Clear() => _services.Clear();
    }
}