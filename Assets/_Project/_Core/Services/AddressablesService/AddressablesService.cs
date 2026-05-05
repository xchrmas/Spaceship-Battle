using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Loads and caches Addressable assets. Releases handles on Dispose.
    /// Chosen over Resources.Load because Addressables support async loading and don't bloat the build with unused assets.
    /// </summary>
    public class AddressablesService : IAssetService, IDisposable
    {
        private readonly Dictionary<string, object> _assets  = new();
        private readonly List<AsyncOperationHandle> _handles = new();

        /// <summary>
        /// Loads an asset and caches it. Safe to call multiple times with the same key, as it will only load once and return the cached asset on subsequent calls.
        /// </summary>
        public async UniTask Load<T>(string key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(key) || _assets.ContainsKey(key))
                return;

            try
            {
                var handle = Addressables.LoadAssetAsync<T>(key);
                _handles.Add(handle);
                var obj = await handle.ToUniTask(cancellationToken: cancellationToken);
                _assets.Add(key, obj);
            }
            catch (InvalidKeyException invalidKeyException)
            {
                Debug.LogException(invalidKeyException);
            }
        }

        /// <summary>
        /// Returns default if not loaded yet.
        /// </summary>
        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key) || !_assets.TryGetValue(key, out var obj))
                return default;

            return obj is T t ? t : default;
        }

        /// <summary>
        /// Releases all Addressable handles. Call when the game shuts down.
        /// </summary>
        public void Dispose()
        {
            foreach (var handle in _handles)
                Addressables.Release(handle);
        }
    }
}