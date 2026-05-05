using System.Threading;
using Cysharp.Threading.Tasks;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Loads and caches Addressable assets at runtime.
    /// All assets must be loaded before calling Get.
    /// </summary>
    public interface IAssetService
    {
        /// <summary>
        /// Loads an asset by its Addressable key. Safe to call multiple times —
        /// skips if already cached.
        /// </summary>
        UniTask Load<T>(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a cached asset. Returns default if not loaded yet.
        /// </summary>
        T Get<T>(string key);
    }
}